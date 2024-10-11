using Microsoft.EntityFrameworkCore;
using PizzaHotApi.Models;

namespace PizzaHotApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) {}
    public DbSet <Pizza> Pizzas { get; set; } = null!;
}