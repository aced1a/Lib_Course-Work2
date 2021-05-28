using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Model.LibraryEntities;

namespace Library.Query
{
    public class FindBookQuery : IExecutableQuery
    {
        public Book Book { get; set; }

        public IEnumerable<Author> Authors { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        public IEnumerable<Story> Stories { get; set; }
        public IEnumerable<Publisher> Publishers { get; set; }

        public Location Location { get; set; }

        public IQueryable<Book> Results { get; private set; }

        public int? BeginYear { get; set; }
        public int? EndYear { get; set; }

        public event QueryExecuted ExecuteQuery;
        
        public bool Execute()
        {
            if (ExecuteQuery != null)
            {
                Results = ExecuteQuery.Invoke(this) as IQueryable<Book>;
                if (Results != null)
                    return true;
            }
            return false;
        }
    }
}
