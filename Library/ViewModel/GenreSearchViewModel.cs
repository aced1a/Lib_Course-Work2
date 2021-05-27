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
    class GenreSearchViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        IMainWindowCodeBehind _mainCodeBehind;
        ObservableCollection<Genre> _genres;
        public ObservableCollection<Genre> Genres
        {
            get
            {
                return _genres = _genres ?? new ObservableCollection<Genre>();
            }
            private set
            {
                _genres = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Genres)));
            }
        }

        Action<Genre> updateSelectedGenres;

        private string _genreName;
        public string GenreName
        {
            get => _genreName;
            set
            {
                _genreName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(GenreName)));
            }
        }

        private Genre _selectedGenre;
        public Genre SelectedGenre
        {
            get => _selectedGenre;
            set
            {
                _selectedGenre = value;
                updateSelectedGenres?.Invoke(_selectedGenre);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedGenre)));
            }
        }

        public GenreSearchViewModel(IMainWindowCodeBehind codeBehind, Action<Genre> action=null) 
        {
            _mainCodeBehind = codeBehind;
            updateSelectedGenres += action;
        }


        private RelayCommand _findGenresCommand;
        public RelayCommand FindGenresCommand
        {
            get => _findGenresCommand = _findGenresCommand ?? new RelayCommand(FindGenres);
        }

        private void FindGenres()
        {
            Genres = _mainCodeBehind?.FindGenres(
                new Genre() { 
                    ID = -1,
                    Name = GenreName
                }
            );
        }

        RelayCommand _addGenreCommand;
        public RelayCommand AddGenreCommand
        {
            get => _addGenreCommand = _addGenreCommand ?? new RelayCommand(AddGenre);
        }

        RelayCommand _chgAuthorCommand;
        public RelayCommand ChangeGenreCommand
        {
            get => _chgAuthorCommand = _chgAuthorCommand ?? new RelayCommand(ChgGenre);
        }

        RelayCommand _deleteGenreCommand;
        public RelayCommand DeleteGenreCommand
        {
            get => _deleteGenreCommand = _deleteGenreCommand ?? new RelayCommand(DeleteGenre);
        }

        private void AddGenre()
        {
            OpenEditWindow(
                new Genre()
                {
                    ID = -1
                }
            );
        }

        private void ChgGenre()
        {
            if (SelectedGenre != null)
                OpenEditWindow(SelectedGenre);
        }

        private void DeleteGenre()
        {
            if (SelectedGenre != null)
            {
                _mainCodeBehind?.Delete(SelectedGenre);
                Genres.Remove(SelectedGenre);
            }
        }

        private void OpenEditWindow(Genre genre)
        {
            SubsidiarySearchWindow window = new SubsidiarySearchWindow();
            EditElement view = new EditElement();
            EditGenreViewModel vm = new EditGenreViewModel(genre, _mainCodeBehind, UpdateItems);
            view.DataContext = vm;
            window.OutputView.Content = view;

            window.ShowDialog();
            //UpdateItems(vm.Item);
        }

        private void UpdateItems(Genre item)
        {
            if (item != null && Genres.Contains(item) == false)
            {
                Genres.Add(item);
            }
            else
            {
                Genres.Remove(item);
                Genres.Add(item);
            }
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Genres)));
        }
    }
}
