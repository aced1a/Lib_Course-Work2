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
    class EditPublisherViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        IMainWindowCodeBehind _mainCodeBehind;
        Action<Publisher> update;
        Publisher _item;

        public Publisher Item
        {
            get => _item = _item ?? new Publisher();
            set
            {
                _item = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Item)));
            }
        }

        public string Name
        {
            get => Item.Name;
            set
            {
                Item.Name = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public EditPublisherViewModel(Publisher publisher, IMainWindowCodeBehind codeBehind, Action<Publisher> action = null)
        {
            _mainCodeBehind = codeBehind;
            Item = publisher;
            update += action;
        }

        RelayCommand _saveChangesCommand;
        public RelayCommand SaveChangesCommand
        {
            get => _saveChangesCommand = _saveChangesCommand ?? new RelayCommand(SaveChanges);
        }

        private void SaveChanges()
        {
            if (Item.ID != -1)
                _mainCodeBehind?.SaveChanges();
            else
                _mainCodeBehind?.Add(Item);
            update?.Invoke(Item);
            Item = new Publisher() { ID = -1 };
        }

    }
}
