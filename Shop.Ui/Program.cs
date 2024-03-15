using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Shop.Application.UsersAdmin;
using Shop.Database;
using System.Security.Claims;
using Shop.Domain.Models;
using Stripe;
using Shop.Ui.FileManager;
using Shop.Application.Infrastructure;
using Shop.Ui.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddDbContext<ApplicationDbContext>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging(); }) ;
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options=>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric= false;
} ).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Accounts/Login";
});
builder.Services.AddRazorPages();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("Role","Admin"));
    //options.AddPolicy("Manager", policy => policy.RequireClaim("Role","Manager"));
    options.AddPolicy("Manager", policy => policy
        .RequireAssertion(context =>
        context.User.HasClaim("Role", "Manager")
        || context.User.HasClaim("Role", "Admin")));

});


builder.Services.AddControllersWithViews().AddRazorPagesOptions(
    options => {
        options.Conventions.AuthorizeFolder("/Admin");
        options.Conventions.AuthorizePage("/Admin/ConfigureUsers","Admin");
        }
    
    ) ; 
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "Cart";
    options.Cookie.MaxAge=TimeSpan.FromDays(90);
});
builder.Services.AddTransient<ISessionManager,SessionManager>();

builder.Services.AddTransient<IFileManager, FileManager>();

builder.Services.AddApplicationServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<String>();

app.UseRouting();
app.UseSession();

app.MapDefaultControllerRoute();
app.UseCors("MyCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
try
{
    using(var scope=app.Services.CreateScope())
    {
       var context=scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        context.Database.EnsureCreated();

        if(!context.Users.Any())
        {
            var adminUser = new IdentityUser()
            {
                UserName = "Admin",

            };
            var managerUser = new IdentityUser()
            {
                UserName = "Manager"
            };
            userManager.CreateAsync(adminUser,"password").GetAwaiter().GetResult();
            userManager.CreateAsync(managerUser, "password").GetAwaiter().GetResult();

            var adminClaim = new Claim("Role", "Admin");
            var managerClaim = new Claim("Role", "Manager");


            userManager.AddClaimAsync(adminUser,adminClaim).GetAwaiter().GetResult() ;
            userManager.AddClaimAsync(managerUser, managerClaim).GetAwaiter().GetResult();

        }
    }
}
catch(Exception e)
{
    Console.WriteLine(e.Message);

}

app.Run();
