using Microsoft.EntityFrameworkCore;
using recipes;
using users;

public class RecipesContext : DbContext
{
    public DbSet<Recipe> Recipes {get; set;}
    public DbSet<User> Users {get; set;}
    public DbSet<Ingredient> Ingredients {get; set;}
    public DbSet<Password> Passwords {get; set;}
    
    public string HostName { get; set; }

    public string Port { get; set; }

    public string ServiceName { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public RecipesContext()
    {
        HostName = Environment.GetEnvironmentVariable("ORACLE_DB_HOST")!;

        Port = Environment.GetEnvironmentVariable("ORACLE_DB_PORT") ?? "1521";

        ServiceName = Environment.GetEnvironmentVariable("ORACLE_DB_SERVICE")!;

        UserName = Environment.GetEnvironmentVariable("ORACLE_DB_USER")!;

        Password = Environment.GetEnvironmentVariable("ORACLE_DB_PASSWORD")!;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseOracle($"Data Source={HostName}:{Port}/{ServiceName}; " +
        $"User Id={UserName}; Password={Password}");
    }

}
