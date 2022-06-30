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
    class CoverTypeSearchViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        IMainWindowCodeBehind _mainCodeBehind;
        ObservableCollection<CoverType> _coverTypes;
        CoverType _selectedCoverType;
        Action<CoverType> updateSelectedCoverTypes;
        string _coverTypeName;

        public ObservableCollection<CoverType> CoverTypes
        {
            get => _coverTypes = _coverTypes ?? new ObservableCollection<CoverType>();
            set
            {
                _coverTypes = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(CoverTypes)));
            }
        }

        public CoverType SelectedCoverType
        {
            get => _selectedCoverType;
            set
            {
                _selectedCoverType = value;
                updateSelectedCoverTypes?.Invoke(_selectedCoverType);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedCoverType)));
            }
        }

        public string CoverTypeName
        {
            get => _coverTypeName;
            set
            {
                _coverTypeName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(CoverTypeName)));
            }
        }

        public CoverTypeSearchViewModel(IMainWindowCodeBehind codeBehind, Action<CoverType> action=null)
        {
            _mainCodeBehind = codeBehind;
            updateSelectedCoverTypes += action;
        }

        RelayCommand _findCoverTypesCommand;

        public RelayCommand FindCoverTypesCommand
        {
            get => _findCoverTypesCommand = _findCoverTypesCommand ?? new RelayCommand(FindCoverTypes);
        }

        private void FindCoverTypes()
        {
            CoverTypes = _mainCodeBehind?.FindCoverTypes(
                new CoverType()
                {
                    Name = CoverTypeName
                }
            );
        }

        RelayCommand _addCoverTypeCommand;
        public RelayCommand AddCoverTypeCommand
        {
            get => _addCoverTypeCommand = _addCoverTypeCommand ?? new RelayCommand(AddCoverType);
        }

        RelayCommand _chgCoverTypeCommand;
        public RelayCommand ChangeCoverTypeCommand
        {
            get => _chgCoverTypeCommand = _chgCoverTypeCommand ?? new RelayCommand(ChgCoverType);
        }

        RelayCommand _deleteCoverTypeCommand;
        public RelayCommand DeleteCoverTypeCommand
        {
            get => _deleteCoverTypeCommand = _deleteCoverTypeCommand ?? new RelayCommand(DeleteCoverType);
        }

        private void AddCoverType()
        {
            OpenEditWindow(
                new CoverType()
                {
                    ID = -1
                }
            );
        }

        private void ChgCoverType()
        {
            if (SelectedCoverType != null)
                OpenEditWindow(SelectedCoverType);
        }

        private void DeleteCoverType()
        {
            if (SelectedCoverType != null)
            {
                _mainCodeBehind?.Delete(SelectedCoverType);
                CoverTypes.Remove(SelectedCoverType);
            }
        }

        private void OpenEditWindow(CoverType type)
        {
            SubsidiarySearchWindow window = new SubsidiarySearchWindow() { Width = 600, Height = 200 };
            EditElement view = new EditElement();
            EditCoverTypeViewModel vm = new EditCoverTypeViewModel(type, _mainCodeBehind, UpdateItems);
            view.DataContext = vm;
            window.OutputView.Content = view;

            window.ShowDialog();
            //UpdateItems(vm.Item);
        }

        private void UpdateItems(CoverType item)
        {
            if (item != null && CoverTypes.Contains(item) == false)
            {
                CoverTypes.Add(item);
            }
            else
            {
                CoverTypes.Remove(item);
                CoverTypes.Add(item);
            }
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(CoverTypes)));
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
            var a = System.Windows.Data.CollectionViewSource.GetDefaultView(CoverTypes);
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
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(CoverTypes)));
        }

        RelayCommand _exportToExcelCommand;
        public RelayCommand ExportToExcelCommand
        {
            get => _exportToExcelCommand = _exportToExcelCommand ?? new RelayCommand(ExportToExcel);
        }

        void ExportToExcel()
        {
            if (CoverTypes.Count > 0)
            {
                var export = new ExcelExporter();
                export.ExportToExcel(CoverTypes);
            }
        }
    }
}
