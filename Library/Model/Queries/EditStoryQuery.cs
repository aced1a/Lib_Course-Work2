using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Library.Model.LibraryEntities;

namespace Library.Query
{
    public class EditStoryQuery
    {
        public Story Story { get; set; }
        public List<int> DeletedAuthors { get; set; }
        public List<int> AddedAuthors { get; set; }

        public EditStoryQuery()
        {
            AddedAuthors = new List<int>();
            DeletedAuthors = new List<int>();
        }
    }
}
