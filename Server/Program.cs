using Microsoft.AspNetCore.Authentication.Cookies;
using Server.Models;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<AppDbContext>();
builder.Services.AddCors();
builder.Services.AddAuthentication("Main").AddCookie("Main", options =>
{
    //options.LoginPath = "/Account/login";
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.IsEssential = true;
    //options.Cookie.MaxAge = new TimeSpan(0, 1, 0);
    //options.ExpireTimeSpan = new TimeSpan(0, 1, 0);

});
builder.Services.AddAuthorization(
    options =>
{
    options.AddPolicy("Admin", builder =>
    {
        builder.RequireAssertion(x =>
        {
            var identity = x.User.Identities.Where(x => x.HasClaim("MyRole", "Admin")).ToList();
            if (identity.Count > 0)
            {
                return true;
            }
            return false;

        });
        //builder.RequireClaim(ClaimsIdentity.DefaultRoleClaimType, "Admin");
        //x.User.HasClaim(ClaimsIdentity.DefaultRoleClaimType, "Admin"));
        //builder.RequireRole(ClaimsIdentity.DefaultRoleClaimType);
    });
}
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors(builder =>
{
    builder.SetIsOriginAllowed(x => true);
    builder.AllowAnyMethod();
    builder.AllowAnyHeader();
    builder.AllowCredentials();
    }
);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Register}/register");
app.Run();
