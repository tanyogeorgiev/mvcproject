namespace PizzaHub.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required]
        public string Text { get; set; }

        
        public int PostID { get; set; }

        [StringLength(128)]
        public string AuthorID { get; set; }

        [Required]
        [StringLength(100)]
        public string AuthorName { get; set; }

        public DateTime? Date { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        [ForeignKey("PostID")]
        public Pizza Pizza { get; set; }
    }
}
