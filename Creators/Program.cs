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

builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


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
    options.User.RequireUniqueEmail = false;

}).AddEntityFrameworkStores<DatabaseContext>()
  .AddDefaultTokenProviders();




builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();


builder.Services.AddScoped<ILogout, AccountAuthenticationServies>();
builder.Services.AddScoped<IRegister, AccountAuthenticationServies>();
builder.Services.AddScoped<ILogin, AccountAuthenticationServies>();

builder.Services.AddScoped<ISendEmail, EmailActionsServies>();
builder.Services.AddScoped<IFunctionsFromEmail, EmailActionsServies>();
builder.Services.AddScoped<INotificationEmail, EmailActionsServies>();


builder.Services.AddScoped<IPageFunctions, CreatorPageServies>();
builder.Services.AddScoped<IPageFunctions, CreatorPageServies>();

builder.Services.AddScoped<IFollow, FollowersServices>();
builder.Services.AddScoped<IGetFollowers, FollowersServices>();

builder.Services.AddScoped<IScheduleData, ScheduleServices>();
builder.Services.AddScoped<IEventsFunctions, ScheduleServices>();

builder.Services.AddScoped<IPaypalPay, PaypalServices>();
builder.Services.AddScoped<IPaypalPayout, PaypalServices>();

builder.Services.AddScoped<IDonatesInfo, DonateServices>();


builder.Services.AddScoped<ILikes, PhotoActionsServices>();
builder.Services.AddScoped<IComments, PhotoActionsServices>();
builder.Services.AddScoped<IPhotosManage, PhotosManagerServices>();
builder.Services.AddScoped<IPhotoDataGet, PhotoDataServices>();

builder.Services.AddScoped<IBlock, BlockUserServices>();

builder.Services.AddScoped<IChatsActions, ChatServices>();

builder.Services.AddScoped<IUserData, UserDataServices>();

builder.Services.AddScoped<IAnnouncementData, AnnouncementServices>();
builder.Services.AddScoped<IAnnouncementManage, AnnouncementServices>();


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
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

// Map default controller route.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
