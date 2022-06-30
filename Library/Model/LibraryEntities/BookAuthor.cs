namespace Library.Model.LibraryEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("book_author")]
    public partial class BookAuthor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int ID { get; set; }

        [Column("book_id")]
        public int BookID { get; set; }

        [Column("author_id")]
        public int AuthorID { get; set; }

    

        public virtual Author Author { get; set; }

        public virtual Book Book { get; set; }
    }
}
