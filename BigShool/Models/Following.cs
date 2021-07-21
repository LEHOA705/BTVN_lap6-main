namespace BigShool.Models
{
    using System;
    using System.Data.Entity.Spatial;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Following")]
    public partial class Following
    {
        [Key]
        [Column(Order = 0)]
        public string FollowerId { get; set; }

        [Key]
        [Column(Order = 1)]
        public string FolloweeId { get; set; }
    }
}
