using ExpanseManagerAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpanseManagerAPI.Data;

public class ExpanseDbContext : DbContext
{
    public ExpanseDbContext(DbContextOptions<ExpanseDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<BankConnection> BankConnections => Set<BankConnection>();
    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Unique constraints that matter
        modelBuilder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<BankConnection>()
            .HasIndex(x => new { x.UserId, x.Provider, x.ProviderItemId })
            .IsUnique();

        modelBuilder.Entity<BankAccount>()
            .HasIndex(x => new { x.BankConnectionId, x.ProviderAccountId })
            .IsUnique();

        modelBuilder.Entity<Transaction>()
            .HasIndex(x => new { x.BankAccountId, x.ProviderTransactionId })
            .IsUnique();

        // Relationships (FKs)
        modelBuilder.Entity<BankConnection>()
            .HasOne(x => x.User)
            .WithMany(x => x.BankConnections)
            .HasForeignKey(x => x.UserId);

        modelBuilder.Entity<BankAccount>()
            .HasOne(x => x.BankConnection)
            .WithMany(x => x.BankAccounts)
            .HasForeignKey(x => x.BankConnectionId);

        modelBuilder.Entity<Transaction>()
            .HasOne(x => x.BankAccount)
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.BankAccountId);
    }
}
