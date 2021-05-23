﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models;
using Library.Query;

namespace Library
{
    partial class LibraryDAL : IDisposable
    {
        LibraryEntities context;

        public LibraryEntities Context { get => context; }

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

            if (query.ISBN != null) FindBookByISBN(query.ISBN, results);
            if (query.Book != null) FindBookByInfo(query.Book, results);
            if (query.Cover != null) FindBookByCover(query.Cover, results);
            if (query.Location != null) FindBookByLocation(query.Location, results);
            if (query.Genres != null) FindBookByGenres(query.Genres, results);
            if (query.Stories != null) FindBookByStory(query.Stories, results);
            //if (query.Stories != null) {
            //    foreach (var story in query.Stories)
            //    {
            //        foreach(var author in story.StoryAuthor.Select(sa => sa.Author).Distinct())
            //        {
            //            query.Authors.Append(author);
            //        }
            //    } 
            //}
            if (query.Authors != null) FindBookByAuthor(query.Authors, results);
            if (query.Publishers != null) FindBookByPublisher(query.Publishers, results);

            return results;
        }

        public void FindBookByInfo(Book book, IQueryable<Book> books=null)
        {
            if (book.Title != null)
                books = from item in (books != null ? books : context.Books) where item.Title.ToLower().Contains(book.Title) select item;
            if (book.Year != null)
                books = from item in (books != null ? books : context.Books) where item.Year == book.Year select item;
            if (book.CoverID != null)
                books = from item in (books != null ? books : context.Books) where item.CoverID == book.CoverID select item;
            if (book.LocationID != null)
                books = from item in (books != null ? books : context.Books) where item.LocationID == book.LocationID select item;    
        }

        public void FindBookByGenres(IEnumerable<Genre> genres, IQueryable<Book> books = null)
        {
            //foreach (var genre in genres)
            //    result = from item in context.BooksGenres where item.GenreID == genre select item.BookID;
            books = context.BooksGenres.Join(genres, bg => bg.GenreID, g => g.ID, (bg, g) => bg.Book).Distinct();
        }

        public void FindBookByAuthor(IEnumerable<Author> authors, IQueryable<Book> books = null)
        {
            books = context.BooksAuthors.Join(authors, ab => ab.AuthorID, a => a.ID, (ab, a) => ab.Book).Distinct();
            //books = context.BooksAuthors.Join(authors, ab => ab.AuthorID, a => a.ID, (ab, a) => ab.Book).GroupBy(a => a.ID).OrderBy(a => a.Count()).Select(a => a.First());
        }

        public void FindBookByStory(IEnumerable<Story> stories, IQueryable<Book> books = null)
        {
            //IQueryable<int> result = null;

            books = context.BooksStories.Join(stories, ab => ab.StoryID, s => s.ID, (ab, s) => ab.Book).Distinct();

            //books = result.Join((books != null ? books : context.Books), r => r, b => b.ID, (r, b) => b);
        }

        public void FindBookByPublisher(IEnumerable<Publisher> publishers, IQueryable<Book> books = null) 
        {
            books = context.BooksPublishers.Join(publishers, bs => bs.PublisherID, p => p.ID, (bs, p) => bs.Book).Distinct();
        }

        public void FindBookByISBN(ISBN isbn, IQueryable<Book> books=null)
        { 
            books = from item in context.ISBNs where item.isbn == isbn.isbn select item.Book;  
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
                results = from item in (results != null ? results : context.Locations) where item.Rack.Contains(location.Rack) select item;
            if (location.Shelf != null)
                results = from item in (results != null ? results : context.Locations) where item.Shelf == location.Shelf select item;

            return results;
        }


        private void FindBookByCover(Cover cover, IQueryable<Book> books)
        {
            var res = FindCover(cover);
            books = res.Join((books != null ? books : context.Books), c => c.ID, b => b.CoverID, (c, b) => b);
        }

        public IQueryable<Cover> FindCover(Cover cover)
        {
            IQueryable<Cover> results = null;

            if (cover.CoverID != null)
                results = from item in (results != null ? results : context.Covers) where item.CoverID == cover.CoverID select item;
            if (cover.BindingID != null)
                results = from item in (results != null ? results : context.Covers) where item.BindingID == cover.BindingID select item;
            if (cover.ImageID != null)
                results = from item in (results != null ? results : context.Covers) where item.ImageID == cover.ImageID select item;

            return results;
        }


        public IQueryable<Story> FindStory(FindStoryQuery query)
        {
            IQueryable<Story> results = null;

            if (query.Story != null) FindStoryByInfo(query.Story, results);
            if (query.Authors != null) FindStoryByAuthor(query.Authors, results);

            return results;
        }

        public void FindStoryByInfo(Story story, IQueryable<Story> stories) 
        {
            stories = from item in (stories != null ? stories : context.Stories) where item.Title.ToLower().Contains(story.Title) select item;
        }


        public void FindStoryByAuthor(IEnumerable<Author> authors, IQueryable<Story> stories = null)
        {
            stories = context.StoriesAuthors.Join(authors, s => s.AuthorID, a => a.ID, (s, a) => s.Story);
        }


        public IQueryable<Genre> FindGenre(Genre genre)
        {
            var results = from item in context.Genres where item.Name == genre.Name select item;
            return results;
        }

        public IQueryable<Author> FindAuthor(Author author)
        {
            IQueryable<Author> results = null;

            if (author.FirstName != null)
                results = from item in (results != null ? results : context.Authors) where item.FirstName.ToLower().Contains(author.FirstName) select item;
            if (author.MiddleName != null)
                results = from item in (results != null ? results : context.Authors) where item.MiddleName.ToLower().Contains(author.MiddleName) select item;
            if (author.LastName != null)
                results = from item in (results != null ? results : context.Authors) where item.LastName.ToLower().Contains(author.LastName) select item;

            return results;
        }

        public IQueryable<Publisher> FindPublisher(string name)
        {
            IQueryable<Publisher> results = null;
            results = from item in context.Publishers where item.Name.Contains(name) select item;

            return results;
        }

        public void Dispose() 
        {
            context.Dispose();
        }
    }
}
