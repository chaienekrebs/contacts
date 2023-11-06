using Contacts.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Persistence
{
    public class ContactsDbContext : DbContext
    {
        public ContactsDbContext(DbContextOptions<ContactsDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseNpgsql(@"User ID=postgres;Password=123456;Host=localhost;Port=5435;Database=contact_db;");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactsDbContext).Assembly);
        }
        public DbSet<Person> Person { get; set; }
        public DbSet<ContactType> ContactType { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<Setting> Setting { get; set; }
    }
}