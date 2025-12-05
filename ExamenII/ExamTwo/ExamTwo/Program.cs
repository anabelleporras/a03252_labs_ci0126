using ExamTwo.Application.Ports;
using ExamTwo.Application.UseCases;
using ExamTwo.Infrastructure;
using ExamTwo.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowVueClient", policy =>
  {
    policy
        .WithOrigins("http://localhost:8080") // tu frontend
        .AllowAnyHeader()
        .AllowAnyMethod();
  });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<CoffeeMachineDataStore>();

builder.Services.AddScoped<ICoffeeMachineQuery, CoffeeMachineQuery>();
builder.Services.AddScoped<ICoffeeMachineCommand, CoffeeMachineCommand>();
builder.Services.AddScoped<ICoffeeMachineRepository, CoffeeMachineRepository>();


var app = builder.Build();

app.UseCors("AllowVueClient");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
