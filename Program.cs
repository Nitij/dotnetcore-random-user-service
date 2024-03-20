using Microsoft.AspNetCore.Authentication;
using RandomUserService.Auth;

var builder = WebApplication.CreateBuilder(args);

var basicAuthSettings = builder.Configuration.GetSection("BasicAuth").Get<BasicAuthSettings>();
builder.Services.AddSingleton(basicAuthSettings);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CustomOrigins",
        builder =>
        { builder.WithOrigins(["http://localhost:3000"]).AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
});

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
builder.Services.AddHealthChecks();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CustomOrigins");
app.UseHttpsRedirection();
app.MapHealthChecks("api/health");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
