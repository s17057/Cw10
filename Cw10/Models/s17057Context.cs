using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Cw10.Models
{
    public partial class s17057Context : DbContext
    {
        public s17057Context()
        {
        }

        public s17057Context(DbContextOptions<s17057Context> options)
            : base(options)
        {
        }
        public virtual DbSet<Enrollment> Enrollment { get; set; }
        public virtual DbSet<StudentApbd> StudentApbd { get; set; }
        public virtual DbSet<Studies> Studies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=db-mssql.pjwstk.edu.pl;Initial Catalog=s17057;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => e.IdEnrollment)
                    .HasName("PK__ENROLLME__5EBB800FE61675DF");

                entity.ToTable("ENROLLMENT");

                entity.Property(e => e.IdEnrollment).ValueGeneratedNever();

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.IdStudyNavigation)
                    .WithMany(p => p.Enrollment)
                    .HasForeignKey(d => d.IdStudy)
                    .HasConstraintName("FK__ENROLLMEN__IdStu__65370702");
            });

            modelBuilder.Entity<StudentApbd>(entity =>
            {
                entity.HasKey(e => e.IndexNumber)
                    .HasName("PK__StudentA__98DAB2EB31B2513F");

                entity.ToTable("StudentAPBD");

                entity.Property(e => e.IndexNumber).HasMaxLength(100);

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.LastName).HasMaxLength(100);

                entity.Property(e => e.Pswd)
                    .HasColumnName("pswd")
                    .HasMaxLength(50);

                entity.Property(e => e.RefreshToken)
                    .HasColumnName("refreshToken")
                    .HasMaxLength(100);

                entity.Property(e => e.Salt)
                    .HasColumnName("salt")
                    .HasMaxLength(32);

                entity.HasOne(d => d.IdEnrollmentNavigation)
                    .WithMany(p => p.StudentApbd)
                    .HasForeignKey(d => d.IdEnrollment)
                    .HasConstraintName("FK__StudentAP__IdEnr__681373AD");
            });


            modelBuilder.Entity<Studies>(entity =>
            {
                entity.HasKey(e => e.IdStudy)
                    .HasName("PK__Studies__2B1257D3F758E4A1");

                entity.Property(e => e.IdStudy).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
