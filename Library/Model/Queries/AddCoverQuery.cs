using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models;

namespace Library.Query
{
    class AddCoverQuery : IExecutableQuery
    {
        public Cover Cover { get; set; }
        public Image Image { get; set; }

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
