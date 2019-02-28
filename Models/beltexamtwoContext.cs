using Microsoft.EntityFrameworkCore;

namespace beltExamTwo.Models
{
    public class beltexamtwoContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public beltexamtwoContext(DbContextOptions<beltexamtwoContext> options) : base(options) { }

        public DbSet<User> Users {get;set;}
        public DbSet<Hobby> Hobbies {get;set;}
        public DbSet<Association> Associations {get;set;}
    }
}