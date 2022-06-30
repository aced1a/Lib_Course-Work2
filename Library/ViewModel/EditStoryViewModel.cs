using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.View;
using Library.Query;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Library.Model.LibraryEntities;

namespace Library.ViewModel
{
    class EditStoryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        IMainWindowCodeBehind _mainCodeBehind;
        Action<StoryInfo> update;
        StoryInfo _story;
        EditStoryQuery query;
        

        public StoryInfo Story
        {
            get => _story = _story ?? new StoryInfo();
            set
            {
                _story = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Story)));
            }
        }

        public EditStoryViewModel(StoryInfo story, IMainWindowCodeBehind codeBehind, Action<StoryInfo> action = null)
        {
            Story = story;
            _mainCodeBehind = codeBehind;
            update += action;
            query = new EditStoryQuery();
        }

        RelayCommand _saveChangesCommand;
        public RelayCommand SaveChangesCommand
        {
            get => _saveChangesCommand = _saveChangesCommand ?? new RelayCommand(SaveChanges);
        }

        private void SaveChanges()
        {
            if (Story.Story.ID != -1)
            {
                query.Story = Story.Story;
                _mainCodeBehind?.EditStory(query);
                update?.Invoke(Story);

            }
            else
            {
                AddStory();
                update?.Invoke(Story);
                Story = new StoryInfo() { Story = new Story { ID = -1 } };
                query = new EditStoryQuery();
            }
        }

        private void AddStory()
        {
            _mainCodeBehind?.Add<AddStoryQuery>(
                new AddStoryQuery()
                {
                    Story = this.Story.Story,
                    Authors = this.Story.Authors,
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
            AuthorSearchViewModel vm = new AuthorSearchViewModel(_mainCodeBehind, AddAuthor);
            view.DataContext = vm;
            window.OutputView.Content = view;

            window.Show();
        }

        private void AddAuthor(Author author)
        {
            if (author != null && Story.Authors.Contains(author) == false)
            {
                Story.Authors.Add(author);
                query.AddedAuthors.Add(author.ID);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Story.Authors)));
            }
        }

        private void DeleteAuthor(object obj)
        {
            Author author = obj as Author;
            if (author != null && Story.Authors.Contains(author))
            {
               Story.Authors.Remove(author);
               query.DeletedAuthors.Add(author.ID);
               PropertyChanged(this, new PropertyChangedEventArgs(nameof(Story.Authors)));
            }
        }
    }
}
