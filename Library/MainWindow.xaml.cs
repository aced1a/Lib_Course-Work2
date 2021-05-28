using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using Library.View;
using Library.ViewModel;
using Library.Model.LibraryEntities;
using Library.Query;
using System.Collections.ObjectModel;

namespace Library
{
    enum ViewType { SearchMode, InfoMode, EditMode, Settings};
    
    enum SearchViewType { AuthorsSearch, BooksSearch, PublishersSearch, GenresSearch, StoriesSearch, BindingTypesSearch, CoverTypesSearch };

    enum PanelType { FirstPanel, SecondPanel };

    interface IMainWindowCodeBehind 
    {
        ObservableCollection<Author> FindAuthors(Author author);
        ObservableCollection<Genre> FindGenres(Genre genre);
        ObservableCollection<Publisher> FindPublishers(Publisher publisher);
        ObservableCollection<Book> FindBooks(FindBookQuery query);
        ObservableCollection<Story> FindStories(FindStoryQuery query);
        ObservableCollection<CoverType> FindCoverTypes(CoverType coverType);
        ObservableCollection<BindingType> FindBindingTypes(BindingType bindingType);
        ObservableCollection<Location> FindLoactions(Location location);
        void SaveChanges();
        void Delete<T>(T item);
        void Add<T>(T item);

    }


    public partial class MainWindow : Window, IMainWindowCodeBehind
    {
        LibraryDAL dal;

        public MainWindow()
        {
            InitializeComponent();
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
                MessageBox.Show("Не удалось подключиться к базе данных");
                dal = null;
            } 
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs args)
        {
            ChangePanel(PanelType.FirstPanel);
            LoadView(ViewType.SearchMode);
        }

        private void LoadView(ViewType type, SearchViewType searchType=SearchViewType.AuthorsSearch)
        {
            switch (type)
            {
                case ViewType.SearchMode:
                    LoadSearchView(searchType);
                    break;
                case ViewType.Settings:
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
            return new ObservableCollection<Author>(dal?.FindAuthor(author)); 
        }

        public ObservableCollection<Genre> FindGenres(Genre genre) 
        {
            return new ObservableCollection<Genre>(dal?.FindGenre(genre));
        }
        public ObservableCollection<Publisher> FindPublishers(Publisher publisher) 
        { 
            return new ObservableCollection<Publisher>(dal?.FindPublishers(publisher));
        }
        public ObservableCollection<Book> FindBooks(FindBookQuery query) 
        {
            return new ObservableCollection<Book>(dal?.FindBook(query));
        }
        public ObservableCollection<Story> FindStories(FindStoryQuery query) 
        {
            return new ObservableCollection<Story>(dal?.FindStory(query));
        }

        public ObservableCollection<CoverType> FindCoverTypes(CoverType coverType) 
        {
            return new ObservableCollection<CoverType>(dal?.FindCoverType(coverType));
        }
        public ObservableCollection<BindingType> FindBindingTypes(BindingType bindingType) { return null; }
        public ObservableCollection<Location> FindLoactions(Location location) { return null; }

        public void SaveChanges()
        {
            if (dal?.SaveChanges() == false)
            {
                MessageBox.Show("Не удалось сохранить изменения");
            }
        }

        //public ObservableCollection<T> Search<T>(T item)
        //{
        //    switch (item)
        //    {
        //        case Author author: return new ObservableCollection<T>(dal?.FindAuthor(author));
        //        case Genre genre: return new ObservableCollection<T>(dal?.FindGenre(genre));
        //        default:
        //            return null;
        //    }
        //}

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
