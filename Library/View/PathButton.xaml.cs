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

namespace Library.View
{
    public partial class PathButton : UserControl
    {
        public static DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(PathButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(Icon_Changed)));

        public object Icon
        {
            get { return (object)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        private static void Icon_Changed(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            PathButton thisClass = o as PathButton;
            thisClass?.IconData();
        }

        private void IconData()
        {
            buttonSurface.Content = Icon;
        }



        public static DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof(string), typeof(PathButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(Caption_Changed)));

        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        private static void Caption_Changed(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            PathButton thisClass = o as PathButton;
            thisClass?.SetCaption();
        }

        private void SetCaption()
        {
            buttonCaption.Content = Caption;
        }

        public static DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(PathButton), new FrameworkPropertyMetadata(new PropertyChangedCallback(Command_Changed)));

        public ICommand Command 
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        
        private static void Command_Changed(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            PathButton thisClass = o as PathButton;
            thisClass?.SetCommand();
        }

        private void SetCommand()
        {
            button.Command = Command;
        }


        public PathButton()
        {
            InitializeComponent();
        }


    }
}
