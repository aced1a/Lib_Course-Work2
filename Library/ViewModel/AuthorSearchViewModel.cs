using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Library.Models;

using Library.Query;

namespace Library.ViewModel
{
    class AuthorSearchViewModel : INotifyPropertyChanged
    {
        Func<Author, ObservableCollection<Author>> search;
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

        public event PropertyChangedEventHandler PropertyChanged = delegate { };


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


        public AuthorSearchViewModel(Func<Author, ObservableCollection<Author>> func)
        {
            search += func;
        }


        RelayCommand _sendQueryCommand;
        public RelayCommand SendQueryCommand
        {
            get
            {
                return _sendQueryCommand = _sendQueryCommand ?? new RelayCommand(SendFindQuery);
            }
        }

        private void SendFindQuery()
        {
            if (FirstName == "KEKW")
            {
                Authors[0].MiddleName = "KEKW";
            }
            else
            {
                Authors = search?.Invoke(GetFindQuery());
                if (Authors == null)
                    System.Windows.MessageBox.Show("It's null");
                else
                    System.Windows.MessageBox.Show(Authors.Count.ToString());
            }
        }

        private Author GetFindQuery()
        {
            Author author = new Author
            {
                ID = -1,
                LastName = this.LastName,
                FirstName = this.FirstName,
                MiddleName = this.MiddleName
            };

            return author;
        }
    }
}
