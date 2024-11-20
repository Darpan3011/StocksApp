using Entities;
using ServiceContracts;
using Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using UnitTesting_UDEMY.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .WriteTo.Console();
});

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.Add(new ServiceDescriptor(
        typeof(IFinnhubService), typeof(FinnhubService), ServiceLifetime.Scoped
));
builder.Services.Add(new ServiceDescriptor(
        typeof(IStocksService), typeof(StocksService), ServiceLifetime.Scoped
));
builder.Services.AddDbContext<OrdersDbContext>(
        options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]!)
);

// for app.UseHttpLogging();
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestProperties |
        Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponsePropertiesAndHeaders;
});


var app = builder.Build();

app.UseSerilogRequestLogging();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
} else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionMiddleware();
}

// to see additional information related to http requests
app.UseHttpLogging();

Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

app.UseStaticFiles();
app.MapControllers();

app.Run();
