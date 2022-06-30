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

            Locations = _mainCodeBehind?.FindLocations(
                new Location()
                {
                    Rack = this.Rack,
                    Shelf = shelf
                }
            );
        }

        #region copypaste
            RelayCommand _addLocationCommand;
            public RelayCommand AddLocationCommand
            {
                get => _addLocationCommand = _addLocationCommand ?? new RelayCommand(AddLocation);
            }

            RelayCommand _editLocationCommand;
            public RelayCommand EditLocationCommand
            {
                get => _editLocationCommand = _editLocationCommand ?? new RelayCommand(EditLocation);
            }

            RelayCommand _deleteLocationCommand;
            public RelayCommand DeleteLocationCommand
            {
                get => _deleteLocationCommand = _deleteLocationCommand ?? new RelayCommand(DeleteLocation);
            }

        void AddLocation() 
        {
            OpenEditWindow(new Location() { ID = -1 });
        }
        void EditLocation() 
        {
            if(SelectedLocation != null)
            {
                OpenEditWindow(SelectedLocation);
            }
        }
        void DeleteLocation() 
        {
            if(SelectedLocation != null)
            {
                _mainCodeBehind?.Delete(SelectedLocation);
                Locations.Remove(SelectedLocation);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Locations)));
            }
        }

        private void OpenEditWindow(Location location)
        {
            SubsidiarySearchWindow window = new SubsidiarySearchWindow() { Height = 800, Width = 800 };
            EditLocation view = new EditLocation();
            EditLocationViewModel vm = new EditLocationViewModel(location, _mainCodeBehind, UpdateItems);
            view.DataContext = vm;
            window.OutputView.Content = view;

            window.ShowDialog();
        }

        void UpdateItems(Location item)
        {
            if(item != null)
            {
                if(Locations.Contains(item) == false)
                {
                    Locations.Add(item);
                }
                else
                {
                    Locations.Remove(item);
                    Locations.Add(item);
                }
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Locations)));
            }
        }

        bool sortAscending;
        RelayCommand _sortCommand;
        public RelayCommand SortCommand
        {
            get => _sortCommand = _sortCommand ?? new RelayCommand(Sort);
        }

        void Sort()
        {
            sortAscending = !sortAscending;
            var a = System.Windows.Data.CollectionViewSource.GetDefaultView(Locations);
            a.SortDescriptions.Clear();

            if (sortAscending)
            {
                a.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            }
            else
            {
                a.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Descending));
            }
            a.Refresh();
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Locations)));
        }

        #endregion
    }
}
