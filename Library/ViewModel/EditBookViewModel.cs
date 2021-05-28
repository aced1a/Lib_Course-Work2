using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Library.Model.LibraryEntities;


namespace Library.ViewModel
{
    class EditBookViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        IMainWindowCodeBehind _mainCodeBehind;
        Action<Book> update;
        Book _book;

        public Book Book
        {
            get => _book ?? new Book();
            set
            {
                _book = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Book)));
            }
        }


    }
}
