namespace Library.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ISBN")]
    public partial class ISBN
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("id")]
        public int ID { get; set; }

        [Column("ISBN")]
        [Required]
        [StringLength(13)]
        public string isbn { get; set; }

        [Column("book_id")]
        public int BookID { get; set; }

        public virtual Book Book { get; set; }
    }
}
