using JPLearn.Api.Extensions;
using JPLearn.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJPLearnInfrastructure(builder.Configuration);
builder.Services.AddFrontendCors(builder.Configuration, builder.Environment.IsDevelopment());

// === Controllers + Swagger ===
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// === Middleware Pipeline ===
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.MapControllers();

app.Run();
