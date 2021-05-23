namespace Library.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("book")]
    public partial class Book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Book()
        {
            BookAuthor = new HashSet<BookAuthor>();
            BookGenre = new HashSet<BookGenre>();
            ISBN = new HashSet<ISBN>();
            BookPublisher = new HashSet<BookPublisher>();
            BookStory = new HashSet<BookStory>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("id")]
        public int ID { get; set; }

        [Required]
        [StringLength(50), Column("title")]
        public string Title { get; set; }

        [Column("year")]
        public int? Year { get; set; }

        [Column("description",TypeName = "text")]
        public string Description { get; set; }

        [Column("note",TypeName = "text")]
        public string Note { get; set; }

        [Column("location_id")]
        public int? LocationID { get; set; }

        [Column("cover_id")]
        public int? CoverID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookAuthor> BookAuthor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookGenre> BookGenre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ISBN> ISBN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookPublisher> BookPublisher { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookStory> BookStory { get; set; }

        public virtual Cover Cover { get; set; }

        public virtual Location Location { get; set; }
    }
}
