using System.Data.Entity;


namespace Library.Model.LibraryEntities
{

    public partial class LibraryEntities : DbContext
    {
        public LibraryEntities()
            : base("name=LibraryEntities")
        {
        }

        public LibraryEntities(string name)
            : base(name)
        {
            Configuration.AutoDetectChangesEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        static public void Init()
        {
            
            Database.SetInitializer<LibraryEntities>(null);
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<BindingType> BindingTypes { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookAuthor> BookAuthors { get; set; }
        public virtual DbSet<BookGenre> BookGenres { get; set; }
        public virtual DbSet<BookPublisher> BookPublishers { get; set; }
        public virtual DbSet<BookStory> BookStories { get; set; }
        public virtual DbSet<CoverType> CoverTypes { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<Story> Stories { get; set; }
        public virtual DbSet<StoryAuthor> StoryAuthors { get; set; }

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
                .HasMany(e => e.Books)
                .WithOptional(e => e.BindingType)
                .HasForeignKey(e => e.BindingTypeID);

            modelBuilder.Entity<Book>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.ISBN)
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
                .HasMany(e => e.BookPublisher)
                .WithRequired(e => e.Book)
                .HasForeignKey(e => e.BookID);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.BookStory)
                .WithRequired(e => e.Book)
                .HasForeignKey(e => e.BookID);

            modelBuilder.Entity<CoverType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<CoverType>()
                .HasMany(e => e.Books)
                .WithOptional(e => e.CoverType)
                .HasForeignKey(e => e.CoverTypeID);

            modelBuilder.Entity<Genre>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Genre>()
                .HasMany(e => e.BookGenre)
                .WithRequired(e => e.Genre)
                .HasForeignKey(e => e.GenreID);

            modelBuilder.Entity<Location>()
                .Property(e => e.Rack)
                .IsUnicode(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.Books)
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
