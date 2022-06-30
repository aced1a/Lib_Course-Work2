using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Library.Model.LibraryEntities;
using Library.Query;
using Library.View;

namespace Library.ViewModel
{
    class StorySearchViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        IMainWindowCodeBehind _mainCodeBehind;
        Action<StoryInfo> updateSelectedStories;
        ObservableCollection<StoryInfo> _stories;
        ObservableCollection<Author> _selectedAuthors;
        StoryInfo _selectedStory;
        private string _storyName;


        public ObservableCollection<StoryInfo> Stories
        {
            get => _stories = _stories ?? new ObservableCollection<StoryInfo>();
            set
            {
                _stories = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Stories)));
            }
        }
        
        public StoryInfo SelectedStory
        {
            get => _selectedStory;
            set
            {
                _selectedStory = value;
                updateSelectedStories?.Invoke(_selectedStory);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedStory)));
            }
        }

        public ObservableCollection<Author> SelectedAuthors
        {
            get => _selectedAuthors = _selectedAuthors ?? new ObservableCollection<Author>();
            set
            {
                _selectedAuthors = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedAuthors)));
            }
        }

        public string StoryName
        {
            get => _storyName;
            set
            {
                _storyName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(StoryName)));
            }
        }

        public StorySearchViewModel(IMainWindowCodeBehind codeBehind, Action<StoryInfo> action=null)
        {
            _mainCodeBehind = codeBehind;
            updateSelectedStories += action;
        }

        RelayCommand _findStoriesCommand;
        public RelayCommand FindStoriesCommand
        {
            get => _findStoriesCommand = _findStoriesCommand ?? new RelayCommand(FindStories);
        }

        private void FindStories()
        {
            Stories = _mainCodeBehind.FindStories(
                    new FindStoryQuery()
                    {
                        Story = new Story()
                        {
                            ID = -1,
                            Title = StoryName
                        },
                        Authors = SelectedAuthors?.Count == 0 ? null : SelectedAuthors
                    }
                );
        }



        #region copypaste

        RelayCommand _addStoryCommand;
        public RelayCommand AddStoryCommand
        {
            get => _addStoryCommand = _addStoryCommand ?? new RelayCommand(AddStory);
        }

        RelayCommand _editStoryCommand;
        public RelayCommand EditStoryCommand
        {
            get => _editStoryCommand = _editStoryCommand ?? new RelayCommand(EditStory);
        }

        RelayCommand _deleteStoryCommand;
        public RelayCommand DeleteStoryCommand
        {
            get => _deleteStoryCommand = _deleteStoryCommand ?? new RelayCommand(DeleteStory);
        }

        private void AddStory()
        {
            OpenEditWindow(new StoryInfo());
        }

        private void EditStory()
        {
            if (SelectedStory != null)
            {
                OpenEditWindow(SelectedStory);
            }
        }

        private void DeleteStory()
        {
            if (SelectedStory != null)
            {
                _mainCodeBehind?.Delete(SelectedStory.Story);
                Stories.Remove(SelectedStory);
                SelectedStory = null;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Stories)));
            }
        }

        private void OpenEditWindow(StoryInfo story)
        {
            SubsidiarySearchWindow window = new SubsidiarySearchWindow() { Height = 200, Width = 800 };
            EditStory view = new EditStory();
            EditStoryViewModel vm = new EditStoryViewModel(story, _mainCodeBehind, UpdateItems);
            view.DataContext = vm;
            window.OutputView.Content = view;

            window.ShowDialog();
        }

        private void UpdateItems(StoryInfo Story)
        {
            if (Story != null)
            {
                if (Stories.Contains(Story) == false)
                {
                    Stories.Add(Story);
                }
                else
                {
                    Stories.Remove(Story);
                    Stories.Add(Story);
                }
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Stories)));
            }
        }

        RelayCommand _openAuthorsSearchCommand;
        public RelayCommand OpenAuthorsSearchCommand
        {
            get => _openAuthorsSearchCommand = _openAuthorsSearchCommand ?? new RelayCommand(OpenAuthorsSearch);
        }

        RelayCommand<object> _deleteAuthorCommand;
        public RelayCommand<object> DeleteAuthorCommand
        {
            get => _deleteAuthorCommand = _deleteAuthorCommand ?? new RelayCommand<object>(DeleteAuthor);
        }


        private void OpenAuthorsSearch()
        {
            SubsidiarySearchWindow window = new SubsidiarySearchWindow();
            AuthorSearch view = new AuthorSearch();
            AuthorSearchViewModel vm = new AuthorSearchViewModel(_mainCodeBehind, AddSelectedAuthor);
            view.DataContext = vm;
            window.OutputView.Content = view;

            window.Show();
        }

        private void AddSelectedAuthor(Author author)
        {
            AddAuthor(author);
        }

        private void AddAuthor(Author author)
        {
            if (SelectedAuthors.Contains(author) == false)
            {
                SelectedAuthors.Add(author);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedAuthors)));
            }
        }

        private void DeleteAuthor(object obj)
        {
            Author author = obj as Author;
            if (author != null && SelectedAuthors.Contains(author))
            {
                SelectedAuthors.Remove(author);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(SelectedAuthors)));
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
            var a = System.Windows.Data.CollectionViewSource.GetDefaultView(Stories);
            a.SortDescriptions.Clear();

            if (sortAscending)
            {
                a.SortDescriptions.Add(new SortDescription("Story.Title", ListSortDirection.Ascending));
            }
            else
            {
                a.SortDescriptions.Add(new SortDescription("Story.Title", ListSortDirection.Descending));
            }
            a.Refresh();
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(Stories)));
        }

        RelayCommand _exportToExcelCommand;
        public RelayCommand ExportToExcelCommand
        {
            get => _exportToExcelCommand = _exportToExcelCommand ?? new RelayCommand(ExportToExcel);
        }

        void ExportToExcel()
        {
            if (Stories.Count > 0)
            {
                var export = new ExcelExporter();
                export.ExportToExcel(Stories);
            }
        }

        #endregion copypaste
    }
}
