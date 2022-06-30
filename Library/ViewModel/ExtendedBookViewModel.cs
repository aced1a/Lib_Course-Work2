using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Model.LibraryEntities;
using Library.View;
using System.ComponentModel;

namespace Library.ViewModel
{
    class ExtendedBookViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        BookInfo _book;
        BookSearchViewModel _vm;
        Action<System.Windows.Controls.UserControl> _chgView;

        public BookInfo Book
        {
            get => _book ?? new BookInfo();
            set {
                _book = value ;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Book)));
            }
        }

        public ExtendedBookViewModel(BookInfo book, BookSearchViewModel vm = null, Action<System.Windows.Controls.UserControl> action = null)
        {
            Book = book;
            _vm = vm;
            _chgView += action;
        }


        RelayCommand _chgViewCommand;
        public RelayCommand ChangeViewCommand
        {
            get => _chgViewCommand = _chgViewCommand ?? new RelayCommand(ComeBack);
        }

        void ComeBack()
        {
            if (_vm != null )
            {
                _chgView?.Invoke(new BookSearch() { DataContext = _vm });
            }
        }

    }
}
