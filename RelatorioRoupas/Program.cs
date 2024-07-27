using Microsoft.AspNetCore.Connections;
using RelatorioRoupas.Data;
using RelatorioRoupas.Endpoints.Loja;
using RelatorioRoupas.Endpoints.Marca;
using RelatorioRoupas.Endpoints.Produto;
using RelatorioRoupas.Endpoints.Tamanho;
using RelatorioRoupas.Models;
using RelatorioRoupas.Tipo_de_produto;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(new DbContext(connectionString));

// Add services to the container.
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

app.UseHttpsRedirection();

app.AddEndpointsMarcas();
app.AddTamanhoEndpoints();
app.AddTipoEndpoints();
app.AddLojaEndpoints();
app.AddProdutoEndpoints();

app.Run();

