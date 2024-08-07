using Creators.Creators.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Creators.Creators.Services;
using Creators.Creators.Services.Interface;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure Entity Framework and the database context.
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Configure Identity.
builder.Services.AddIdentity<UserModel, IdentityRole>(options =>
{
    // Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 2;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    // Sign-in settings.
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;

    // User settings.
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<DatabaseContext>()
  .AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
{
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddGoogle(options =>
{
    var googleAuth = builder.Configuration.GetSection("Authentication:Google");
    options.ClientId = googleAuth["ClientId"];
    options.ClientSecret = googleAuth["ClientSecret"];
})
.AddFacebook(options =>
{
    var facebookAuth = builder.Configuration.GetSection("Authentication:Facebook");
    options.AppId = facebookAuth["AppId"];
    options.AppSecret = facebookAuth["AppSecret"];
})
.AddMicrosoftAccount(options =>
{
    var microsoftAuth = builder.Configuration.GetSection("Authentication:Microsoft");
    options.ClientId = microsoftAuth["ClientId"];
    options.ClientSecret = microsoftAuth["ClientSecret"];
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ILogout, AccountAuthentication>();
builder.Services.AddScoped<IRegister, AccountAuthentication>();
builder.Services.AddScoped<ILogin, AccountAuthentication>();
builder.Services.AddScoped<ISendEmail, EmailActions>();
builder.Services.AddScoped<IFunctionsFromEmail, EmailActions>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. For production, consider changing this value.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Map default controller route.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
