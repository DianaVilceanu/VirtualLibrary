using GameLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary
{
    public interface IBookRepository
    {
        IEnumerable<Book> Books { get; set; }
        void SaveBook(Book book);
        IEnumerable<Book> GetAllBooks();
        Book GetBook(long id);
        void DeleteBook(long id);
        void UpdateBook(Book book);
    }
}
