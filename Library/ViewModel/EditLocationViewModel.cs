using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Library.Model.LibraryEntities;
using Library.View;

namespace Library.ViewModel
{
    class EditLocationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        IMainWindowCodeBehind _mainCodeBehind;
        Location _location;
        Action<Location> update;
        string _shelf;

        public string Shelf
        {
            get => _shelf;
            set
            {
                _shelf = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Shelf)));
                Location.Shelf = ParseShelf();
            }
        }

        int? ParseShelf()
        {
            int s;
            return int.TryParse(Shelf, out s) ? (int?)s : null;
        }

        public Location Location
        {
            get => _location = _location ?? new Location() { ID = -1 };
            set
            {
                _location = value;
                Shelf = _location.Shelf?.ToString();
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Location)));
            }
        }

        public EditLocationViewModel(Location location, IMainWindowCodeBehind codeBehind, Action<Location> action = null)
        {
            Location = location;
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
            if (Location.ID != -1)
            {
                _mainCodeBehind?.SaveChanges();
                update?.Invoke(Location);

            }
            else
            {
                _mainCodeBehind?.Add(Location);
                update?.Invoke(Location);
                Location = new Location() {  ID = -1 };
            }
        }
    }
}
