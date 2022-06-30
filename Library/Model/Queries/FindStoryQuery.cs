using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Model.LibraryEntities;

namespace Library.Query
{
    public class FindStoryQuery 
    {
        public Story Story { get; set; }
        public IEnumerable<Author> Authors { get; set; }
    }
}
