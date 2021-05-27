using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Library.Models;
using Library.View;

namespace Library.ViewModel
{
    class PublisherSearchViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        IMainWindowCodeBehind _mainCodeBehind;
        ObservableCollection<Publisher> _publishers;
        Action<Publisher> updateSelectedPublishers;

        public ObservableCollection<Publisher> Publishers
        {
            get
            {
                return _publishers = _publishers ?? new ObservableCollection<Publisher>();
            }
            private set
            {
                _publishers = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Publishers)));
            }
        }

        private Publisher _selectedPublisher;
        public Publisher SelectedPublisher
        {
            get => _selectedPublisher;
            set
            {
                _selectedPublisher = value;
                updateSelectedPublishers?.Invoke(_selectedPublisher);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedPublisher)));
            }
        }

        private string _publisherName;
        public string PublisherName
        {
            get => _publisherName;
            set
            {
                _publisherName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(PublisherName)));
            }
        }

        public PublisherSearchViewModel(IMainWindowCodeBehind codeBehind, Action<Publisher> action=null)
        {
            _mainCodeBehind = codeBehind;
            updateSelectedPublishers += action;
        }

         private RelayCommand _findPublishersCommand;
        public RelayCommand FindPublishersCommand
        {
            get => _findPublishersCommand = _findPublishersCommand ?? new RelayCommand(FindPublishers);
        }

        private void FindPublishers()
        {
            Publishers = _mainCodeBehind?.FindPublishers(
                new Publisher()
                {
                    ID = -1,
                    Name = PublisherName
                }
            );
        }

        RelayCommand _addPublisherCommand;
        public RelayCommand AddPublisherCommand
        {
            get => _addPublisherCommand = _addPublisherCommand ?? new RelayCommand(AddPublisher);
        }

        RelayCommand _chgPublisherCommand;
        public RelayCommand ChangePublisherCommand
        {
            get => _chgPublisherCommand = _chgPublisherCommand ?? new RelayCommand(ChgPublisher);
        }

        RelayCommand _deletePublisherCommand;
        public RelayCommand DeletePublisherCommand
        {
            get => _deletePublisherCommand = _deletePublisherCommand ?? new RelayCommand(DeletePublisher);
        }

        private void AddPublisher()
        {
            OpenEditWindow(
                new Publisher()
                {
                    ID = -1
                }
            );
        }

        private void ChgPublisher()
        {
            if (SelectedPublisher != null)
                OpenEditWindow(SelectedPublisher);
        }

        private void DeletePublisher()
        {
            if (SelectedPublisher != null)
            {
                _mainCodeBehind?.Delete(SelectedPublisher);
                Publishers.Remove(SelectedPublisher);
            }
        }

        private void OpenEditWindow(Publisher publisher)
        {
            SubsidiarySearchWindow window = new SubsidiarySearchWindow();
            EditElement view = new EditElement();
            EditPublisherViewModel vm = new EditPublisherViewModel(publisher, _mainCodeBehind, UpdateItems);
            view.DataContext = vm;
            window.OutputView.Content = view;

            window.ShowDialog();
            //UpdateItems(vm.Item);
        }

        private void UpdateItems(Publisher item)
        {
            if (item != null && Publishers.Contains(item) == false)
            {
                Publishers.Add(item);
            }
            else
            {
                Publishers.Remove(item);
                Publishers.Add(item);
            }
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Publishers)));
        }
    }
}
