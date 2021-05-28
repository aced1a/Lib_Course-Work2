using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Library.Model.LibraryEntities;
using Library.View;

namespace Library.ViewModel
{
    class AuthorSearchViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        IMainWindowCodeBehind _mainCodeBehind;

        string _firstName, _lastName, _middleName;

        private ObservableCollection<Author> _authors;
        public ObservableCollection<Author> Authors {
            get
            {
                return _authors = _authors ?? new ObservableCollection<Author>();
            }
            private set
            {
                _authors = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Authors)));
            }
        }

        private Author _selectedAuthor;
        public Author SelectedAuthor
        {
            get => _selectedAuthor;
            set
            {
                _selectedAuthor = value;
                updateSelectedAuthors?.Invoke(_selectedAuthor);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedAuthor)));
            }
        }
        Action<Author> updateSelectedAuthors;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(FirstName)));
            }
        }
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(LastName)));
            }
        }
        public string MiddleName
        {
            get { return _middleName; }
            set
            {
                _middleName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(MiddleName)));
            }
        }


        public AuthorSearchViewModel(IMainWindowCodeBehind codeBehind, Action<Author> action=null)
        {
            _mainCodeBehind = codeBehind;
            updateSelectedAuthors += action;
        }


        RelayCommand _findAuthorsCommand;
        public RelayCommand FindAuthorsCommand
        {
            get
            {
                return _findAuthorsCommand = _findAuthorsCommand ?? new RelayCommand(FindAuthors);
            }
        }

        private void FindAuthors()
        {
            Authors = _mainCodeBehind.FindAuthors(
                new Author
                {
                    ID = -1,
                    LastName = this.LastName,
                    FirstName = this.FirstName,
                    MiddleName = this.MiddleName
                }
            );
        }


        RelayCommand _addAuthorCommand;
        public RelayCommand AddAuthorCommand
        {
            get => _addAuthorCommand = _addAuthorCommand ?? new RelayCommand(AddAuthor);
        }

        RelayCommand _chgAuthorCommand;
        public RelayCommand ChangeAuthorCommand
        {
            get => _chgAuthorCommand = _chgAuthorCommand ?? new RelayCommand(ChgAuthor);
        }

        RelayCommand _deleteAuthorCommand;
        public RelayCommand DeleteAuthorCommand
        {
            get => _deleteAuthorCommand = _deleteAuthorCommand ?? new RelayCommand(DeleteAuthor);
        }

        private void AddAuthor() 
        {
            OpenEditWindow(
                new Author()
                {
                    ID = -1
                }    
            );   
        }

        private void ChgAuthor()
        {
            if(SelectedAuthor != null)
                OpenEditWindow(SelectedAuthor);
        }

        private void DeleteAuthor()
        {
            if(SelectedAuthor != null)
            {
                _mainCodeBehind?.Delete(SelectedAuthor);
                Authors.Remove(SelectedAuthor);
            }
        }

        private void OpenEditWindow(Author author)
        {
            SubsidiarySearchWindow window = new SubsidiarySearchWindow();
            EditAuthor view = new EditAuthor();
            EditAuthorViewModel vm = new EditAuthorViewModel(author, _mainCodeBehind, UpdateItems);
            view.DataContext = vm;
            window.OutputView.Content = view;

            window.ShowDialog();
        }

        private void UpdateItems(Author item)
        {
            if (item != null && Authors.Contains(item) == false)
            {
                Authors.Add(item);
            }
            else
            {
                Authors.Remove(item);
                Authors.Add(item);
            }
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Authors)));
        }
    }
}
