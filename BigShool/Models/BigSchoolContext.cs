using System;
using System.Linq;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigShool.Models
{
    public partial class BigSchoolContext : DbContext
    {
        public BigSchoolContext()
            : base("name=DefaultConnection") {}
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Following> Followings { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }

        protected override void OnModelCreating(DbModelBuilder ModelBuilder)
        {
            ModelBuilder.Entity<AspNetUser>()
                .HasMany(Error => Error.Attendances)
                .WithRequired(Error => Error.AspNetUser)
                .HasForeignKey(Error => Error.Attendee)
                .WillCascadeOnDelete(false);
        }
    }
}
