using System;

using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Library.View;
using Library.ViewModel;
using Library.Model.LibraryEntities;
using Library.Query;
using System.Collections.ObjectModel;

namespace Library
{
    enum ViewType { SearchMode, Settings};
    
    enum SearchViewType { AuthorsSearch, BooksSearch, PublishersSearch, GenresSearch, StoriesSearch, BindingTypesSearch, CoverTypesSearch };

    enum PanelType { FirstPanel, SecondPanel };

    interface IMainWindowCodeBehind
    {
        void LoadThisView(UserControl control);
        ObservableCollection<Author> FindAuthors(Author author);
        ObservableCollection<Genre> FindGenres(Genre genre);
        ObservableCollection<Publisher> FindPublishers(Publisher publisher);
        ObservableCollection<BookInfo> FindBooks(FindBookQuery query);
        ObservableCollection<StoryInfo> FindStories(FindStoryQuery query);
        ObservableCollection<CoverType> FindCoverTypes(CoverType coverType);
        ObservableCollection<BindingType> FindBindingTypes(BindingType bindingType);
        ObservableCollection<Location> FindLocations(Location location);
        void EditBook(EditBookQuery query);
        //void EditStory(EditStoryQuery query);
        void SaveChanges();
        void Delete<T>(T item);
        void Add<T>(T item);
        void EditStory(EditStoryQuery query);

    }


    public partial class MainWindow : Window, IMainWindowCodeBehind
    {
        LibraryDAL dal = null;

        public MainWindow()
        {
            InitializeComponent();
            App.Current.Resources.Add("EditAccess", Visibility.Visible);
            InitDAL();
            Loaded += MainWindow_Loaded;
        }

        private void InitDAL()
        {
            try
            {
                dal = new LibraryDAL();
            }
            catch (Exception)
            {
                dal = null;
            }
           
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs args)
        {
            ChangePanel(PanelType.FirstPanel);
            LoadView(ViewType.SearchMode);
        }

        public void LoadThisView(UserControl control)
        {
            OutputView.Content = control;
        }

        private void LoadView(ViewType type, SearchViewType searchType=SearchViewType.AuthorsSearch)
        {
            switch (type)
            {
                case ViewType.SearchMode:
                    LoadSearchView(searchType);
                    break;
                case ViewType.Settings:
                    Settings view = new Settings();
                    SettingsViewModel vm = new SettingsViewModel();
                    view.DataContext = vm;
                    OutputView.Content = view;
                    break;
                default:
                    break;
            }
        }

        private void ChangePanel(PanelType type)
        {
            SelectDictionaryMenuViewModel vm = null;
            switch (type)
            {
                case PanelType.FirstPanel:
                    SelectDictionaryMenu view = new SelectDictionaryMenu();
                    vm = new SelectDictionaryMenuViewModel(LoadView, ChangePanel);
                    view.DataContext = vm;
                    PanelView.Content = view;
                    break;
                case PanelType.SecondPanel:
                    SelectedDictionaryMenuSecondPage view_second = new SelectedDictionaryMenuSecondPage();
                    vm = new SelectDictionaryMenuViewModel(LoadView, ChangePanel);
                    view_second.DataContext = vm;
                    PanelView.Content = view_second;
                    break;
                default:
                    break;
            }
        }

        private void LoadSearchView(SearchViewType type)
        {
            switch (type)
            {
                case SearchViewType.BooksSearch:
                    BookSearch bview = new BookSearch();
                    BookSearchViewModel bvm = new BookSearchViewModel(this);
                    bview.DataContext = bvm;
                    OutputView.Content = bview;
                    break;
                case SearchViewType.GenresSearch:
                    GenreSearch gview = new GenreSearch();
                    GenreSearchViewModel gvm = new GenreSearchViewModel(this);
                    gview.DataContext = gvm;
                    OutputView.Content = gview;
                    break;
                case SearchViewType.AuthorsSearch:
                    AuthorSearch aview = new AuthorSearch();
                    AuthorSearchViewModel avm = new AuthorSearchViewModel(this);
                    aview.DataContext = avm;
                    OutputView.Content = aview;
                    break;
                case SearchViewType.PublishersSearch:
                    PublisherSearch pview = new PublisherSearch();
                    PublisherSearchViewModel pvm = new PublisherSearchViewModel(this);
                    pview.DataContext = pvm;
                    OutputView.Content = pview;
                    break;
                case SearchViewType.StoriesSearch:
                    StorySearch sview = new StorySearch();
                    StorySearchViewModel svm = new StorySearchViewModel(this);
                    sview.DataContext = svm;
                    OutputView.Content = sview;
                    break;
                case SearchViewType.BindingTypesSearch:
                    BindingTypeSearch btview = new BindingTypeSearch();
                    BindingTypeSearchViewModel btvm = new BindingTypeSearchViewModel(this);
                    btview.DataContext = btvm;
                    OutputView.Content = btview;
                    break;
                case SearchViewType.CoverTypesSearch:
                    CoverTypeSearch ctview = new CoverTypeSearch();
                    CoverTypeSearchViewModel ctvm = new CoverTypeSearchViewModel(this);
                    ctview.DataContext = ctvm;
                    OutputView.Content = ctview;
                    break;
                default:
                    break;
            }
        }

        public ObservableCollection<Author> FindAuthors(Author author) 
        {
            return dal != null ? new ObservableCollection<Author>(dal?.FindAuthor(author)) : null; 
        }

        public ObservableCollection<Genre> FindGenres(Genre genre) 
        {
            return dal != null ? new ObservableCollection<Genre>(dal?.FindGenre(genre)) : null;
        }
        public ObservableCollection<Publisher> FindPublishers(Publisher publisher) 
        { 
            return dal != null ? new ObservableCollection<Publisher>(dal?.FindPublishers(publisher)) : null;
        }
        public ObservableCollection<BookInfo> FindBooks(FindBookQuery query) 
        {
            return dal != null ? new ObservableCollection<BookInfo>((from item in dal?.FindBook(query) select new BookInfo() { Book = item})) : null;
        }
        public ObservableCollection<StoryInfo> FindStories(FindStoryQuery query) 
        {
            return dal != null ? new ObservableCollection<StoryInfo>(from item in dal?.FindStory(query) select new StoryInfo() { Story = item }) : null;
        }

        public ObservableCollection<CoverType> FindCoverTypes(CoverType coverType) 
        {
            return dal != null ? new ObservableCollection<CoverType>(dal?.FindCoverType(coverType)) : null;
        }
        public ObservableCollection<BindingType> FindBindingTypes(BindingType bindingType) 
        { return dal != null ? new ObservableCollection<BindingType>(dal?.FindBindingType(bindingType)) : null; }
        public ObservableCollection<Location> FindLocations(Location location) 
        {
            return dal != null ? new ObservableCollection<Location>(dal?.FindLocation(location)) : null;
        }

        public void SaveChanges()
        {
            if (dal?.SaveChanges() == false)
            {
                MessageBox.Show("Не удалось сохранить изменения");
            }
        }

        public void EditBook(EditBookQuery query) 
        {
            if (dal?.EditBook(query) == false)
            {
                MessageBox.Show("Не удалось изменить книгу");
            }
        }

        public void EditStory(EditStoryQuery query)
        {
            if (dal?.EditStory(query) == false)
            {
                MessageBox.Show("Не удалось изменить рассказ");
            }
        }

        public void Add<T>(T item)
        {
            bool? result = false;
            switch (item) 
            {
                case Author a:
                    result = dal?.AddAuthor(a);
                    break;
                case Genre g:
                    result = dal?.AddGenre(g);
                    break;
                case Publisher p:
                    result = dal?.AddPublisher(p);
                    break;
                case AddBookQuery query:
                    result = dal?.AddBook(query);
                    break;
                case AddStoryQuery query:
                    result = dal?.AddStory(query);
                    break;
                case Location location:
                    result = dal?.AddLocation(location);
                    break;
                case BindingType type:
                    result = dal?.AddBindingType(type);
                    break;
                case CoverType type:
                    result = dal?.AddCoverType(type);
                    break;
                default:
                    break;
            }
            if(result == false)
            {
                MessageBox.Show("Не удалось добавить элемент");
            }
        }

        public void Delete<T>(T item)
        {
            bool? result = false;
            switch (item)
            {
                case Author a:
                    result = dal?.DeleteAuthor(a);
                    break;
                case Genre g:
                    result = dal?.DeleteGenre(g);
                    break;
                case Publisher p:
                    result = dal?.DeletePublisher(p);
                    break;
                case Book book:
                    result = dal?.DeleteBook(book);
                    break;
                case Story story:
                    result = dal?.DeleteStory(story);
                    break;
                case Location location:
                    result = dal?.DeleteLocation(location);
                    break;
                case BindingType type:
                    result = dal?.DeleteBindingType(type);
                    break;
                case CoverType type:
                    result = dal?.DeleteCoverType(type);
                    break;
                default:
                    break;
            }
            if (result == false)
            {
                MessageBox.Show("Не удалось удалить элемент");
            }
        }
    }
}
