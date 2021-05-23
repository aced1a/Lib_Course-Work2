namespace Library.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("cover")]
    public partial class Cover
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cover()
        {
            Book = new HashSet<Book>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("id")]
        public int ID { get; set; }

        [Column("cover_id")]
        public int? CoverID { get; set; }

        [Column("binding_id")]
        public int? BindingID { get; set; }

        [Column("image_id")]
        public int? ImageID { get; set; }

        public virtual BindingType BindingType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Book> Book { get; set; }

        public virtual CoverType CoverType { get; set; }

        public virtual Image Image { get; set; }
    }
}
