namespace Library.Model.LibraryEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("book_genre")]
    public partial class BookGenre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int ID { get; set; }
        [Column("book_id")]
        public int BookID { get; set; }
        [Column("genre_id")]
        public int GenreID { get; set; }

        public virtual Book Book { get; set; }

        public virtual Genre Genre { get; set; }
    }
}
