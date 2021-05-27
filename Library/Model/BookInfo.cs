using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


using Library.Models;

namespace Library.Model
{
    class BookInfo
    {
        Book Book { get; set; }
        IEnumerable<Author> Authors { get; set; }
        IEnumerable<Genre> Genres { get; set; }
        Image Image { get; set; }

        public BookInfo(Book book)
        {
            Book = book;
            Authors = from item in book.BookAuthor select item.Author;
            Genres = from item in book.BookGenre select item.Genre;
            Image = Book.Cover?.Image;
            if (Image == null) { 
                
            }
        }

        private void DrawImage()
        {
            
        }

        public string AuthorsText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Авторы: ");
                int i = 0, min = Math.Min(3, Authors.Count());
                foreach (var author in Authors)
                {
                    sb.Append(author.FullName); i++;
                    if (i >= min) break;
                    sb.Append(", ");
                }
                if (min > 3) sb.Append(" ...");
                return sb.ToString();
            }
        }


    }
}
