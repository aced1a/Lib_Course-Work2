using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Library.Models
{
    public partial class LibraryEntities : DbContext
    {
        public LibraryEntities()
            : base("name=LibraryEntities")
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<BindingType> BindingTypes { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookAuthor> BooksAuthors { get; set; }
        public virtual DbSet<BookGenre> BooksGenres { get; set; }
        public virtual DbSet<BookPublisher> BooksPublishers { get; set; }
        public virtual DbSet<BookStory> BooksStories { get; set; }
        public virtual DbSet<Cover> Covers { get; set; }
        public virtual DbSet<CoverType> CoverTypes { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<ISBN> ISBNs { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<Story> Stories { get; set; }
        public virtual DbSet<StoryAuthor> StoriesAuthors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Author>()
                .Property(e => e.MiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<Author>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Author>()
                .HasMany(e => e.AuthorBook)
                .WithRequired(e => e.Author)
                .HasForeignKey(e => e.AuthorID);

            modelBuilder.Entity<Author>()
                .HasMany(e => e.AuthorStory)
                .WithRequired(e => e.Author)
                .HasForeignKey(e => e.AuthorID);

            modelBuilder.Entity<BindingType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<BindingType>()
                .HasMany(e => e.Cover)
                .WithOptional(e => e.BindingType)
                .HasForeignKey(e => e.BindingID);

            modelBuilder.Entity<Book>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.Note)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.BookAuthor)
                .WithRequired(e => e.Book)
                .HasForeignKey(e => e.BookID);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.BookGenre)
                .WithRequired(e => e.Book)
                .HasForeignKey(e => e.BookID);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.ISBN)
                .WithRequired(e => e.Book)
                .HasForeignKey(e => e.BookID);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.BookPublisher)
                .WithRequired(e => e.Book)
                .HasForeignKey(e => e.BookID);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.BookStory)
                .WithRequired(e => e.Book)
                .HasForeignKey(e => e.BookID);

            modelBuilder.Entity<Cover>()
                .HasMany(e => e.Book)
                .WithOptional(e => e.Cover)
                .HasForeignKey(e => e.CoverID);

            modelBuilder.Entity<CoverType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<CoverType>()
                .HasMany(e => e.Cover)
                .WithOptional(e => e.CoverType)
                .HasForeignKey(e => e.CoverID);

            modelBuilder.Entity<Genre>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Genre>()
                .HasMany(e => e.BookGenre)
                .WithRequired(e => e.Genre)
                .HasForeignKey(e => e.GenreID);

            modelBuilder.Entity<Image>()
                .HasMany(e => e.Cover)
                .WithOptional(e => e.Image)
                .HasForeignKey(e => e.ImageID);

            modelBuilder.Entity<ISBN>()
                .Property(e => e.isbn)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .Property(e => e.Rack)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.Book)
                .WithOptional(e => e.Location)
                .HasForeignKey(e => e.LocationID);

            modelBuilder.Entity<Publisher>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Publisher>()
                .HasMany(e => e.BookPublisher)
                .WithRequired(e => e.Publisher)
                .HasForeignKey(e => e.PublisherID);

            modelBuilder.Entity<Story>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Story>()
                .HasMany(e => e.BookStory)
                .WithRequired(e => e.Story)
                .HasForeignKey(e => e.StoryID);

            modelBuilder.Entity<Story>()
                .HasMany(e => e.StoryAuthor)
                .WithRequired(e => e.Story)
                .HasForeignKey(e => e.StoryID);
        }
    }
}
