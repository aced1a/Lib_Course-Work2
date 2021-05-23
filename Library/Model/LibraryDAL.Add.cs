using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models;
using Library.Query;

namespace Library
{
    partial class LibraryDAL
    {
        public AddBookQuery CreateAddBookQuery()
        {
            var query = new AddBookQuery();
            query.ExecuteQuery += new QueryExecuted(AddBookWithQuery);
            return query;
        }

        public AddStoryQuery CreateAddStoryQuery()
        {
            var query = new AddStoryQuery();
            query.ExecuteQuery += new QueryExecuted(AddStoryWithQuery);
            return query;
        }

        public bool AddAuthor(Author author) 
        {
            context.Authors.Add(author);
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

        public bool AddGenre(Genre genre)
        {
            context.Genres.Add(genre);
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

        public bool AddPublisher(Publisher publisher)
        {
            context.Publishers.Add(publisher);
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
    
        public bool AddBindingType(BindingType binding) 
        {
            context.BindingTypes.Add(binding);
            try {
                context.SaveChanges();
                return true;
            } catch(Exception) {
                return false;
            }
        }

        public bool AddCoverType(CoverType cover)
        {
            context.CoverTypes.Add(cover);
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


        private void AddBookAuthorsRelations(Book book, IEnumerable<Author> authors) 
        {
            context.BooksAuthors.AddRange(from item in authors select new BookAuthor { AuthorID = item.ID, BookID = book.ID });
        }

        private void AddBookGenresRelations(Book book, IEnumerable<Genre> genres)
        {
            context.BooksGenres.AddRange(from item in genres select new BookGenre { GenreID = item.ID, BookID = book.ID });
        }

        private void AddBookStoriesRelations(Book book, IEnumerable<Story> stories)
        {
            context.BooksStories.AddRange(from item in stories select new BookStory { StoryID = item.ID, BookID = book.ID });
        }

        private void AddBookPublishersRelations(Book book, IEnumerable<Publisher> publishers)
        {
            context.BooksPublishers.AddRange(from item in publishers select new BookPublisher { PublisherID = item.ID, BookID = book.ID });
        }
    
        private void AddISBNsForBook(Book book, IEnumerable<ISBN> ISBNs)
        {
            context.ISBNs.AddRange(from item in ISBNs select new ISBN { BookID = book.ID, isbn = item.isbn });
        }
    
        private int AddLocation(Location location)
        {
            context.Locations.Add(location);
            context.SaveChanges();
            return location.ID;
        }

        private int AddCoverInBook(AddCoverQuery cover)
        {
            if (cover.Image != null) cover.Cover.ImageID = AddImage(cover.Image);

            context.Covers.Add(cover.Cover);
            context.SaveChanges();
            return cover.Cover.ID;
        }

        private int AddImage(Image image)
        {
            context.Images.Add(image);
            context.SaveChanges();
            return image.ID;
        }
    
        private object AddBookWithQuery(IExecutableQuery query) 
        {
            var r = query as AddBookQuery;
            if(r != null)
                return AddBook(r);
            return null;
        }

        public bool AddBook(AddBookQuery query)
        {
            bool sucefull = true;
            using(var transaction = context.Database.BeginTransaction())
            {
                try 
                {
                    //ID always = 0
                    if (query.AddingLocation != null) { query.Book.LocationID = AddLocation(query.AddingLocation); }
                    if (query.AddingCover    != null) { query.Book.CoverID = AddCoverInBook(query.AddingCover); }
                    if (query.AddingStories  != null) {
                        foreach (var addingStory in query.AddingStories)
                        {
                            Story story = AddStoryInBook(addingStory);
                            if(story != null)
                                query.Stories.Append(story);
                        }
                    }

                    AddBook(query.Book);
                    AddRelationsForBook(query);

                    context.SaveChanges();
                    transaction.Commit();
                } 
                catch (Exception) 
                {
                    transaction.Rollback();
                    sucefull = false;
                }
            }
            return sucefull;
        }

        private Story AddStoryInBook(AddStoryQuery story)
        {
            context.Stories.Add(story.Story);
            context.SaveChanges();
            AddStoryAuthorRelations(story.Story, story.Authors);
            return story.Story;
        }

        private void AddStoryAuthorRelations(Story story, IEnumerable<Author> authors)
        {
            context.StoriesAuthors.AddRange(from item in authors select new StoryAuthor { StoryID = story.ID, AuthorID = item.ID });
        }

        private void AddBook(Book book)
        {
            context.Books.Add(book);
            context.SaveChanges();
        }

        private void AddRelationsForBook(AddBookQuery book) 
        {
            if (book.Authors != null) AddBookAuthorsRelations(book.Book, book.Authors);
            if (book.Genres != null) AddBookGenresRelations(book.Book, book.Genres);
            if (book.Stories != null) AddBookStoriesRelations(book.Book, book.Stories);
            if (book.Publishers != null) AddBookPublishersRelations(book.Book, book.Publishers);
            if (book.ISBNs != null) AddISBNsForBook(book.Book, book.ISBNs);
        }

        private object AddStoryWithQuery(IExecutableQuery query)
        {
            var r = query as AddStoryQuery;
            if (r != null)
                return AddStory(r);
            return null;
        }

        public bool AddStory(AddStoryQuery query)
        {
            bool sucefull = true;
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    AddStory(query.Story);
                    if(query.Authors != null) AddStoryAuthorRelations(query.Story, query.Authors);

                    context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    sucefull = false;
                }
            }
            return sucefull;
        }

        private void AddStory(Story story)
        {
            context.Stories.Add(story);
            context.SaveChanges();
        }
    
        //public bool AddCover(AddCoverQuery query) 
        //{

        //}
    }
}
