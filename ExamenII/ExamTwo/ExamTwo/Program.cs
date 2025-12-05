using ExamTwo.Application.Ports;
using ExamTwo.Application.UseCases;
using ExamTwo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

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
