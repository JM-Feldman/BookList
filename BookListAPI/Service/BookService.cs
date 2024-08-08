using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using BookListAPI.Models;
using Microsoft.Extensions.Configuration;

namespace BookListAPI.Services
{
    public class BookService
    {
        private readonly string _filePath;

        public BookService(IConfiguration configuration)
        {
            _filePath = configuration.GetValue<string>("BooksFilePath");
        }

        public List<Book> GetBooks()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Book>();
            }

            var jsonData = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Book>>(jsonData);
        }

        public void SaveBooks(List<Book> books)
        {
            var jsonData = JsonSerializer.Serialize(books);
            File.WriteAllText(_filePath, jsonData);
        }
    }
}

