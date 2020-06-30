using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BlazorExam.Data
{
    public partial class studentsContext : DbContext
    {
        public void ExcludeStudent(Student student)
        {
            Students.Remove(student);
            SaveChanges();
        }

        public bool IncludeStudent(Student student)
        {
            if (
                string.IsNullOrEmpty(student.Name) || 
                string.IsNullOrEmpty(student.Secondname) ||
                // Not 8-length number
                student.GradebookNumber.Length != 8 ||
                // Or it's not even a number
                !int.TryParse(student.GradebookNumber, out _) ||
                // Or unique key constraint violated
                Students.Any(existed => existed.GradebookNumber == student.GradebookNumber)
                )
                return false;
            
            Students.Add(student);
            SaveChanges();
            return true;
        }
    }
    public partial class studentsContext : DbContext
    {
        public studentsContext(DbContextOptions<studentsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=students;Username=postgres;Password=2233;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("groups");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasViewColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasViewColumnName("name");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(x => x.GradebookNumber)
                    .HasName("students_pk");

                entity.ToTable("students");

                entity.Property(e => e.GradebookNumber)
                    .HasColumnName("gradebook_number")
                    .HasViewColumnName("gradebook_number");

                entity.Property(e => e.IdGroup)
                    .HasColumnName("id_group")
                    .HasViewColumnName("id_group");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasViewColumnName("name");

                entity.Property(e => e.Secondname)
                    .IsRequired()
                    .HasColumnName("secondname")
                    .HasViewColumnName("secondname");

                entity.Property(e => e.Thirdname)
                    .HasColumnName("thirdname")
                    .HasViewColumnName("thirdname");

                entity.HasOne(d => d.IdGroupNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(x => x.IdGroup)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("students_fk");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
