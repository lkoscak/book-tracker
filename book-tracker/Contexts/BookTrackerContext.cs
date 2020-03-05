using book_tracker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace book_tracker.Contexts
{
    public class BookTrackerContext : DbContext
    {
        public BookTrackerContext(DbContextOptions<BookTrackerContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rating>()
                .Property<DateTime>("RatingGiven");

            
        }
    }
}
