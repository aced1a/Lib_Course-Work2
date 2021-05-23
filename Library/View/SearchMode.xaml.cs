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
using Library.ViewModel;

namespace Library.View
{
    enum SearchViewType { BooksSearch, AuthorsSearch, GenresSearch, PublishersSearch, StoriesSearch };

    public partial class SearchMode : UserControl
    {
        public SearchMode()
        {
            InitializeComponent();
            Loaded += SearchMode_Loaded;
        }

        private void SearchMode_Loaded(object sender, RoutedEventArgs args)
        {
            LoadSearchView(SearchViewType.AuthorsSearch);
        }

        private void LoadSearchView(SearchViewType type)
        {
            switch (type)
            {
                case SearchViewType.AuthorsSearch:
                    AuthorSearch view = new AuthorSearch();
                    //AuthorSearchViewModel vm = new AuthorSearchViewModel();
                    //view.DataContext = vm;
                    OutputView.Content = view;
                    break;
                default:
                    break;
            }
        }
    }
}
