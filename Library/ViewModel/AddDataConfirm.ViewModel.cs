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
    class AddDataConfirmViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        IMainWindowCodeBehind _mainCodeBehind;
        int _index=-1;
        Action<Author> updateAuthors;
        Action<Publisher> updatePublisher;

        public int SelectedIndex
        {
            get => _index;
            set
            {
                _index = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedIndex)));
            }
        }

        private ObservableCollection<Author> _authors;
        public ObservableCollection<Author> Authors
        {
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

        private Publisher _publisher;
        public Publisher Publisher
        {
            get => _publisher;
            set
            {
                _publisher = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Publisher)));
            }
        }

        private Author _selectedAuthor;
        public Author SelectedAuthor
        {
            get => _selectedAuthor;
            set
            {
                _selectedAuthor = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedAuthor)));
            }
        }

        public List<bool> AuthorsIsChecked { get; set; }
        public bool PublisherIsChecked { get; set; }

        public AddDataConfirmViewModel(ObservableCollection<Author> authors, Publisher publisher,
            IMainWindowCodeBehind codeBehind, Action<Author> action_a=null, Action<Publisher> action_p=null)
        {
            Authors = authors;
            Publisher = publisher;
            AuthorsIsChecked = new List<bool>(from item in Enumerable.Range(0, authors.Count) select false);
            _mainCodeBehind = codeBehind;
            PublisherIsChecked = false;
            updateAuthors += action_a;
            updatePublisher += action_p;

        }

        RelayCommand _checkAuthorCommand;
        public RelayCommand CheckAuthorCommand
        {
            get => _checkAuthorCommand = _checkAuthorCommand ?? new RelayCommand(AuthorCheck);
        }

        void AuthorCheck()
        {
           if(SelectedAuthor != null && SelectedIndex != -1 && SelectedIndex < AuthorsIsChecked.Count())
            {
                AuthorsIsChecked[(int)SelectedIndex] = !AuthorsIsChecked[(int)SelectedIndex];
            }
        }

        RelayCommand _checkPublisherCommand;
        public RelayCommand CheckPublisherCommand
        {
            get => _checkPublisherCommand = _checkPublisherCommand ?? new RelayCommand(PublisherCheck);
        }

        void PublisherCheck()
        {
            PublisherIsChecked = !PublisherIsChecked;
        }

        RelayCommand _addCommand;
        public RelayCommand AddCommand
        {
            get => _addCommand = _addCommand ?? new RelayCommand(Add);
        }

        void Add() 
        {
            int count = 0;
            foreach(var item in Authors)
            {
                if(AuthorsIsChecked[count]) { _mainCodeBehind?.Add(item); updateAuthors?.Invoke(item); }
                count++;
            }
            if(Publisher != null && PublisherIsChecked) { _mainCodeBehind?.Add(Publisher); updatePublisher?.Invoke(Publisher); }
        }
    }
}
