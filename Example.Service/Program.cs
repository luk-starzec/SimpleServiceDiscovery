using Example.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<DiscoveryRegistrationHelper>();

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddGrpc();
builder.Services.AddMagicOnion();

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

var helper = app.Services.GetRequiredService<DiscoveryRegistrationHelper>();
await helper.RegisterInServiceDiscoveryAsync();

app.WaitForShutdown();