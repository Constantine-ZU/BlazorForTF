//using BlazorForTF.Data;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.Web;
//using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.HttpOverrides;

using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);
string pfxFilePath = "";

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    pfxFilePath = @"C:\Users\1\source\repos\cert_webapi_pan4_com\20240808_43c3e236.pfx";
}
else 
{
    pfxFilePath = "/etc/ssl/certs/20240808_43c3e236.pfx";
}


//  Kestrel for HTTPS
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(80); 
    serverOptions.ListenAnyIP(443, listenOptions =>
    {
        listenOptions.UseHttps(pfxFilePath);
    });
});


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<SarService>();
builder.Services.AddSingleton<SarCommandValidator>();
builder.Services.AddScoped<SarServiceList>();

builder.Services.AddRazorComponents();
builder.Services.AddHttpContextAccessor();
builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrapProviders()
    .AddFontAwesomeIcons();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (true) //(!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseMiddleware<ClientInfoMiddleware>();

app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();