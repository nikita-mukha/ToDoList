using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ToDoList.Domain.Entities;
using ToDoList.Infrastructure.Identity;

namespace ToDoList.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<ToDoItem> Items { get; set; }
    public DbSet<ToDoEvent> Events { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ToDoItem>()
            .HasDiscriminator<string>("Discriminator")
            .HasValue<ToDoTask>("Task")
            .HasValue<Call>("Call")
            .HasValue<Meeting>("Meeting")
            .HasValue<DateOfBirth>("DateOfBirth");
        var stringListConverter = new ValueConverter<List<string>, string>(
            v => string.Join(',', v),
            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

        modelBuilder.Entity<Call>()
            .Property(c => c.InvitedPerson)
            .HasConversion(stringListConverter);

        modelBuilder.Entity<Meeting>()
            .Property(m => m.InvitedPerson)
            .HasConversion(stringListConverter);
    }
}