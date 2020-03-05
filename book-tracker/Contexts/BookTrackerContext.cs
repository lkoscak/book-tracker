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
    }
}
