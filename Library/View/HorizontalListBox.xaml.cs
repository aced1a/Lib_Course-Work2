using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Library.View
{
    /// <summary>
    /// Логика взаимодействия для HorizontalListBox.xaml
    /// </summary>
    public partial class HorizontalListBox : UserControl
    {
        public HorizontalListBox()
        {
            InitializeComponent();
        }


        public static DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(HorizontalListBox), new FrameworkPropertyMetadata(null));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }




        public static DependencyProperty DataSourceProperty = DependencyProperty.Register("DataSource", typeof(IEnumerable), typeof(HorizontalListBox), new FrameworkPropertyMetadata(new PropertyChangedCallback(DataSource_Changed)));
        public IEnumerable DataSource
        {
            get => (IEnumerable)GetValue(DataSourceProperty);
            set => SetValue(DataSourceProperty, value);
        }

        private static void DataSource_Changed(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            HorizontalListBox listBox = o as HorizontalListBox;
            listBox?.SetDataSource();
        }

        private void SetDataSource()
        {
            listBox.ItemsSource = DataSource;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            Command?.Execute(b.Tag);
        }
    }
}
