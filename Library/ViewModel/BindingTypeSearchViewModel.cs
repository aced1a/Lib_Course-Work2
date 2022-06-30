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
    class BindingTypeSearchViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        IMainWindowCodeBehind _mainCodeBehind;
        ObservableCollection<BindingType> _bindingTypes;
        BindingType _selectedBindingType;
        Action<BindingType> updateSelectedBindingTypes;
        string _bindingTypeName;

        public ObservableCollection<BindingType> BindingTypes
        {
            get => _bindingTypes = _bindingTypes ?? new ObservableCollection<BindingType>();
            set
            {
                _bindingTypes = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(BindingTypes)));
            }
        }

        public BindingType SelectedBindingType
        {
            get => _selectedBindingType;
            set
            {
                _selectedBindingType = value;
                updateSelectedBindingTypes?.Invoke(_selectedBindingType);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedBindingType)));
            }
        }

        public string BindingTypeName
        {
            get => _bindingTypeName;
            set
            {
                _bindingTypeName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(BindingTypeName)));
            }
        }

        public BindingTypeSearchViewModel(IMainWindowCodeBehind codeBehind, Action<BindingType> action = null)
        {
            _mainCodeBehind = codeBehind;
            updateSelectedBindingTypes += action;
        }

        RelayCommand _findBindingTypesCommand;

        public RelayCommand FindBindingTypesCommand
        {
            get => _findBindingTypesCommand = _findBindingTypesCommand ?? new RelayCommand(FindBindingTypes);
        }

        private void FindBindingTypes()
        {
            BindingTypes = _mainCodeBehind?.FindBindingTypes(
                new BindingType()
                {
                    Name = BindingTypeName
                }
            );
        }

        RelayCommand _addBindingTypeCommand;
        public RelayCommand AddBindingTypeCommand
        {
            get => _addBindingTypeCommand = _addBindingTypeCommand ?? new RelayCommand(AddBindingType);
        }

        RelayCommand _chgBindingTypeCommand;
        public RelayCommand ChangeBindingTypeCommand
        {
            get => _chgBindingTypeCommand = _chgBindingTypeCommand ?? new RelayCommand(ChgBindingType);
        }

        RelayCommand _deleteBindingTypeCommand;
        public RelayCommand DeleteBindingTypeCommand
        {
            get => _deleteBindingTypeCommand = _deleteBindingTypeCommand ?? new RelayCommand(DeleteBindingType);
        }

        private void AddBindingType()
        {
            OpenEditWindow(
                new BindingType()
                {
                    ID = -1
                }
            );
        }

        private void ChgBindingType()
        {
            if (SelectedBindingType != null)
                OpenEditWindow(SelectedBindingType);
        }

        private void DeleteBindingType()
        {
            if (SelectedBindingType != null)
            {
                _mainCodeBehind?.Delete(SelectedBindingType);
                BindingTypes.Remove(SelectedBindingType);
            }
        }

        private void OpenEditWindow(BindingType type)
        {
            SubsidiarySearchWindow window = new SubsidiarySearchWindow() { Width = 600, Height = 200 };
            EditElement view = new EditElement();
            EditBindingTypeViewModel vm = new EditBindingTypeViewModel(type, _mainCodeBehind, UpdateItems);
            view.DataContext = vm;
            window.OutputView.Content = view;

            window.ShowDialog();
            //UpdateItems(vm.Item);
        }

        private void UpdateItems(BindingType item)
        {
            if (item != null && BindingTypes.Contains(item) == false)
            {
                BindingTypes.Add(item);
            }
            else
            {
                BindingTypes.Remove(item);
                BindingTypes.Add(item);
            }
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(BindingTypes)));
        }

        RelayCommand _exportToExcelCommand;
        public RelayCommand ExportToExcelCommand
        {
            get => _exportToExcelCommand = _exportToExcelCommand ?? new RelayCommand(ExportToExcel);
        }

        void ExportToExcel()
        {
            if (BindingTypes.Count > 0)
            {
                var export = new ExcelExporter();
                export.ExportToExcel(BindingTypes);
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
            var a = System.Windows.Data.CollectionViewSource.GetDefaultView(BindingTypes);
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
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(BindingTypes)));
        }
    }
}
