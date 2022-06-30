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
    /// <summary>
    /// Логика взаимодействия для BookSearch.xaml
    /// </summary>
    public partial class BookSearch : UserControl
    {
        bool hided = false;
        GridLength length;

        public BookSearch()
        {
            InitializeComponent();
            length = firstRow.Height;
        }

        private void Hide(object sender, RoutedEventArgs e)//MouseButtonEventArgs e)
        {
            if (hided)
            {
                first.Visibility = Visibility.Visible;
                second.Visibility = Visibility.Visible;
                third.Visibility = Visibility.Visible;
                four.Visibility = Visibility.Visible;
                firstRow.Height = length;
                HideButton.Icon = App.Current.FindResource("UpArrowIcon");
            }
            else
            {
                first.Visibility = Visibility.Collapsed;
                second.Visibility = Visibility.Collapsed;
                third.Visibility = Visibility.Collapsed;
                four.Visibility = Visibility.Collapsed;
                firstRow.Height = GridLength.Auto;
                HideButton.Icon = App.Current.FindResource("DownArrowIcon");
            }
            hided = !hided;
        }

        private void YearTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Char.IsDigit(e.Text, 0) == false)
            {
                e.Handled = true;
            }
        }

        private void ISBN_Input(object sender, TextCompositionEventArgs e)
        {
            if((sender as TextBox)?.Text.Length > 12)
            {
                e.Handled = true;
            }
        }
    }
}
