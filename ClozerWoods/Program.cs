using ClozerWoods.Models;
using ClozerWoods.Models.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddControllersWithViews();
services.AddDbContext<ApplicationDbContext>();
services.AddTransient<IUserRepository, UserRepository>();
services.AddTransient<IPageRepository, PageRepository>();
services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.SlidingExpiration = true;
        options.LoginPath = "/subgate/login";
    });

var app = builder.Build();

if(!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

// This is for reverse proxy configuration
app.UseForwardedHeaders(new ForwardedHeadersOptions {
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy(new CookiePolicyOptions {
    MinimumSameSitePolicy = SameSiteMode.Strict,
});
app.MapControllers();

app.Run();
