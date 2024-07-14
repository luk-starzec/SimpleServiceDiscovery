using Example.Service.Interfaces;
using Example.Service.Services;
using Example.Service.Utils;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IIdentityService, IdentityService>();
builder.Services.AddTransient<IDiscoveryRegistrationService, DiscoveryRegistrationService>();
builder.Services.AddSingleton<IAvailabilityService, AvailabilityService>();

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks()
    .AddCheck<BasicHealthCheck>("Basic");
builder.Services.AddGrpc();
builder.Services.AddMagicOnion();

var parameters = new Dictionary<string, string>();
if (args.Length > 0)
{
    for (int i = 0; i < args.Length - 1; i += 2)
    {
        var key = args[i].TrimStart('-').ToLower();
        var value = args[i + 1];
        parameters.Add(key, value);
    }
}

// port passed as argument
if (parameters.ContainsKey("port"))
{
    var port = int.Parse(parameters["port"]);
    builder.WebHost.ConfigureKestrel((context, serverOptions) =>
    {
        serverOptions.Listen(IPAddress.Loopback, port, listenOptions =>
        {
            listenOptions.UseHttps();
        });
    });
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapMagicOnionService();

app.MapHealthChecks("/hc");

app.Start();

// service name passed as argument
if (parameters.ContainsKey("name"))
{
    var identityService = app.Services.GetRequiredService<IIdentityService>();
    identityService.Name = parameters["name"];
}

var useServiceDiscovery = app.Configuration.GetValue<bool>("UseServiceDiscovery", true);
if (parameters.ContainsKey("usediscovery"))
    useServiceDiscovery = bool.Parse(parameters["usediscovery"]);

if (useServiceDiscovery)
{
    var discoveryService = app.Services.GetRequiredService<IDiscoveryRegistrationService>();
    await discoveryService.RegisterInServiceDiscoveryAsync();

    app.Lifetime.ApplicationStopped.Register(async () => await discoveryService.RemoveFromServiceDiscoveryAsync());
}

app.WaitForShutdown();