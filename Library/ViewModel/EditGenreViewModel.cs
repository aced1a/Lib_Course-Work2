using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Library.Models;

namespace Library.ViewModel
{
    class EditGenreViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        IMainWindowCodeBehind _mainCodeBehind;
        Action<Genre> update;
        Genre _item;

        public Genre Item
        {
            get => _item = _item ?? new Genre();
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

        public EditGenreViewModel(Genre genre, IMainWindowCodeBehind codeBehind, Action<Genre> action=null)
        {
            _mainCodeBehind = codeBehind;
            Item = genre;
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
            Item = new Genre();
        }
    }
}
