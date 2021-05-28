using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Library;


namespace Library.Model.LibraryEntities
{
    class BookInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public Book Book { get; set; }

        private ObservableCollection<Author> _authors;
        public ObservableCollection<Author>  Authors {
            get => _authors = _authors ?? new ObservableCollection<Author>(GetAuthors());
            set
            {
                _authors = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Authors)));
            }
        }

        private ObservableCollection<Genre> _genres;
        public ObservableCollection<Genre> Genres
        {
            get => _genres = _genres ?? new ObservableCollection<Genre>(GetGenres());
            set
            {
                _genres = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Genres)));
            }
        }
        ObservableCollection<Publisher> _publishers;
        public ObservableCollection<Publisher> Publishers
        {
            get => _publishers = _publishers ?? new ObservableCollection<Publisher>(GetPublishers());
            set
            {
                _publishers = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Publishers)));
            }
        }

        ObservableCollection<Story> _stories;
        public ObservableCollection<Story> Stories
        {
            get => _stories = _stories ?? new ObservableCollection<Story>(GetStories());
            set
            {
                _stories = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Stories)));
            }
        }

        public object Image
        {
            get => (object) Book.Image ?? (object) App.GetResourceStream(new Uri("ImageNotFound"));
        }

        public BookInfo(Book book)
        {
            Book = book ?? new Book() { ID = -1 };     
        }


        private IEnumerable<Author> GetAuthors() 
        {
            return from item in Book.BookAuthor select item.Author;
        }

        private IEnumerable<Genre> GetGenres()
        {
            return from item in Book.BookGenre select item.Genre;
        }

        private IEnumerable<Publisher> GetPublishers()
        {
            return from item in Book.BookPublisher select item.Publisher;
        }

        private IEnumerable<Story> GetStories()
        {
            return from item in Book.BookStory select item.Story;
        }


        private void DrawImage()
        {
            
        }

        public string AuthorsText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Авторы: ");
                int i = 0, min = Math.Min(3, Authors.Count());
                foreach (var author in Authors)
                {
                    sb.Append(author.FullName); i++;
                    if (i >= min) break;
                    sb.Append(", ");
                }
                if (min > 3) sb.Append(" ...");
                return sb.ToString();
            }
        }

        public string GenresText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Жанры: ");
                int i = 0, min = Math.Min(3, Genres.Count);
                foreach (var genre in Genres)
                {
                    sb.Append(genre.Name); i++;
                    if (i >= min) break;
                    sb.Append(", ");
                }
                if (min > 3) sb.Append(" ...");
                return sb.ToString();
            }
        }

        public string PublishersText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Жанры: ");
                int i = 0, min = Math.Min(3, Publishers.Count);
                foreach (var publisher in Publishers) { 
                
                    sb.Append(publisher.Name); i++;
                    if (i >= min) break;
                    sb.Append(", ");
                }
                if (min > 3) sb.Append(" ...");
                return sb.ToString();
            }
        }
    }
}
