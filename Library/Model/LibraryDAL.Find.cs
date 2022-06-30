using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Model.LibraryEntities;
using Library.Query;

namespace Library
{
    partial class LibraryDAL : IDisposable
    {
        LibraryEntities context;

        public LibraryDAL()
        {
            string name = $"name={System.Configuration.ConfigurationManager.AppSettings["currentConnection"] ?? "LibraryEntities"}";
            LibraryEntities.Init();
            context = new LibraryEntities(name);
        }

        public IEnumerable<Book> FindBook(FindBookQuery query)
        {
            IEnumerable<Book> results = null;

            if (query.Book != null) results = FindBookByInfo(query.Book);
           
            if (query.BeginYear != null || query.EndYear != null) results = FindBookByYear(query, results);
            if (query.Genres != null) results = FindBookByGenres(query.Genres, results);
            if (query.Stories != null) results = FindBookByStory(query.Stories, results);
            if (query.Authors != null) results = FindBookByAuthor(query.Authors, results);
            if (query.Publishers != null) results = FindBookByPublisher(query.Publishers, results);

            return results ?? context.Books;
        }

        IQueryable<Book> FindBookByInfo(Book book)
        {
            IQueryable<Book> books = null;

            if (book.Title != null)
                books = from item in (books ?? context.Books) where item.Title.ToLower().Contains(book.Title.ToLower()) select item;
            if (book.Year != null)
                books = from item in (books ?? context.Books) where item.Year == book.Year select item;
            if (book.ISBN != null)
                books = from item in (books ?? context.Books) where item.ISBN == book.ISBN select item;
            if (book.CoverTypeID != null)
                books = from item in (books ?? context.Books) where item.CoverTypeID == book.CoverTypeID select item;
            if (book.BindingTypeID != null)
                books = from item in (books ?? context.Books) where item.BindingTypeID == book.BindingTypeID select item;
            if (book.LocationID != null)
                books = from item in (books ?? context.Books) where item.LocationID == book.LocationID select item;

            return books;
        }

        IEnumerable<Book> FindBookByYear(FindBookQuery query, IEnumerable<Book> books =null) {
            query.BeginYear = query.BeginYear ?? query.EndYear;
            query.EndYear = query.EndYear ?? query.BeginYear;

            return from item in (books ?? context.Books) where item.Year >= query.BeginYear && item.Year <= query.EndYear select item;
        }
		
        IEnumerable<Book> FindBookByGenres(IEnumerable<Genre> genres, IEnumerable<Book> books = null)
        {
            var res = context.BookGenres.AsEnumerable().Join(genres, bg => bg.GenreID, g => g.ID, (bg, g) => bg.Book).Distinct();
            return books != null ? books.Join(res, b => b.ID, i => i.ID, (b, i) => b) : res;
        }

        IEnumerable<Book> FindBookByAuthor(IEnumerable<Author> authors, IEnumerable<Book> books = null)
        {
            var res = context.BookAuthors.AsEnumerable().Join(authors, ba => ba.AuthorID, a => a.ID, (ba, a) => ba.Book).Distinct();
            return books != null ? books.Join(res, b => b.ID, i => i.ID, (b, i) => b) : res;

        }

        IEnumerable<Book> FindBookByStory(IEnumerable<Story> stories, IEnumerable<Book> books = null)
        {
            var res = context.BookStories.AsEnumerable().Join(stories, ab => ab.StoryID, s => s.ID, (ab, s) => ab.Book).Distinct();
            return books != null ? books.Join(res, b => b.ID, i => i.ID, (b, i) => b) : res;

        }

        IEnumerable<Book> FindBookByPublisher(IEnumerable<Publisher> publishers, IEnumerable<Book> books = null) 
        {
            var res = context.BookPublishers.AsEnumerable().Join(publishers, bs => bs.PublisherID, p => p.ID, (bs, p) => bs.Book).Distinct();
            return books != null ? books.Join(res, b => b.ID, i => i.ID, (b, i) => b) : res;
        }


        public IQueryable<Location> FindLocation(Location location) 
        {
            IQueryable<Location> results = null;

            if (location.Rack != null)
                results = from item in (results ?? context.Locations) where item.Rack.Contains(location.Rack) select item;
            if (location.Shelf != null)
                results = from item in (results ?? context.Locations) where item.Shelf == location.Shelf select item;

            return results ?? context.Locations;
        }




        public IQueryable<CoverType> FindCoverType(CoverType cover)
        {
            IQueryable<CoverType> results = null;
            if (cover.Name != null)
                results = from item in context.CoverTypes where item.Name.ToLower().Contains(cover.Name.ToLower()) select item;
            return results ?? context.CoverTypes;
        }

        public IQueryable<BindingType> FindBindingType(BindingType cover)
        {
            IQueryable<BindingType> results = null;
            if (cover.Name != null)
                results = from item in context.BindingTypes where item.Name.ToLower().Contains(cover.Name.ToLower()) select item;
            return results ?? context.BindingTypes;
        }

        public IEnumerable<Story> FindStory(FindStoryQuery query)
        {
            IEnumerable<Story> results = null;

            if (query.Story?.Title != null) results = FindStoryByInfo(query.Story);
            if (query.Authors != null) results = FindStoryByAuthor(query.Authors, results);

            return results ?? context.Stories;
        }

        IQueryable<Story> FindStoryByInfo(Story story) 
        {
            return from item in context.Stories where item.Title.ToLower().Contains(story.Title.ToLower()) select item;
        }

        IEnumerable<Story> FindStoryByAuthor(IEnumerable<Author> authors, IEnumerable<Story> stories = null)
        {
            var res = stories = context.StoryAuthors.AsEnumerable().Join(authors, s => s.AuthorID, a => a.ID, (s, a) => s.Story).Distinct();
            return stories != null ? stories.Join(res, b => b.ID, i => i.ID, (b, i) => b) : res;
        }

        public IQueryable<Genre> FindGenre(Genre genre)
        {
            IQueryable<Genre> results = null;
            if(genre.Name != null)
                results = from item in context.Genres where item.Name == genre.Name select item;

            return results ?? context.Genres;
        }
        public IQueryable<Author> FindAuthor(Author author)
        {
            IQueryable<Author> results = null;

            if (author.FirstName != null)
                results = from item in (results ?? context.Authors) where item.FirstName.ToLower().Contains(author.FirstName) select item;
           
            if (author.MiddleName != null)
                results = from item in (results ?? context.Authors) where item.MiddleName.ToLower().Contains(author.MiddleName) select item;
            
            if (author.LastName != null)
                results = from item in (results ?? context.Authors) where item.LastName.ToLower().Contains(author.LastName) select item;

            return results ?? context.Authors;
        }

        public IQueryable<Publisher> FindPublishers(Publisher publisher)
        {
            IQueryable<Publisher> results = null;
            if(publisher.Name != null)
                results = from item in context.Publishers where item.Name.Contains(publisher.Name) select item;

            return results ?? context.Publishers;
        }

        public void Dispose() 
        {
            context.Dispose();
        }
    }
}
