using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Library.Models
{
    public class EntityBase : INotifyPropertyChanged
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }

        //[Timestamp]
        //public byte[] Timestamp { get; set; }

        //public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        //public virtual bool NotEmpty { get; }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}