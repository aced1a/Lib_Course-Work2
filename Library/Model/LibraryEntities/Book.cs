namespace Library.Model.LibraryEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Text;

    [Table("book")]
    public partial class Book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Book()
        {
            BookAuthor = new HashSet<BookAuthor>();
            BookGenre = new HashSet<BookGenre>();
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

        [StringLength(13), Column("isbn")]
        public string ISBN { get; set; }

        [Column("description", TypeName = "text")]
        public string Description { get; set; }

        [Column("note", TypeName = "text")]
        public string Note { get; set; }

        [Column("location_id")]
        public int? LocationID { get; set; }

        [Column("cover_id")]
        public int? CoverTypeID { get; set; }

        [Column("binding_id")]
        public int? BindingTypeID { get; set; }

        [Column("image", TypeName = "image")]
        public byte[] Image { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookAuthor> BookAuthor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookGenre> BookGenre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookPublisher> BookPublisher { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookStory> BookStory { get; set; }
      
        public virtual BindingType BindingType { get; set; }
        public virtual CoverType CoverType { get; set; }
        public virtual Location Location { get; set; }

        
        [NotMapped]
        public string Authors
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var author in BookAuthor)
                {
                    sb.Append($"{author.Author.FullName} ");
                }
                return sb.ToString();
            }
        }
        [NotMapped]
        public string Genres
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in BookGenre)
                {
                    sb.Append($"{item.Genre.Name} ");
                }
                return sb.ToString();
            }
        }

        [NotMapped]
        public string Publishers
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in BookPublisher)
                {
                    sb.Append($"{item.Publisher.Name} ");
                }
                return sb.ToString();
            }
        }        

        [NotMapped]
        public string Stories
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in BookStory)
                {
                    sb.Append($"{item.Story.Name} ");
                }
                return sb.ToString();
            }
        }

    }
}
