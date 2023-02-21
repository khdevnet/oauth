using Microsoft.IdentityModel.Tokens;
using SPA.Controllers;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5001";

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        ForecastPolicy.ReaderResourcePermission.PolicyName,
        policy => policy
           .RequireClaim(ForecastPolicy.ReaderResourcePermission.UserForecastReadPermission)
           .RequireClaim("scope", ForecastPolicy.ReaderResourcePermission.ForecastReadScope));
    
    options.AddPolicy(
       ForecastPolicy.ExternalReaderResourcePermission.PolicyName,
       policy => policy
          .RequireClaim(ForecastPolicy.ExternalReaderResourcePermission.UserForecastReadPermission)
          .RequireClaim("scope", ForecastPolicy.ExternalReaderResourcePermission.ForecastExternlReadScope));
});

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
