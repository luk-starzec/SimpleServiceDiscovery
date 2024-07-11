using Discovery.Service.Interfaces;
using Discovery.Service.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<WatchDogBackgroundService>();
builder.Services.AddTransient<IWatchDog, WatchDog>();
builder.Services.AddSingleton<ISimpleDiscovery, SimpleDiscovery>();

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/hc");

app.Run();
