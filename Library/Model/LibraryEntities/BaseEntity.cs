using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Library.Model.LibraryEntities
{
    class EntityBase
    {
        [Key]
        public int ID { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public event ProgressChangedEventHandler PropertyChanged;
    }
}