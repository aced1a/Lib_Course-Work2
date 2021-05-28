using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Library.Model.LibraryEntities;

namespace Library.ViewModel
{
    class EditBindingTypeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        IMainWindowCodeBehind _mainCodeBehind;
        Action<BindingType> update;
        BindingType _item;

        public BindingType Item
        {
            get => _item = _item ?? new BindingType();
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

        public EditBindingTypeViewModel(BindingType item, IMainWindowCodeBehind codeBehind, Action<BindingType> action=null)
        {
            _mainCodeBehind = codeBehind;
            Item = item;
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
            Item = new BindingType() { ID = -1 };
        }

    }
}
