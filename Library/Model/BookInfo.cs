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
    public class BookInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private Book _book;
        public Book Book
        {
            get => _book = _book ?? new Book() { ID = -1 };
            set
            {
                _book = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Book)));
            }
        }

        private ObservableCollection<Author> _authors;
        public ObservableCollection<Author> Authors {
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

        ObservableCollection<StoryInfo> _stories;
        public ObservableCollection<StoryInfo> Stories
        {
            get => _stories = _stories ?? new ObservableCollection<StoryInfo>(GetStories());
            set
            {
                _stories = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Stories)));
            }
        }

        public object Image
        {
            get => (object)Book.Image ?? App.Current.FindResource("ImageNotFound");
        }

        public BookInfo(Book book)
        {
            Book = book;
            
        }

        public BookInfo() { }


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

        private IEnumerable<StoryInfo> GetStories()
        {
            return from item in Book.BookStory select new StoryInfo { Story = item.Story };
        }

        public string AuthorsText
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Авторы: ");
                int i = 0, min = Math.Min(5, Authors.Count());
                foreach (var author in Authors)
                {
                    sb.Append(author.FullName); i++;
                    if (i >= min) break;
                    sb.Append(", ");
                }
                if (min > 5) sb.Append(" ...");
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
                sb.Append("Издатели: ");
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


    public class StoryInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        Story _story;

        public Story Story
        {
            get => _story = _story ?? new Story() { ID = -1 };
            set
            {
                _story = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Story)));
            }
        }
        private ObservableCollection<Author> _authors;

        public ObservableCollection<Author> Authors
        {
            get => _authors = _authors ?? new ObservableCollection<Author>(GetAuthors());
            set
            {
                _authors = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Authors)));
            }
        }

        public StoryInfo()
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

        private IEnumerable<Author> GetAuthors()
        {
            return from item in Story.StoryAuthor select item.Author;
        }
    }

}
