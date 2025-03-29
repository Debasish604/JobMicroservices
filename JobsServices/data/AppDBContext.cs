
using JobsServices.models.Entity;
using Microsoft.EntityFrameworkCore;

namespace JobServices.data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Job> Jobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Explicitly map table name to "Job" (case-sensitive in PostgreSQL)
            modelBuilder.Entity<Job>().ToTable("Job", schema: "ADANI_TALENT");
        }

       

    }
}