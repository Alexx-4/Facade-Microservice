using FacadeMicroservice.Core.Services;
using FacadeMicroservice.Logic.Jobs;
using FacadeMicroservice.Logic.Services;
using Hangfire;
using Hangfire.MemoryStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IConvertCurrency, ConvertCurrency>();
builder.Services.AddSingleton<IExchangeRateCache, ExchangeRateCache>();

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

builder.Services.AddHttpClient();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(config =>
{
    config.UseMemoryStorage();
});
builder.Services.AddHangfireServer();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseHangfireDashboard();

RecurringJob.AddOrUpdate<UpdateExchangeRatesCache>(nameof(UpdateExchangeRatesCache), obj => obj.Run(), "*/5 * * * *");
RecurringJob.TriggerJob(nameof(UpdateExchangeRatesCache));

app.MapControllers();

app.Run();
