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
    class LocationSearchViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        IMainWindowCodeBehind _mainCodeBehind;
        ObservableCollection<Location> _locations;
        Location _selectedLocation;
        Action<Location> updateSelectedLocation;
        string _rack, _shelf;

        public ObservableCollection<Location> Locations
        {
            get => _locations = _locations ?? new ObservableCollection<Location>();
            set
            {
                _locations = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Locations)));
            }
        }

        public Location SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                _selectedLocation = value;
                updateSelectedLocation?.Invoke(_selectedLocation);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedLocation)));
            }
        }

        public string Rack
        {
            get => _rack;
            set
            {
                _rack = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Rack)));
            }
        }

        public string Shelf
        {
            get => _shelf;
            set
            {
                _shelf = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Shelf)));
            }
        }

        public LocationSearchViewModel(IMainWindowCodeBehind codeBehind, Action<Location> aciton)
        {
            _mainCodeBehind = codeBehind;
            updateSelectedLocation += aciton;
        }

        RelayCommand _findLocationsCommand;
        public RelayCommand FindLocationsCommand
        {
            get => _findLocationsCommand = _findLocationsCommand ?? new RelayCommand(FindLocations);
        }

        private void FindLocations()
        {
            int s = 0;
            int? shelf = int.TryParse(Shelf, out s) ? (int?)s : null;

            Locations = _mainCodeBehind?.FindLoactions(
                new Location()
                {
                    Rack = this.Rack,
                    Shelf = s
                }
            );
        }
    }
}
