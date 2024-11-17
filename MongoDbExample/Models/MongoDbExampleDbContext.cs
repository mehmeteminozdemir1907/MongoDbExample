using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace MongoDbExample.Models
{
    public class MongoDbExampleDbContext : DbContext
    {
        public MongoDbExampleDbContext(DbContextOptions<MongoDbExampleDbContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Todo>().ToCollection("Todos");
        }

        public DbSet<Todo> Todos { get; set; }
    }
}