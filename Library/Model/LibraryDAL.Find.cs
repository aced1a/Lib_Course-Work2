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

        public LibraryEntities Context { get => context; }

        public IEnumerable<Author> Authors { get => context.Authors; }
        public IEnumerable<Genre> Genres { get => context.Genres; }
        public IEnumerable<Publisher> Publishers { get => context.Publishers; }
        public IEnumerable<Book> Books { get => context.Books; }


        public LibraryDAL()
        {
            context = new LibraryEntities();
        }

        public FindBookQuery CreateFindBookQuery()
        {
            var query = new FindBookQuery();
            query.ExecuteQuery += new QueryExecuted(FindBookWithQuery);
            return query;
        }

        private object FindBookWithQuery(IExecutableQuery query)
        {
            var r = query as FindBookQuery;
            if (r != null)
                return FindBook(r);
            return null;
        }

        public FindStoryQuery CreateFindStoryQuery()
        {
            var query = new FindStoryQuery();
            query.ExecuteQuery += new QueryExecuted(FindStoryWithQuery);
            return query;
        }

        private object FindStoryWithQuery(IExecutableQuery query)
        {
            var r = query as FindStoryQuery;
            if (r != null)
                return FindStory(r);
            return null;
        }


        public IQueryable<Book> FindBook(FindBookQuery query)
        {
            IQueryable<Book> results = null;

            if (query.Book != null) FindBookByInfo(query.Book, results);
            if (query.Location != null) FindBookByLocation(query.Location, results);
            if (query.Genres != null) FindBookByGenres(query.Genres, results);
            if (query.Stories != null) FindBookByStory(query.Stories, results);
            if (query.Authors != null) FindBookByAuthor(query.Authors, results);
            if (query.Publishers != null) FindBookByPublisher(query.Publishers, results);

            return results ?? context.Books;
        }

        public void FindBookByInfo(Book book, IQueryable<Book> books=null)
        {
            if (book.Title != null)
                books = from item in (books ?? context.Books) where item.Title.ToLower().Contains(book.Title.ToLower()) select item;
            if (book.Year != null)
                books = from item in (books ?? context.Books) where item.Year == book.Year select item;
            if (book.CoverTypeID != null)
                books = from item in (books ?? context.Books) where item.CoverTypeID == book.CoverTypeID select item;
            if (book.BindingTypeID != null)
                books = from item in (books ?? context.Books) where item.BindingTypeID == book.BindingTypeID select item;
            if (book.LocationID != null)
                books = from item in (books ?? context.Books) where item.LocationID == book.LocationID select item;    
        }

        public void FindBookByGenres(IEnumerable<Genre> genres, IQueryable<Book> books = null)
        {
            books = (books ?? context.Books).Join(
                context.BookGenres.Join(genres, bg => bg.GenreID, g => g.ID, (bg, g) => bg.Book).Distinct(),
                b => b.ID, i => i.ID, (b,i) => b
            );
        }

        public void FindBookByAuthor(IEnumerable<Author> authors, IQueryable<Book> books = null)
        {
            books = context.BookAuthors.Join(authors, ab => ab.AuthorID, a => a.ID, (ab, a) => ab.Book).Distinct();
            //books = context.BooksAuthors.Join(authors, ab => ab.AuthorID, a => a.ID, (ab, a) => ab.Book).GroupBy(a => a.ID).OrderBy(a => a.Count()).Select(a => a.First());
        }

        public void FindBookByStory(IEnumerable<Story> stories, IQueryable<Book> books = null)
        {
            books = context.BookStories.Join(stories, ab => ab.StoryID, s => s.ID, (ab, s) => ab.Book).Distinct();
        }

        public void FindBookByPublisher(IEnumerable<Publisher> publishers, IQueryable<Book> books = null) 
        {
            books = context.BookPublishers.Join(publishers, bs => bs.PublisherID, p => p.ID, (bs, p) => bs.Book).Distinct();
        }

        
        private void FindBookByLocation(Location location, IQueryable<Book> books)
        {
            var res = FindLocation(location);
            books = res.Join((books != null ? books : context.Books), l => l.ID, b => b.LocationID, (l, b) => b);
        }

        public IQueryable<Location> FindLocation(Location location) 
        {
            IQueryable<Location> results = null;

            if (location.Rack != null)
                results = from item in (results ?? context.Locations) where item.Rack.Contains(location.Rack) select item;
            if (location.Shelf != null)
                results = from item in (results ?? context.Locations) where item.Shelf == location.Shelf select item;

            return results;
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

        public IQueryable<Story> FindStory(FindStoryQuery query)
        {
            IQueryable<Story> results = null;

            if (query.Story != null) FindStoryByInfo(query.Story, results);
            if (query.Authors != null) FindStoryByAuthor(query.Authors, results);

            return results ?? context.Stories;
        }

        public void FindStoryByInfo(Story story, IQueryable<Story> stories) 
        {
            stories = from item in (stories ?? context.Stories) where item.Title.ToLower().Contains(story.Title.ToLower()) select item;
        }

        public void FindStoryByAuthor(IEnumerable<Author> authors, IQueryable<Story> stories = null)
        {
            stories = context.StoryAuthors.Join(authors, s => s.AuthorID, a => a.ID, (s, a) => s.Story);
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
