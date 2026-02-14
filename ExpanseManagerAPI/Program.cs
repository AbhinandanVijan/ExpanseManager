using ExpanseManagerAPI.Data.Repositories;
using ExpanseManagerAPI.Interfaces.Repositories;
using ExpanseManagerAPI.Interfaces.Services;
using ExpanseManagerAPI.Services;
using ExpanseManagerAPI.Security;
using ExpanseManagerAPI.Interfaces.Security;
using ExpanseManagerAPI.Interfaces.Providers;
using ExpanseManagerAPI.Providers;
using Microsoft.EntityFrameworkCore;
using ExpanseManagerAPI.Data;
using Going.Plaid;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ExpanseDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddScoped<IBankConnectionService, BankConnectionService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.AddScoped<IBankConnectionRepository, BankConnectionRepository>();
builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

builder.Services.AddScoped<IBankDataProviderClient, PlaidBankDataProviderClient>();
builder.Services.AddScoped<ITokenEncryptionService, AesTokenEncryptionService>();

builder.Services.AddPlaid(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run(); 