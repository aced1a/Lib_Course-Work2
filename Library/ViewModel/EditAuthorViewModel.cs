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
    class EditAuthorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        IMainWindowCodeBehind _mainCodeBehind;
        Action<Author> update;
        Author _author;

        public Author Author
        {
            get => _author = _author ?? new Author();
            set
            {
                _author = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Author)));
            }
        }

        public string FirstName
        {
            get => Author.FirstName;
            set
            {
                Author.FirstName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(FirstName)));
            }
        }

        public string MiddleName
        {
            get => Author.MiddleName;
            set
            {
                Author.MiddleName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(MiddleName)));
            }
        }

        public string LastName
        {
            get => Author.LastName;
            set
            {
                Author.LastName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(LastName)));
            }
        }

        public EditAuthorViewModel(Author author, IMainWindowCodeBehind codeBehind, Action<Author> action=null)
        {
            Author = author;
            _mainCodeBehind = codeBehind;
            update += action;
        }

        RelayCommand _saveChangesCommand;
        public RelayCommand SaveChangesCommand
        {
            get => _saveChangesCommand = _saveChangesCommand ?? new RelayCommand(SaveChanges);
        }

        private void SaveChanges()
        {
            if (Author.ID != -1)
            {
                _mainCodeBehind?.SaveChanges();
                update?.Invoke(Author);
            }
            else
            {
                _mainCodeBehind?.Add(Author);
                update?.Invoke(Author);
                Author = new Author() { ID = -1 };
            }
        }
    }
}
