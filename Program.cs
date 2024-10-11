using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PizzaHotApi.Data;
using PizzaHotApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("pizzadb"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(
    c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pizza")
  );
}
app.MapGet("/pizzas", async (AppDbContext db) => await db.Pizzas.ToListAsync());
app.MapGet("/pizza/{id}", async(AppDbContext db, int id) => await db.Pizzas.FindAsync(id));
app.MapPut("/pizza/{id}", async (AppDbContext db, Pizza updatepizza, int id) =>
{
var pizza = await db.Pizzas.FindAsync(id);
if (pizza is null) return
Results.NotFound();
pizza.Nome = updatepizza.Nome;
pizza.Descricao = updatepizza.Descricao;
pizza.Quantidade = updatepizza.Quantidade;
await db.SaveChangesAsync();
return Results.NoContent();
});

app.MapDelete("/pizza/{id}", async
(AppDbContext db, int id) =>
{
var pizza = await db.Pizzas.FindAsync(id);
if (pizza is null)
{
return Results.NotFound();
}
db.Pizzas.Remove(pizza);
await db.SaveChangesAsync();
return Results.Ok();
});



app.MapPost("/pizza", async (AppDbContext db, Pizza pizza) =>

{
  await db.Pizzas.AddAsync(pizza);
  await db.SaveChangesAsync();
  return Results.Created($"/pizza/{pizza.Id}", pizza);
});
app.Run();