using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace usm.Models;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ClassEnrollment> ClassEnrollments { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SessionTime> SessionTimes { get; set; }

    public virtual DbSet<TeacherPerCourse> TeacherPerCourses { get; set; }

    public virtual DbSet<TeacherPerCoursePerSessionTime> TeacherPerCoursePerSessionTimes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClassEnrollment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("classenrollment_pk");

            entity.ToTable("ClassEnrollment");

            entity.HasIndex(e => e.Id, "classenrollment_id_uindex").IsUnique();

            entity.Property(e => e.ClassId).ValueGeneratedOnAdd();
            entity.Property(e => e.StudentId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Class).WithMany(p => p.ClassEnrollments)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("classenrollment_class_id_fk");

            entity.HasOne(d => d.Student).WithMany(p => p.ClassEnrollments)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("classenrollment_users_id_fk");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("courses_pk");

            entity.HasIndex(e => e.Name, "courses_\"name\"_uindex").IsUnique();

            entity.HasIndex(e => e.Id, "courses_id_uindex").IsUnique();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pk");

            entity.HasIndex(e => e.Id, "roles_\"id\"_uindex").IsUnique();

            entity.HasIndex(e => e.Name, "roles_\"name\"_uindex").IsUnique();
        });

        modelBuilder.Entity<SessionTime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sessiontime_pk");

            entity.ToTable("SessionTime");

            entity.HasIndex(e => e.Id, "sessiontime_id_uindex").IsUnique();

            entity.Property(e => e.EndTime).HasColumnType("timestamp without time zone");
            entity.Property(e => e.StartTime).HasColumnType("timestamp without time zone");
        });

        modelBuilder.Entity<TeacherPerCourse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("class_pk");

            entity.ToTable("TeacherPerCourse");

            entity.HasIndex(e => e.Id, "class_id_uindex").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"Class_Id_seq\"'::regclass)");
            entity.Property(e => e.CourseId).HasDefaultValueSql("nextval('\"Class_CourseId_seq\"'::regclass)");
            entity.Property(e => e.TeacherId).HasDefaultValueSql("nextval('\"Class_TeacherId_seq\"'::regclass)");

            entity.HasOne(d => d.Course).WithMany(p => p.TeacherPerCourses)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("class_course_id_fk");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TeacherPerCourses)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("class_teacher_id_fk");
        });

        modelBuilder.Entity<TeacherPerCoursePerSessionTime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("classsessions_pk");

            entity.ToTable("TeacherPerCoursePerSessionTime");

            entity.HasIndex(e => e.Id, "classsessions_id_uindex").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"ClassSessions_Id_seq\"'::regclass)");
            entity.Property(e => e.SessionTimeId).HasDefaultValueSql("nextval('\"ClassSessions_SessionTimeId_seq\"'::regclass)");
            entity.Property(e => e.TeacherPerCourseId).HasDefaultValueSql("nextval('\"ClassSessions_ClassId_seq\"'::regclass)");

            entity.HasOne(d => d.SessionTime).WithMany(p => p.TeacherPerCoursePerSessionTimes)
                .HasForeignKey(d => d.SessionTimeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("classsessions_sessiontime_id_fk");

            entity.HasOne(d => d.TeacherPerCourse).WithMany(p => p.TeacherPerCoursePerSessionTimes)
                .HasForeignKey(d => d.TeacherPerCourseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("classsessions_class_id_fk");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pk");

            entity.HasIndex(e => e.Email, "users_\"email\"_uindex").IsUnique();

            entity.HasIndex(e => e.Id, "users_\"id\"_uindex").IsUnique();

            entity.HasIndex(e => e.KeycloakId, "users_\"keycloackid\"_uindex").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"Users_id_seq\"'::regclass)");
            entity.Property(e => e.Name).HasColumnType("character varying");
            entity.Property(e => e.RoleId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("users_role_id_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
