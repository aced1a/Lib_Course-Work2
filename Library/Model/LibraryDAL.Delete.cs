using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Query;
using Library.Models;

namespace Library
{
    partial class LibraryDAL
    {
        //Удалять ли рассказы, если они не содержатся в иных книгах?
        public bool DeleteBook(Book book) 
        {
            return false; 
        }
        public bool DeleteAuthor(Author author) 
        {
            try
            {
                context.Authors.Remove(author);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteGenre(Genre genre) { return false; }
        public bool DeletePublisher(Publisher publisher) { return false; }
        public bool DeleteBindingType(BindingType type) { return false; }
        public bool DeleteCoverType(CoverType type) { return false; }
        public bool DeleteStory(Story story) { return false; }
    }
}
