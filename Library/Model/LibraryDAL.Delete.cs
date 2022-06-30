using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Query;
using Library.Model.LibraryEntities;

namespace Library
{
    partial class LibraryDAL
    {
        public bool DeleteBook(Book book) 
        {
            context.Books.Remove(book);
            try 
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteAuthor(Author author) 
        {
            context.Authors.Remove(author);
            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteGenre(Genre genre) 
        {
            context.Genres.Remove(genre);
            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DeletePublisher(Publisher publisher) 
        {
            context.Publishers.Remove(publisher);
            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DeleteBindingType(BindingType type) 
        {
            context.BindingTypes.Remove(type);
            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DeleteCoverType(CoverType type) 
        {
            context.CoverTypes.Remove(type);
            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool DeleteStory(Story story) 
        {
            context.Stories.Remove(story);
            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteLocation(Location location)
        {
            context.Locations.Remove(location);
            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
