using ContactBus.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ContactBus.Data
{
    public class ContactDbContext : DbContext
    {
        public ContactDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Contact> contacts { get; set; }
    }
}
