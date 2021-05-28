namespace Library.Model.LibraryEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("story_author")]
    public partial class StoryAuthor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int ID { get; set; }

        [Column("story_id")]
        public int StoryID { get; set; }

        [Column("author_id")]
        public int AuthorID { get; set; }

        public virtual Author Author { get; set; }

        public virtual Story Story { get; set; }
    }
}
