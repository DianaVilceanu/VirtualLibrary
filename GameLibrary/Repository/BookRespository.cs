using Microsoft.EntityFrameworkCore;
using GameLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.Repository
{
    public class BookRepository : IBookRepository
    {
        private StoreContext context;
        private DbSet<Book> bookEntity;

        IEnumerable<Book> IBookRepository.Books
        {
            get
            {
                return context.Books ;

            }
            set { }
        }


        public BookRepository(StoreContext context)
        {
            this.context = context;
            bookEntity = context.Set<Book>();
        }


        public void SaveBook(Book book)
        {
            context.Entry(book).State = EntityState.Added;
            context.SaveChanges();
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return bookEntity.AsEnumerable();
        }

        public Book GetBook(long id)
        {
            return bookEntity.SingleOrDefault(s => s.BookID == id);
        }
        public void DeleteBook(long id)
        {
            Book book = GetBook(id);
            bookEntity.Remove(book);
            context.SaveChanges();
        }
        public void UpdateBook(Book book)
        {
            context.SaveChanges();
        }

    }
}
