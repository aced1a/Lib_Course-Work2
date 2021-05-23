namespace Library.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("image")]
    public partial class Image
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Image()
        {
            Cover = new HashSet<Cover>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("id")]
        public int ID { get; set; }

        [Column("screen",TypeName = "image")]
        [Required]
        public byte[] Screen { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cover> Cover { get; set; }
    }
}
