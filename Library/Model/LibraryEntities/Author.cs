namespace Library.Model.LibraryEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("author")]
    public partial class Author
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Author()
        {
            AuthorBook = new HashSet<BookAuthor>();
            AuthorStory = new HashSet<StoryAuthor>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int ID { get; set; }

       
        [StringLength(50), Column("first_name")]
        public string FirstName { get; set; }

        [StringLength(50), Column("middle_name")]
        public string MiddleName { get; set; }

        [StringLength(50), Column("last_name")]
        public string LastName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookAuthor> AuthorBook { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StoryAuthor> AuthorStory { get; set; }

        public override string ToString()
        {
            return $"{LastName} {FirstName} {MiddleName}";
        }

        [NotMapped]
        public string FullName
        {
            get => ToString();
        }

        [NotMapped]
        public string Name
        {
            get => ToString();
        }


        [NotMapped]
        public string NumberOfBooks
        {
            get => $"({AuthorBook.Count})";
        }
    }
}
