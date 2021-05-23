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
using Library.Models;
using Library.Query;
using System.Collections.ObjectModel;

namespace Library
{
    enum ViewType { SearchMode, InfoMode, EditMode}    
    
    public partial class MainWindow : Window
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
            LoadView(ViewType.SearchMode);
        }

        private void LoadView(ViewType type, SearchViewType searchType=SearchViewType.AuthorsSearch)
        {
            switch (type)
            {
                case ViewType.SearchMode:
                    ASideButtonMenu view = new ASideButtonMenu();
                    //
                    //
                    PanelView.Content = view;
                    LoadSearchView(searchType);
                    //SearchMode view = new SearchMode();
                    //SearchModeViewModel vm = new SearchModeViewModel();
                    //view.DataContext = vm;
                    //this.OutputView.Content = view;
                    break;
                default:
                    break;
            }
        }

        private void LoadSearchView(SearchViewType type)
        {
            switch (type)
            {
                case SearchViewType.AuthorsSearch:
                    AuthorSearch view = new AuthorSearch();
                    AuthorSearchViewModel vm = new AuthorSearchViewModel(SendFindAuthorQuery);
                    view.DataContext = vm;
                    OutputView.Content = view;
                    break;
            }
        }

        private ObservableCollection<Author> SendFindAuthorQuery(Author author) 
        {
            MessageBox.Show(author.FullName);
            return new ObservableCollection<Author>(dal?.FindAuthor(author));
            //return null;
        }
    }
}
