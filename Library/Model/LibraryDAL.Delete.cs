using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Query;

namespace Library
{
    partial class LibraryDAL
    {
        //Удалять ли рассказы, если они не содержатся в иных книгах?
        public bool DeleteBook(int id) { return false; }
        public bool DeleteAuthor(int id) { return false; }
        public bool DeleteGenre(int id) { return false; }
        public bool DeletePublisher(int id) { return false; }
        public bool DeleteBindingType(int id) { return false; }
        public bool DeleteCoverType(int id) { return false; }
        public bool DeleteStory(int id) { return false; }
    }
}
