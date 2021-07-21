namespace BigShool.Models
{
    using System;
    using System.Data.Entity.Spatial;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Course")]
    public partial class Course
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [NotMapped]
        public bool IsLogin = false;

        [NotMapped]
        public bool IsGoing = false;

        [NotMapped]
        public bool IsFollowing = false;

        [Key]
        public int Id { get; set; }

        [Required]
        [Editable(false)]
        [StringLength(250)]
        public string LecturerId { get; set; }

        [Required]
        [StringLength(250)]
        public string Place { get; set; }

        public DateTime DateTime { get; set; }

        public int CategoryId { get; set; }

        [NotMapped]
        [Editable(false)]
        public string Name { set; get; }

        [NotMapped]
        [Editable(false)]
        public string CategoryName { set; get; }

        public List<Category> ListCategory = new List<Category>();
    }
}
