using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models;

namespace Library.Query
{
    class FindStoryQuery : IExecutableQuery
    {
        public Story Story { get; set; }
        public IEnumerable<Author> Authors { get; set; }


        public IQueryable<Story> Results { get; private set; }

        public event QueryExecuted ExecuteQuery;
        public bool Execute()
        {
            if (ExecuteQuery != null)
            {
                Results = ExecuteQuery.Invoke(this) as IQueryable<Story>;
                if (Results != null)
                    return true;
            }
            return false;
        }
    }
}
