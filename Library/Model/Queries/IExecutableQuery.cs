using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Query
{
    public interface IExecutableQuery
    {
        event QueryExecuted ExecuteQuery;
        bool Execute();
    }


    public delegate object QueryExecuted(IExecutableQuery query);
}
