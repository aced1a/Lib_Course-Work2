using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Query
{
    interface IExecutableQuery
    {
        event QueryExecuted ExecuteQuery;
        bool Execute();
    }


    delegate object QueryExecuted(IExecutableQuery query);
}
