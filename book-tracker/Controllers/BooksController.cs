using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using book_tracker.Contexts;
using book_tracker.Models;

namespace book_tracker.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookTrackerContext _context;

        public BooksController(BookTrackerContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.Include(b => b.Author).Include(b => b.BookRatings).ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {

            var modelViewBook = new BooksViewModel
            {
                Authors = _context.Authors.Select(a =>
                                  new SelectListItem
                                  {
                                      Value = a.AuthorID.ToString(),
                                      Text = a.AuthorsName
                                  }).ToList()
            };

            return View(modelViewBook);
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookID,Title,Description,NumberOfPages,ImageURL,BookDepoURL, AuthorID, Review")] BooksViewModel book)
        {
            if (ModelState.IsValid)
            {
                var newBook = new Book();
                _context.Add(newBook).CurrentValues.SetValues(book);
                var newRating = new Rating();
                _context.Add(newRating).CurrentValues.SetValues(book);
                var author = await _context.Authors.FindAsync(book.AuthorID);
                if (author == null)
                {
                    author = new Author();
                    author.AuthorsName = "test";
                }

                newBook.Author = author;
                _context.Entry(newRating).Property("BookID").CurrentValue = newBook.BookID;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookID,Title,Description,NumberOfPages,ImageURL,BookDepoURL")] Book book)
        {
            if (id != book.BookID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                  
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var query = _context.Books.Where(b => b.BookID == id).Include(b => b.Author).Include(b => b.BookRatings);
            var book = await query.SingleAsync();

            
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.Include(m => m.BookRatings)
                .FirstOrDefaultAsync(m => m.BookID == id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAuthor(Author author)
        {

            if (ModelState.IsValid)
            {

                _context.Add(author);
                await _context.SaveChangesAsync();
            }
               
            return RedirectToAction("Create");
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookID == id);
        }
    }
}
