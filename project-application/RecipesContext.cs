using Microsoft.EntityFrameworkCore;
using recipes;
using users;

public class RecipesContext : DbContext
{
    public virtual DbSet<Recipe> RecipeManager_Recipes {get; set;}
    public virtual DbSet<User> RecipeManager_Users {get; set;}
    public virtual DbSet<Ingredient> RecipeManager_Ingredients {get; set;}
    
    public virtual DbSet<Tag> RecipeManager_Tags {get; set;}
    public virtual DbSet<DifficultyRating> RecipeManager_DifficultyRatings {get; set;}
    public virtual DbSet<Rating> RecipeManager_Ratings {get; set;}
    public virtual DbSet<Instruction> RecipeManager_Instructions {get; set;}
    public virtual DbSet<MeasuredIngredient> RecipeManager_MeasuredIngredients {get; set;}
    public string HostName { get; set; }

    public string Port { get; set; }

    public string ServiceName { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    private static RecipesContext? _instance {get; set;}


    public static RecipesContext Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RecipesContext();
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recipe>()
            .HasMany(bc => bc.UserFavourite)
            .WithMany(b => b.UserFavoriteRecipies);
        modelBuilder.Entity<User>()
            .HasMany(bc => bc.UserFavoriteRecipies)
            .WithMany(c => c.UserFavourite);
        modelBuilder.Entity<User>()
            .HasMany(r => r.UserCreatedRecipies)
            .WithOne( u => u.Owner)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
