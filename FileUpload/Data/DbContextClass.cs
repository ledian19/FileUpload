using FileUpload.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileUpload.Data {
    public class DbContextClass : DbContext {
        private readonly IConfiguration _configuration;

        public DbContextClass(IConfiguration configuration) {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<FileDetails> FileDetails { get; set; }
    }
}