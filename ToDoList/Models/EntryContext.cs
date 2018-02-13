using Microsoft.EntityFrameworkCore;

namespace ToDoList.Models
{
    public class EntryContext : DbContext
    {
        public EntryContext(DbContextOptions<EntryContext> options) : base(options)
        {
        }

        public DbSet<Entry> Entry { get; set; }
    }
}
