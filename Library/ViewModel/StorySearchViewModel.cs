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
        Action<Story> updateSelectedStories;
        ObservableCollection<Story> _stories;
        ObservableCollection<Author> _selectedAuthors;
        Story _selectedStory;
        private string _storyName;


        public ObservableCollection<Story> Stories
        {
            get => _stories = _stories ?? new ObservableCollection<Story>();
            set
            {
                _stories = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Stories)));
            }
        }
        
        public Story SelectedStory
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

        public StorySearchViewModel(IMainWindowCodeBehind codeBehind, Action<Story> action=null)
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
    }
}
