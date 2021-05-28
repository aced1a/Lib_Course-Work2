namespace Library.Model.LibraryEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("book_publisher")]
    public partial class BookPublisher
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int ID { get; set; }

        [Column("book_id")]
        public int BookID { get; set; }

        [Column("publisher_id")]
        public int PublisherID { get; set; }

        public virtual Book Book { get; set; }

        public virtual Publisher Publisher { get; set; }
    }
}
