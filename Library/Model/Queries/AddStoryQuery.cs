using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Model.LibraryEntities;

namespace Library.Query
{
    public class AddStoryQuery : IExecutableQuery
    {
        public Story Story { get; set; }
        public IEnumerable<Author> Authors { get; set; }


        public event QueryExecuted ExecuteQuery;
        public bool Execute()
        {
            object res = null;
            if (ExecuteQuery != null)
                if ((res = ExecuteQuery.Invoke(this)) as bool? != null) { return (bool)res; }
            return false;
        }
    }

}