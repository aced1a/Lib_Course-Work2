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
    
        public bool EditBook(EditBookQuery query)
        {
            if(query.Book != null)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        EditBookAuthorsRelations(query);
                        EditBookGenresRelations(query);
                        EditBookPublishersRelations(query);
                        EditBookStoriesRelations(query);

                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private void EditBookAuthorsRelations(EditBookQuery query) 
        { 
            if(query.DeletedAuthors != null && query.DeletedAuthors.Count() > 0)
                    context.BookAuthors.RemoveRange(
                        from item in query.Book.BookAuthor where query.DeletedAuthors.Contains(item.ID) select item
                );
            if(query.AddedAuthors != null && query.AddedAuthors.Count()>0)
                    context.BookAuthors.AddRange(
                    from item in query.AddedAuthors
                    select new BookAuthor
                    {
                        BookID = query.Book.ID,
                        AuthorID = item
                    }
                );
        }

        private void EditBookGenresRelations(EditBookQuery query)
        {
            if (query.DeletedGenres != null && query.DeletedGenres.Count() > 0)
                context.BookGenres.RemoveRange(
                    from item in query.Book.BookGenre where query.DeletedGenres.Contains(item.ID) select item
            );
            if (query.AddedGenres != null && query.AddedGenres.Count() > 0)
                context.BookGenres.AddRange(
                from item in query.AddedGenres
                select new BookGenre
                {
                    BookID = query.Book.ID,
                    GenreID = item
                }
            );
        }

        private void EditBookPublishersRelations(EditBookQuery query)
        {
            if (query.DeletedPublishers != null && query.DeletedPublishers.Count() > 0)
            {
                context.BookPublishers.RemoveRange(
                    from item in query.Book.BookPublisher where query.DeletedPublishers.Contains(item.ID) select item
                );

            }
            if (query.AddedPublishers != null && query.AddedPublishers.Count() > 0)
            {
                context.BookPublishers.AddRange(
                    from item in query.AddedPublishers
                    select new BookPublisher
                    {
                        BookID = query.Book.ID,
                        PublisherID = item
                    }
                );
             
            }
        }

        private void EditBookStoriesRelations(EditBookQuery query)
        {
            if (query.DeletedStories != null && query.DeletedStories.Count() > 0)
                context.BookStories.RemoveRange(
                    from item in query.Book.BookStory where query.DeletedStories.Contains(item.ID) select item
            );
            if (query.AddedStories != null && query.AddedStories.Count() > 0)
                context.BookStories.AddRange(
                from item in query.AddedStories
                select new BookStory
                {
                    BookID = query.Book.ID,
                    StoryID = item
                }
            );
        }
    
        public bool EditStory(EditStoryQuery query)
        {
            if (query.Story != null)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try 
                    { 
                    
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
   
        private void EditStoryAuthorsRelations(EditStoryQuery query)
        {
            if (query.DeletedAuthors != null && query.DeletedAuthors.Count() > 0)
                context.StoryAuthors.RemoveRange(
                    from item in query.Story.StoryAuthor where query.DeletedAuthors.Contains(item.ID) select item
            );
            if (query.AddedAuthors != null && query.AddedAuthors.Count() > 0)
                context.StoryAuthors.AddRange(
                from item in query.AddedAuthors
                select new StoryAuthor
                {
                    StoryID = query.Story.ID,
                    AuthorID = item
                }
            );
        }
    }
}
