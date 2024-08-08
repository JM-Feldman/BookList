using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BookListAPI.Models;
using BookListAPI.Services;

namespace BookListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetBooks(
        [FromQuery] string title = null,
        [FromQuery] string author = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
        {
            var books = _bookService.GetBooks();

            if (!string.IsNullOrEmpty(title))
            {
                books = books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(author))
            {
                books = books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var paginatedBooks = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Ok(paginatedBooks);
        }


        [HttpGet("{id}")]
        public ActionResult<Book> GetBook(int id)
        {
            var book = _bookService.GetBooks().FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public ActionResult<Book> CreateBook([FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var books = _bookService.GetBooks();
            book.Id = books.Any() ? books.Max(b => b.Id) + 1 : 1;
            books.Add(book);
            _bookService.SaveBooks(books);

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateBook(int id, [FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var books = _bookService.GetBooks();
            var existingBook = books.FirstOrDefault(b => b.Id == id);

            if (existingBook == null)
            {
                return NotFound();
            }

            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.ISBN = book.ISBN;
            existingBook.PublishedDate = book.PublishedDate;

            _bookService.SaveBooks(books);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteBook(int id)
        {
            var books = _bookService.GetBooks();
            var book = books.FirstOrDefault(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            books.Remove(book);
            _bookService.SaveBooks(books);
            return NoContent();
        }
    }
}
