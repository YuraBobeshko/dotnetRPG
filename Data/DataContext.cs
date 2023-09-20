namespace dotnetRPG.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    public DataContext() { }
    public DbSet<Character> Characters => Set<Character>();
}