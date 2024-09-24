using Creators.Creators.Database;
using Creators.Creators.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PayPal;
using PayPal.Api;
using System;
using System.Globalization;
using System.Net.Http;

namespace Creators.Creators.Services
{
    public class PaypalServices : IPaypalPay, IPaypalPayout
    {
        private readonly APIContext _apiContext;
        private readonly IConfiguration _configuration;
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger<PaypalServices> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _exchangeRateApiUrl;
        private readonly decimal _percentDiff;

        public PaypalServices(IConfiguration configuration, DatabaseContext databaseContext, ILogger<PaypalServices> logger, HttpClient httpClient)
        {
            _configuration = configuration;
            _databaseContext = databaseContext;
            _logger = logger;
            _httpClient = httpClient;

            var clientId = _configuration["PayPal:ClientId"];
            var clientSecret = _configuration["PayPal:ClientSecret"];
            var paypalmode = _configuration["PayPal:Mode"];
            _exchangeRateApiUrl = _configuration["Rates:Api"];
            _percentDiff = decimal.Parse(_configuration["Rates:PercentDiff"]);

            var config = new Dictionary<string, string>
            {
                { "mode", paypalmode },
                { "clientId", clientId },
                { "clientSecret", clientSecret }
            };

            var accessToken = new OAuthTokenCredential(clientId, clientSecret, config).GetAccessToken();
            _apiContext = new APIContext(accessToken);
        }

        public async Task<Payment> CreatePayment(int amount, string returnUrl, string cancelUrl, string creator, string currency)
        {
            try
            {
                _logger.LogInformation($"Creating PayPal payment for {creator} with amount: {amount} {currency}");

                var transaction = new Transaction
                {
                    amount = new Amount
                    {
                        currency = currency,
                        total = amount.ToString(),
                        details = new Details { subtotal = amount.ToString() }
                    },
                    description = $"Donate for {creator}"
                };

                var payment = new Payment
                {
                    intent = "sale",
                    payer = new Payer { payment_method = "paypal" },
                    redirect_urls = new RedirectUrls
                    {
                        return_url = returnUrl,
                        cancel_url = cancelUrl
                    },
                    transactions = new List<Transaction> { transaction }
                };

                var createdPayment = payment.Create(_apiContext);
                _logger.LogInformation($"PayPal payment created successfully with ID: {createdPayment.id}");

                return createdPayment;
            }
            catch (PayPalException ex)
            {
                _logger.LogError(ex, $"PayPal error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while creating the PayPal payment.");
                throw;
            }
        }

        public async Task PaymentSuccessful(string paymentId, string token, UserModel user, string Id_Donates)
        {
            try
            {
                _logger.LogInformation($"Processing successful payment for Payment ID: {paymentId}");

                var payment = Payment.Get(_apiContext, paymentId);
                var transaction = payment.transactions.FirstOrDefault();
                var amountString = transaction?.amount.total;
                var currency = transaction?.amount.currency;

                if (decimal.TryParse(amountString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amountDecimal))
                {
                    int amount = (int)Math.Floor(amountDecimal);

                    var page = _databaseContext.CreatorPage.Single(p => p.Id_Donates == Id_Donates);
                    var donate = new Donates
                    {
                        Id_Donates = Id_Donates,
                        Donator = user.Id,
                        DateTime = DateTime.Now,
                        Count = amount,
                        Currency = currency,
                        PaymentId = paymentId,
                        CreatorPage = page
                    };

                    var balance = _databaseContext.CreatorBalance.Find(Id_Donates);
                    var rate = await GetExchangeRate(currency);

                    balance.Balance += (1 - page.Site_Commission) * amountDecimal * (1 - _percentDiff) / rate;
                    balance.LastDeposit = DateTime.Now;

                    _databaseContext.Donates.Add(donate);
                    _databaseContext.SaveChanges();

                    _logger.LogInformation($"Donation successfully processed for user {user.Id} and creator {page.Id_Creator}");
                }
                else
                {
                    _logger.LogError("Failed to parse the amount as a decimal.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while processing the payment for Payment ID: {paymentId}");
            }
        }

        private async Task<decimal> GetExchangeRate(string currencyCode)
        {
            try
            {
                _logger.LogInformation($"Fetching exchange rate for currency: {currencyCode}");
                var response = await _httpClient.GetAsync(_exchangeRateApiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error fetching exchange rate: {response.StatusCode}");
                    throw new Exception("Failed to retrieve exchange rate data.");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var exchangeData = JsonConvert.DeserializeObject<ExchangeRateResponse>(responseContent);

                if (exchangeData.Rates.TryGetValue(currencyCode, out var exchangeRate))
                {
                    _logger.LogInformation($"Fetched exchange rate for {currencyCode}: {exchangeRate}");
                    return exchangeRate;
                }
                else
                {
                    _logger.LogWarning($"Exchange rate for {currencyCode} not found.");
                    throw new Exception("Exchange rate not found for the specified currency.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching the exchange rate for {currencyCode}");
                throw;
            }
        }

        public async Task<string> CreatePayout(decimal amount, UserModel user)
        {
            try
            {
                _logger.LogInformation($"Initiating PayPal Payout to {user.Email} for {amount} USD");

                var payout = new Payout
                {
                    sender_batch_header = new PayoutSenderBatchHeader
                    {
                        sender_batch_id = Guid.NewGuid().ToString(),
                        email_subject = "You have a payment from us!"
                    },
                    items = new List<PayoutItem>
                    {
                        new PayoutItem
                        {
                            recipient_type = PayoutRecipientType.EMAIL,
                            amount = new Currency
                            {
                                value = amount.ToString(CultureInfo.InvariantCulture),
                                currency = "USD"
                            },
                            receiver = user.Email,
                            note = $"Payout initiated for {amount} USD",
                            sender_item_id = Guid.NewGuid().ToString()
                        }
                    }
                };

                var createdPayout = payout.Create(_apiContext, false);
                _logger.LogInformation($"Payout created successfully with batch ID: {createdPayout.batch_header.payout_batch_id}");

                var balance = _databaseContext.CreatorBalance.Single(p => p.Id_Creator == user.Id);
                balance.LastCashout = DateTime.Now;
                balance.Balance -= amount;

                _databaseContext.SaveChanges();

                return createdPayout.batch_header.payout_batch_id;
            }
            catch (PayPalException ex) when (ex.Message.Contains("422"))
            {
                _logger.LogError($"PayPal Payout error (422): {ex.Message}. Possible lack of resources.");
                return null;
            }
            catch (PayPalException ex)
            {
                _logger.LogError(ex, $"PayPal Payout error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred during the payout for {user.Email}");
                throw;
            }
        }

        internal class ExchangeRateResponse
        {
            public Dictionary<string, decimal> Rates { get; set; }
        }
    }
}
