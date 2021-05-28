using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Model.LibraryEntities;
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

        public bool SaveChanges()
        {
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
            context.BookAuthors.AddRange(from item in authors select new BookAuthor { AuthorID = item.ID, BookID = book.ID });
        }

        private void AddBookGenresRelations(Book book, IEnumerable<Genre> genres)
        {
            context.BookGenres.AddRange(from item in genres select new BookGenre { GenreID = item.ID, BookID = book.ID });
        }

        private void AddBookStoriesRelations(Book book, IEnumerable<Story> stories)
        {
            context.BookStories.AddRange(from item in stories select new BookStory { StoryID = item.ID, BookID = book.ID });
        }

        private void AddBookPublishersRelations(Book book, IEnumerable<Publisher> publishers)
        {
            context.BookPublishers.AddRange(from item in publishers select new BookPublisher { PublisherID = item.ID, BookID = book.ID });
        }
    
    
        private int AddLocation(Location location)
        {
            context.Locations.Add(location);
            context.SaveChanges();
            return location.ID;
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


        private void AddStoryAuthorRelations(Story story, IEnumerable<Author> authors)
        {
            context.StoryAuthors.AddRange(from item in authors select new StoryAuthor { StoryID = story.ID, AuthorID = item.ID });
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
