using Library.Model.LibraryEntities;
using Library.Query;
using Library.View;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Linq;


namespace Library.ViewModel
{
    class EditBookViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        IMainWindowCodeBehind _mainCodeBehind;
        Action<BookInfo> update;
        BookInfo _book;
        CoverType _coverType;
        BindingType _bindingType;
        Location _location;
        string _year;
        string _coverTypeName, _bindingTypeName;
        string _locationName;
        byte[] _image;
        EditBookQuery query;

        public object Image
        {
            get => _image ?? Book.Image;
            set
            {
                _image = (byte[]) value;
                if (_image?.Length > 0) Book.Book.Image = _image;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Image)));
            }
        } 

        public BookInfo Book
        {
            get => _book = _book ?? new BookInfo();
            set
            {
                _book = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Book)));
                CoverType = _book.Book.CoverType;
                BindingType = _book.Book.BindingType;
                Location = _book.Book.Location;
                Image = _book.Book.Image;
                Year = _book.Book.Year?.ToString() ?? "";
            }
        }

        public CoverType CoverType
        {
            get => _coverType ?? new CoverType();
            set
            {
                _coverType = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(CoverType)));
                Book.Book.CoverTypeID = _coverType?.ID;
                CoverTypeName = _coverType?.Name;
            }
        }

        public BindingType BindingType
        {
            get => _bindingType ?? new BindingType();
            set
            {
                _bindingType = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(BindingType)));
                Book.Book.BindingTypeID = _bindingType?.ID;
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
                Book.Book.LocationID = _location?.ID;
                LocationName = _location?.Name;
            }
        }

        public string Year
        {
            get => _year;
            set
            {
                _year = value;
                Book.Book.Year = ParseYear();
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Year)));
            }
        }


        private int? ParseYear()
        {
            int y;
            return int.TryParse(Year, out y) ? (int?)y : null;
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

        public EditBookViewModel(BookInfo book, IMainWindowCodeBehind codeBehind, Action<BookInfo> action)
        {
            Book = book;
            _mainCodeBehind = codeBehind;
            update += action;
            query = new EditBookQuery();
        }


        RelayCommand _searchByISBNCommand;
        public RelayCommand SearchByISBNCommand
        {
            get => _searchByISBNCommand = _searchByISBNCommand ?? new RelayCommand(SearchByISBN);
        }
        void SearchByISBN()
        {
            if(Book.Book.ISBN != null)
            {
                GetBookByISBN();
                
            }
        }

        void GetBookByISBN()
        {
            AuthorDataParse data = new AuthorDataParse();
            if (data.GetAuthorByISBN(Book.Book.ISBN))
            {
         
                Book.Book.Title = data.Book.Title;
                if (data.Book.Description != null)
                    Book.Book.Description = data.Book.Description;
                if (data.Book.Image != null)
                    Image = data.Book.Image;
                if (data.Authors?.Count > 0 || data.Publisher.Name != null)
                {
                    SubsidiarySearchWindow window = new SubsidiarySearchWindow() { Width = 600, Height = 300, Title = "Добавление новых авторов и издателей" };
                    AddDataConfirm view = new AddDataConfirm();
                    AddDataConfirmViewModel vm = new AddDataConfirmViewModel(
                        new System.Collections.ObjectModel.ObservableCollection<Author>(data.Authors),
                        data.Publisher.Name != null ? data.Publisher : null,
                        _mainCodeBehind, AddAuthor, AddPublisher
                   );
                    view.DataContext = vm;
                    window.OutputView.Content = view;
                    window.ShowDialog();
                }
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Book)));
            }
            else System.Windows.MessageBox.Show("Не удалось выполнить поиск по ISBN");
        } 

        RelayCommand _loadImageCommand;

        public RelayCommand LoadImageCommand
        {
            get => _loadImageCommand = _loadImageCommand ?? new RelayCommand(LoadImage);
        }

        private void LoadImage()
        {
            OpenFileDialog openFile = new OpenFileDialog() { Filter = "jpg files(.jpg)|*.jpg|All files (*.*)|(*.*)", FilterIndex=1 };
            try
            {
                if (openFile.ShowDialog() == true)
                {
                    using (var br = new System.IO.BinaryReader(new System.IO.FileStream(openFile.FileName, System.IO.FileMode.Open)))
                    {
                        Image = br.ReadBytes((int)br.BaseStream.Length);
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }

        RelayCommand _saveChangesCommand;
        public RelayCommand SaveChangesCommand
        {
            get => _saveChangesCommand = _saveChangesCommand ?? new RelayCommand(SaveChanges);
        }

        private void SaveChanges()
        {
            if(Book.Book.ID != -1)
            {
                System.Windows.MessageBox.Show($"{Book.Book.ID}");
                query.Book = Book.Book;
                _mainCodeBehind?.EditBook(query);
                update?.Invoke(Book);
                
            } else {
                AddBook();
                update?.Invoke(Book);
                Book = new BookInfo() { Book = new Book { ID = -1 } };
                query = new EditBookQuery();
            }
        }

        private void AddBook()
        {
            _mainCodeBehind?.Add<AddBookQuery>(
                new AddBookQuery()
                {
                    Book = this.Book.Book,
                    Authors = this.Book.Authors,
                    Genres = this.Book.Genres,
                    Publishers = this.Book.Publishers,
                    Stories = (from item in this.Book.Stories select item.Story)
                }
            );
        }




        #region CopyPaste

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
            if (author != null && Book.Authors.Contains(author) == false)
            {
                Book.Authors.Add(author);
                query.AddedAuthors.Add(author.ID);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Book.Authors)));
            }
        }

        private void DeleteAuthor(object obj)
        {
            Author author = obj as Author;
            if (author != null && Book.Authors.Contains(author))
            {
                Book.Authors.Remove(author);
                query.DeletedAuthors.Add(author.ID);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Book.Authors)));
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
            if (genre != null && Book.Genres.Contains(genre) == false)
            {
                Book.Genres.Add(genre);
                query.AddedGenres.Add(genre.ID);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Book.Genres)));
            }
        }

        private void DeleteGenre(object obj)
        {
            Genre genre = obj as Genre;
            if (genre != null && Book.Genres.Contains(genre))
            {
                Book.Genres.Remove(genre);
                query.DeletedGenres.Add(genre.ID);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Book.Genres)));
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
            if (publisher != null && Book.Publishers.Contains(publisher) == false)
            {
                System.Windows.MessageBox.Show("YYE");
                Book.Publishers.Add(publisher);
                query.AddedPublishers.Add(publisher.ID);
                System.Windows.MessageBox.Show($"{query.AddedPublishers?.Count() ?? -1}");
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Book.Publishers)));
            }
        }
        private void DeletePublisher(object obj)
        {
            Publisher publisher = obj as Publisher;
            if (publisher != null && Book.Publishers.Contains(publisher))
            {
                Book.Publishers.Remove(publisher);
                query.DeletedPublishers.Add(publisher.ID);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Book.Publishers)));
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
            if (story != null && Book.Stories.Contains(story) == false)
            {
                Book.Stories.Add(story);
                query.AddedStories.Add(story.Story.ID);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Book.Stories)));
            }
        }

        private void DeleteStory(object obj)
        {
            StoryInfo story = obj as StoryInfo;
            if (story != null && Book.Stories.Contains(story))
            {
                Book.Stories.Remove(story);
                query.DeletedStories.Add(story.Story.ID);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Book.Stories)));
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
            CoverTypeSearchViewModel vm = new CoverTypeSearchViewModel(_mainCodeBehind, SelectCoverType);
            view.DataContext = vm;
            window.OutputView.Content = view;

            window.ShowDialog();
        }

        private void SelectCoverType(CoverType cover)
        {
            if (cover != null)
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

        #endregion
    }
}
