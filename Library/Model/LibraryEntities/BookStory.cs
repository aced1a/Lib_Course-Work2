namespace Library.Model.LibraryEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("book_story")]
    public partial class BookStory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int ID { get; set; }

        [Column("book_id")]
        public int BookID { get; set; }

        [Column("story_id")]
        public int StoryID { get; set; }

        public virtual Book Book { get; set; }

        public virtual Story Story { get; set; }
    }
}
