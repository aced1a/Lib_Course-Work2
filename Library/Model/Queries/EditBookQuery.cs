using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Library.Model.LibraryEntities;

namespace Library.Query
{
    public class EditBookQuery
    {
        public Book Book { get; set; }
 
        public List<int> DeletedAuthors { get; set; }
        public List<int> AddedAuthors { get; set; }
        public List<int> AddedGenres { get; set; }
        public List<int> AddedStories { get; set; }
        public List<int> AddedPublishers { get; set; }

        public List<int> DeletedGenres { get; set; }
        public List<int> DeletedPublishers { get; set; }
        public List<int> DeletedStories { get; set; }


        public EditBookQuery() {
            AddedAuthors = new List<int>();
            DeletedAuthors = new List<int>();
            AddedGenres = new List<int>();
            AddedPublishers = new List<int>();
            AddedStories = new List<int>();
            DeletedGenres = new List<int>();
            DeletedPublishers = new List<int>();
            DeletedStories = new List<int>();
        }
    }
}
