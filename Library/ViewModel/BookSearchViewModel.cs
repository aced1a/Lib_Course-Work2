using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Library.Model.LibraryEntities;
using Library.Query;
using Library.View;

namespace Library.ViewModel
{
    class BookSearchViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        IMainWindowCodeBehind _mainCodeBehind;
        private ObservableCollection<BookInfo> _books;
        private ObservableCollection<Genre> _selectedGenres;
        private ObservableCollection<Publisher> _selectedPublishers;
        private ObservableCollection<Author> _selectedAuthors;
        private ObservableCollection<StoryInfo> _selectedStories;
        private BookInfo _selectedBook;
        private CoverType _coverType;
        private BindingType _bindingType;
        private Location _location;
        string _bookName, _isbn;
        string _beginYear, _endYear;
        string _coverTypeName, _bindingTypeName;
        string _locationName;
       

        public BookSearchViewModel(IMainWindowCodeBehind codeBehind)
        {
            _mainCodeBehind = codeBehind;
        }


        public string BookName
        {
            get => _bookName;
            set
            {
                _bookName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(BookName)));
            }
        }

        public string ISBN
        {
            get => _isbn;
            set
            {
                _isbn = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ISBN)));
            }
        }

        public string BeginYear
        {
            get => _beginYear;
            set
            {
                _beginYear = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(BeginYear)));
            }
        }

        public string EndYear
        {
            get => _endYear;
            set
            {
                _endYear = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(EndYear)));
            }
        }

        public string LocationName
        {
            get => _locationName;
            set
            {
                _locationName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(LocationName)));
            }
        }

        public string CoverTypeName
        {
            get => _coverTypeName;
            set
            {
                _coverTypeName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(CoverTypeName)));
            }
        }

        public string BindingTypeName
        {
            get => _bindingTypeName;
            set
            {
                _bindingTypeName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(BindingTypeName)));
            }
        }

        public ObservableCollection<BookInfo> Books
        {
            get => _books = _books ?? new ObservableCollection<BookInfo>();
            set
            {
                _books = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Books)));
            }
        }

        public BookInfo SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedBook)));
            }
        }

        public CoverType CoverType
        {
            get => _coverType;
            set
            {
                _coverType = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(CoverType)));
                CoverTypeName = _coverType?.Name;
            }
        }

        public BindingType BindingType
        {
            get => _bindingType;
            set
            {
                _bindingType = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(BindingType)));
                BindingTypeName = _bindingType?.Name;
            }
        }

        public Location Location
        {
            get => _location;
            set
            {
                _location = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Location)));
                LocationName = _location?.Name;
            }
        }

        public ObservableCollection<Author> SelectedAuthors 
        {
            get => _selectedAuthors = _selectedAuthors ?? new ObservableCollection<Author>();
            set
            {
                _selectedAuthors = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedAuthors)));
            }
        }

        public ObservableCollection<Genre> SelectedGenres
        {
            get => _selectedGenres = _selectedGenres ?? new ObservableCollection<Genre>();
            set
            {
                _selectedGenres = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedGenres)));
            }
        }

        public ObservableCollection<Publisher> SelectedPublishers
        {
            get => _selectedPublishers = _selectedPublishers ?? new ObservableCollection<Publisher>();
            set
            {
                _selectedPublishers = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedPublishers)));
            }
        }

        public ObservableCollection<StoryInfo> SelectedStories
        {
            get => _selectedStories = _selectedStories ?? new ObservableCollection<StoryInfo>();
            set
            {
                _selectedStories = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedStories)));
            }
        }

        RelayCommand _addBookCommand;
        public RelayCommand AddBookCommand
        {
            get => _addBookCommand = _addBookCommand ?? new RelayCommand(AddBook);
        }

        RelayCommand _editBookCommand;
        public RelayCommand EditBookCommand
        {
            get => _editBookCommand = _editBookCommand ?? new RelayCommand(EditBook);
        }

        RelayCommand _deleteBookCommand;
        public RelayCommand DeleteBookCommand
        {
            get => _deleteBookCommand = _deleteBookCommand ?? new RelayCommand(DeleteBook);
        }

        private void AddBook()
        {
            OpenEditWindow(new BookInfo());
        }

        private void EditBook()
        {
            if(SelectedBook != null)
            {
                OpenEditWindow(SelectedBook);
            }
        }

        private void DeleteBook()
        {
            if(SelectedBook != null)
            {
                _mainCodeBehind?.Delete(SelectedBook.Book);
                Books.Remove(SelectedBook);
                SelectedBook = null;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Books)));
            }
        }

        private void OpenEditWindow(BookInfo book)
        {
            SubsidiarySearchWindow window = new SubsidiarySearchWindow() { Height=800, Width=800};
            EditBook view = new EditBook();
            EditBookViewModel vm = new EditBookViewModel(book, _mainCodeBehind, UpdateItems);
            view.DataContext = vm;
            window.OutputView.Content = view;

            window.ShowDialog();
        }

        private void UpdateItems(BookInfo book)
        {
            if (book != null)
            {
                if (Books.Contains(book) == false)
                {
                    Books.Add(book);
                }
                else {
                    Books.Remove(book);
                    Books.Add(book);
                }
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Books)));
            }
        }

        RelayCommand _openExtendedViewCommand;
        public RelayCommand OpenExtendedViewCommand
        {
            get => _openExtendedViewCommand = _openExtendedViewCommand ?? new RelayCommand(OpenExtendedView);
        }

        void OpenExtendedView()
        {
            if(SelectedBook != null)
            {
                BookExtended view = new BookExtended();
                ExtendedBookViewModel vm = new ExtendedBookViewModel(SelectedBook, this, ChangeView);
                view.DataContext = vm;
                _mainCodeBehind?.LoadThisView(view);
            }
        }

        void ChangeView(System.Windows.Controls.UserControl control)
        {
            _mainCodeBehind?.LoadThisView(control);
        }

        RelayCommand _exportToExcelCommand;
        public RelayCommand ExportToExcelCommand
        {
            get => _exportToExcelCommand = _exportToExcelCommand ?? new RelayCommand(ExportToExcel);
        }

        void ExportToExcel()
        {
            if (Books.Count > 0)
            {
                var export = new ExcelExporter();
                var items = from item in Books select item.Book;
                export.ExportToExcel(items);
            }
        }

        RelayCommand<string> _selectSortModeCommand;
        public RelayCommand<string> SelectSortModeCommand
        {
            get => _selectSortModeCommand = _selectSortModeCommand ?? new RelayCommand<string>(SelectSortMode);
        }

        void SelectSortMode(string mode)
        {
            System.Windows.MessageBox.Show(mode);
            sortMode = mode;
            Sort();
        }

        bool sortAscending = false;
        string sortMode = "Author";
        RelayCommand _sortCommand;
        public RelayCommand SortCommand
        {
            get => _sortCommand = _sortCommand ?? new RelayCommand(Sort);
        }
        void Sort()
        {
            sortAscending = !sortAscending;
            ListSortDirection direction = sortAscending ? ListSortDirection.Ascending : ListSortDirection.Descending;

            var books = System.Windows.Data.CollectionViewSource.GetDefaultView(Books);
            books.SortDescriptions.Clear();

            switch (sortMode)
            {
                case "Author":
                    books.SortDescriptions.Add(new SortDescription("AuthorsText", direction));
                    break;
                case "Genre":
                    books.SortDescriptions.Add(new SortDescription("GenresText", direction));
                    break;
                case "Publisher":
                    books.SortDescriptions.Add(new SortDescription("PublishersText", direction));
                    break;
                case "Title":
                    books.SortDescriptions.Add(new SortDescription("Book.Title", direction));
                    break;

                default:
                    return;
            }
            books.Refresh();
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Books)));
        }

            #region copypaste

            RelayCommand _findBooksCommand;
        public RelayCommand FindBooksCommand
        {
            get => _findBooksCommand = _findBooksCommand ?? new RelayCommand(FindBooks);
        }

        private void FindBooks()
        {
            int year;
            Books = _mainCodeBehind?.FindBooks(
                new FindBookQuery()
                {
                    Book = GetBook(),
                    Authors = SelectedAuthors?.Count == 0 ? null : SelectedAuthors,
                    Genres = SelectedGenres?.Count == 0 ? null : SelectedGenres,
                    Stories = SelectedStories?.Count == 0 ? null : (from item in SelectedStories select item.Story),
                    Publishers = SelectedPublishers?.Count == 0 ? null : SelectedPublishers,
                    BeginYear = int.TryParse(BeginYear, out year) ? (int?)year : null,
                    EndYear = int.TryParse(EndYear, out year) ? (int?)year : null
                }
            );
        }

        private Book GetBook()
        {
            return new Book()
            {
                Title = BookName,
                LocationID = Location?.ID,
                BindingTypeID = BindingType?.ID,
                CoverTypeID = CoverType?.ID,
                ISBN = this.ISBN
            };
        }


        RelayCommand _openAuthorsSearchCommand;
        public RelayCommand OpenAuthorsSearchCommand
        {
            get => _openAuthorsSearchCommand = _openAuthorsSearchCommand ?? new RelayCommand(OpenAuthorsSearch);
        }

        RelayCommand<object> _deleteAuthorCommand;
        public RelayCommand<object> DeleteAuthorCommand
        {
            get => _deleteAuthorCommand = _deleteAuthorCommand ?? new RelayCommand<object>(DeleteAuthor);
        }


        private void OpenAuthorsSearch()
        {
            SubsidiarySearchWindow window = new SubsidiarySearchWindow();
            AuthorSearch view = new AuthorSearch();
            AuthorSearchViewModel vm = new AuthorSearchViewModel(_mainCodeBehind, AddAuthor);
            view.DataContext = vm;
            window.OutputView.Content = view;

            window.Show();
        }

        private void AddAuthor(Author author)
        {
            if (SelectedAuthors.Contains(author) == false)
            {
                SelectedAuthors.Add(author);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedAuthors)));
            }
        }

        private void DeleteAuthor(object obj)
        {
            Author author = obj as Author;
            if (author != null && SelectedAuthors.Contains(author))
            {
                SelectedAuthors.Remove(author);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedAuthors)));
            }
        }

        RelayCommand<object> _deleteGenreCommand;
        public RelayCommand<object> DeleteGenreCommand
        {
            get => _deleteGenreCommand = _deleteGenreCommand ?? new RelayCommand<object>(DeleteGenre);
        }

        RelayCommand _openGenresSearchCommand;
        public RelayCommand OpenGenresSearchCommand
        {
            get => _openGenresSearchCommand = _openGenresSearchCommand ?? new RelayCommand(OpenGenresSearch);
        }

        private void OpenGenresSearch()
        {
            SubsidiarySearchWindow window = new SubsidiarySearchWindow();
            GenreSearch view = new GenreSearch();
            GenreSearchViewModel vm = new GenreSearchViewModel(_mainCodeBehind, AddGenre);
            view.DataContext = vm;
            window.OutputView.Content = view;

            window.ShowDialog();
        }

        private void AddGenre(Genre genre) 
        { 
            if(SelectedGenres.Contains(genre) == false)
            {
                SelectedGenres.Add(genre);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedGenres)));
            }
        }

        private void DeleteGenre(object obj)
        {
            Genre genre = obj as Genre;
            if (genre != null && SelectedGenres.Contains(genre))
            {
                SelectedGenres.Remove(genre);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedGenres)));
            }
        }

        RelayCommand<object> _deletePublisherCommand;
        public RelayCommand<object> DeletePublisherCommand
        {
            get => _deletePublisherCommand = _deletePublisherCommand ?? new RelayCommand<object>(DeletePublisher);
        }

        RelayCommand _openPublishersSearchCommand;
        public RelayCommand OpenPublishersSearchCommand
        {
            get => _openPublishersSearchCommand = _openPublishersSearchCommand ?? new RelayCommand(OpenPublishersSearch);
        }

        private void OpenPublishersSearch()
        {
            SubsidiarySearchWindow window = new SubsidiarySearchWindow();
            PublisherSearch view = new PublisherSearch();
            PublisherSearchViewModel vm = new PublisherSearchViewModel(_mainCodeBehind, AddPublisher);
            view.DataContext = vm;
            window.OutputView.Content = view;

            window.ShowDialog();
        }

        private void AddPublisher(Publisher publisher) 
        {
            if (SelectedPublishers.Contains(publisher) == false)
            {
                SelectedPublishers.Add(publisher);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedPublishers)));
            }
        }
        private void DeletePublisher(object obj)
        {
            Publisher publisher = obj as Publisher;
            if(publisher != null && SelectedPublishers.Contains(publisher))
            {
                SelectedPublishers.Remove(publisher);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedPublishers)));
            }
        }


        RelayCommand _openStoriesSearchCommand;
        public RelayCommand OpenStoriesSearchCommand
        {
            get => _openStoriesSearchCommand = _openStoriesSearchCommand ?? new RelayCommand(OpenStoriesSearch);
        }

        RelayCommand<object> _deleteStoryCommand;
        public RelayCommand<object> DeleteStoryCommand
        {
            get => _deleteStoryCommand = _deleteStoryCommand ?? new RelayCommand<object>(DeleteStory);
        }


        private void OpenStoriesSearch()
        {
            SubsidiarySearchWindow window = new SubsidiarySearchWindow();
            StorySearch view = new StorySearch();
            StorySearchViewModel vm = new StorySearchViewModel(_mainCodeBehind, AddStory);
            view.DataContext = vm;
            window.OutputView.Content = view;

            window.ShowDialog();
        }

        private void AddStory(StoryInfo story)
        {
            if (SelectedStories.Contains(story) == false)
            {
                SelectedStories.Add(story);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedStories)));
            }
        }

        private void DeleteStory(object obj)
        {
            StoryInfo story = obj as StoryInfo;
            if (story != null && SelectedStories.Contains(story))
            {
                SelectedStories.Remove(story);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedStories)));
            }
        }

        RelayCommand _openCoverTypesSearchCommand;
        public RelayCommand OpenCoverTypesSearchCommand
        {
            get => _openCoverTypesSearchCommand = _openCoverTypesSearchCommand ?? new RelayCommand(OpenCoverTypesSearch);
        }

        RelayCommand _deleteCoverTypeCommand;
        public RelayCommand DeleteCoverTypeCommand
        {
            get => _deleteCoverTypeCommand = _deleteCoverTypeCommand ?? new RelayCommand(DeleteCoverType);
        }

        private void OpenCoverTypesSearch()
        {
            SubsidiarySearchWindow window = new SubsidiarySearchWindow();
            CoverTypeSearch view = new CoverTypeSearch();
            CoverTypeSearchViewModel vm = new CoverTypeSearchViewModel(_mainCodeBehind,SelectCoverType);
            view.DataContext = vm;
            window.OutputView.Content = view;

            window.ShowDialog();
        }

        private void SelectCoverType(CoverType cover) 
        {
            if(cover != null)
                CoverType = cover;
        }
   
        private void DeleteCoverType()
        {
            CoverType = null;
        }

        RelayCommand _openBindingTypesSearchCommand;
        public RelayCommand OpenBindingTypesSearchCommand
        {
            get => _openBindingTypesSearchCommand = _openBindingTypesSearchCommand ?? new RelayCommand(OpenBindingTypesSearch);
        }

        RelayCommand _deleteBindingTypeCommand;
        public RelayCommand DeleteBindingTypeCommand
        {
            get => _deleteBindingTypeCommand = _deleteBindingTypeCommand ?? new RelayCommand(DeleteBindingType);
        }

        private void OpenBindingTypesSearch()
        {
            SubsidiarySearchWindow window = new SubsidiarySearchWindow();
            BindingTypeSearch view = new BindingTypeSearch();
            BindingTypeSearchViewModel vm = new BindingTypeSearchViewModel(_mainCodeBehind, SelectBindingType);
            view.DataContext = vm;
            window.OutputView.Content = view;

            window.ShowDialog();
        }

        private void SelectBindingType(BindingType type)
        {
            BindingType = type;
        }
    
        private void DeleteBindingType()
        {
            BindingType = null;
        }

        RelayCommand _openLocationsSearchCommand;
        public RelayCommand OpenLocationsSearchCommand
        {
            get => _openLocationsSearchCommand = _openLocationsSearchCommand ?? new RelayCommand(OpenLocationsSearch);
        }

        RelayCommand _deleteLocationCommand;
        public RelayCommand DeleteLocationCommand
        {
            get => _deleteLocationCommand = _deleteLocationCommand ?? new RelayCommand(DeleteLocation);
        }

        private void OpenLocationsSearch()
        {
            SubsidiarySearchWindow window = new SubsidiarySearchWindow();
            LocationSearch view = new LocationSearch();
            LocationSearchViewModel vm = new LocationSearchViewModel(_mainCodeBehind, SelectLocation);
            view.DataContext = vm;
            window.OutputView.Content = view;

            window.ShowDialog();
        }

        private void SelectLocation(Location location) 
        {
            Location = location;
        }
        private void DeleteLocation() { Location = null; }
    }
    #endregion copypaste
}
