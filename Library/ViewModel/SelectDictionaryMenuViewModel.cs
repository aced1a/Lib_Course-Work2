using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ViewModel
{
    class SelectDictionaryMenuViewModel
    {
        Action<ViewType, SearchViewType> chgView;
        Action<PanelType> chgPanel;

        public SelectDictionaryMenuViewModel(Action<ViewType, SearchViewType> action, Action<PanelType> change) 
        {
            chgView += action;
            chgPanel += change;
        }

        RelayCommand _onLoadBooks;
        public RelayCommand OnLoadBooksCommand
        {
            get => _onLoadBooks = _onLoadBooks ?? new RelayCommand(OnLoadBooks);
        }

        RelayCommand _onLoadAuthors;
        public RelayCommand OnLoadAuthorsCommand
        {
            get => _onLoadAuthors = _onLoadAuthors ?? new RelayCommand(OnLoadAuthors);
        }

        RelayCommand _onLoadGenres;
        public RelayCommand OnLoadGenresCommand
        {
            get => _onLoadGenres = _onLoadGenres ?? new RelayCommand(OnLoadGenres);
        }

        RelayCommand _onLoadPublishers;
        public RelayCommand OnLoadPublishersCommand
        {
            get => _onLoadPublishers = _onLoadPublishers ?? new RelayCommand(OnLoadPublishers);
        }

        RelayCommand _onLoadPreviusPage;
        public RelayCommand OnLoadPreviusPageCommand
        {
            get => _onLoadPreviusPage = _onLoadPreviusPage ?? new RelayCommand(OnLoadPreviusPage);
        }

        RelayCommand _onLoadNextPage;
        public RelayCommand OnLoadNextPageCommand
        {
            get => _onLoadNextPage = _onLoadNextPage ?? new RelayCommand(OnLoadNextPage);
        }

        RelayCommand _onLoadStories;
        public RelayCommand OnLoadStoriesCommand
        {
            get => _onLoadStories = _onLoadStories ?? new RelayCommand(OnLoadStories);
        }

        RelayCommand _onLoadBindingTypes;
        public RelayCommand OnLoadBindingTypesCommand
        {
            get => _onLoadBindingTypes = _onLoadBindingTypes ?? new RelayCommand(OnLoadBindingTypes);
        }

        RelayCommand _onLoadCoverTypes;
        public RelayCommand OnLoadCoverTypesCommand
        {
            get => _onLoadCoverTypes = _onLoadCoverTypes ?? new RelayCommand(OnLoadCoverTypes);
        }

        RelayCommand _onLoadSettings;
        public RelayCommand OnLoadSettingsCommand
        {
            get => _onLoadStories = _onLoadStories ?? new RelayCommand(OnLoadSettings);
        }

        private void OnLoadBooks() => chgView?.Invoke(ViewType.SearchMode, SearchViewType.BooksSearch);

        private void OnLoadAuthors() => chgView?.Invoke(ViewType.SearchMode, SearchViewType.AuthorsSearch);

        private void OnLoadGenres() => chgView?.Invoke(ViewType.SearchMode, SearchViewType.GenresSearch);

        private void OnLoadPublishers() => chgView?.Invoke(ViewType.SearchMode, SearchViewType.PublishersSearch);

        private void OnLoadPreviusPage() => chgPanel?.Invoke(PanelType.FirstPanel);
        private void OnLoadNextPage() => chgPanel?.Invoke(PanelType.SecondPanel);

        private void OnLoadStories() => chgView?.Invoke(ViewType.SearchMode, SearchViewType.StoriesSearch);
        private void OnLoadBindingTypes() => chgView?.Invoke(ViewType.SearchMode, SearchViewType.BindingTypesSearch);
        private void OnLoadCoverTypes() => chgView?.Invoke(ViewType.SearchMode, SearchViewType.CoverTypesSearch);

        private void OnLoadSettings() => chgView?.Invoke(ViewType.SearchMode, SearchViewType.AuthorsSearch);
    }
}
