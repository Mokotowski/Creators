using Creators.Creators.Database;
using Creators.Creators.Models;
using Creators.Creators.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PayPal;
using PayPal.Api;
using Microsoft.Extensions.Logging;

namespace Creators.Creators.Controllers
{
    public class DonateController : Controller
    {
        private readonly IPaypalPay _paypalPay;
        private readonly IPaypalPayout _paypalPayout;
        private readonly UserManager<UserModel> _userManager;
        public readonly IDonatesInfo _donatesInfo;
        private readonly ILogger<DonateController> _logger;

        public DonateController(
            IPaypalPay paypalPay,
            IPaypalPayout paypalPayout,
            UserManager<UserModel> userManager,
            IDonatesInfo donatesInfo,
            ILogger<DonateController> logger)
        {
            _paypalPay = paypalPay;
            _paypalPayout = paypalPayout;
            _userManager = userManager;
            _donatesInfo = donatesInfo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> DonateCreator(string Creator, string Id_Donates)
        {
            ViewBag.Creator = Creator;
            ViewBag.Id_Donates = Id_Donates;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PaymentCancelled(string token)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PaymentSuccess(string paymentId, string token, string PayerID)
        {
            await _paypalPay.PaymentSuccessful(paymentId, token, await _userManager.GetUserAsync(User), HttpContext.Session.GetString("Id_Donates"));
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DonateCreator(int amount, string currency, string creator, string Id_Donates)
        {
            HttpContext.Session.SetString("Id_Donates", Id_Donates);
            try
            {
                string returnUrl = $@"https://localhost:7131/Donate/PaymentSuccess";
                string cancelUrl = $@"https://localhost:7131/Donate/PaymentCancelled";

                var createdPayment = await _paypalPay.CreatePayment(amount, returnUrl, cancelUrl, creator, currency);

                _logger.LogInformation("Payment created successfully with ID: {PaymentId}, Intent: {Intent}, State: {State}, Payer: {PayerMethod}",
                    createdPayment.id, createdPayment.intent, createdPayment.state, createdPayment.payer?.payment_method);

                foreach (var transaction in createdPayment.transactions)
                {
                    _logger.LogInformation("Transaction Amount: {Amount}, Currency: {Currency}, Description: {Description}",
                        transaction.amount.total, transaction.amount.currency, transaction.description);
                }

                foreach (var link in createdPayment.links)
                {
                    _logger.LogInformation("Link Rel: {Rel}, Link Href: {Href}", link.rel, link.href);
                }

                string approvalUrl = createdPayment.links.FirstOrDefault(p => p.rel.ToLower() == "approval_url")?.href;

                return Redirect(approvalUrl);
            }
            catch (PaymentsException ex)
            {
                _logger.LogError(ex, "Payment failed");

                if (ex.Response != null)
                {
                    _logger.LogError("Error Response: {ErrorResponse}", ex.Response);
                }
                else
                {
                    _logger.LogError("No response from PayPal");
                }

                return RedirectToAction("Error", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the donation process");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CheckDonates(string Id_Donates)
        {
            List<DonateCreatorForView> donates = await _donatesInfo.GetDonates(Id_Donates);
            CreatorBalance balance = await _donatesInfo.GetCreatorBalance(Id_Donates);

            ViewBag.Balance = balance.Balance;
            ViewBag.LastDeposit = balance.LastDeposit;
            ViewBag.LastCashout = balance.LastCashout;

            return View(donates);
        }

        [HttpGet]
        public async Task<IActionResult> Payout()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ErrorPayout()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SuccessPayout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Payout(decimal amount)
        {
            UserModel user = await _userManager.GetUserAsync(User);
            if (await _donatesInfo.CheckPayoutPossibility(amount, user))
            {
                string result = await _paypalPayout.CreatePayout(amount, user);

                if (result != null && result != "lackofresources")
                {
                    return RedirectToAction("SuccessPayout");
                }
                else
                {
                    return RedirectToAction("ErrorPayout");
                }
            }
            else
            {
                return RedirectToAction("ErrorPayout");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CheckSendDonates()
        {
            UserModel user = await _userManager.GetUserAsync(User);
            List<DonateUserForView> donates = await _donatesInfo.CheckSendDonates(user.Id);
            return View(donates);
        }
    }
}
