using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ConnectEduV2.Models;

public partial class ConnectEduV1Context : DbContext
{
    public ConnectEduV1Context()
    {
    }

    public ConnectEduV1Context(DbContextOptions<ConnectEduV1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<ClassChat> ClassChats { get; set; }

    public virtual DbSet<ClassRegistration> ClassRegistrations { get; set; }

    public virtual DbSet<ClassStatus> ClassStatuses { get; set; }

    public virtual DbSet<DataStatus> DataStatuses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DepositTransaction> DepositTransactions { get; set; }

    public virtual DbSet<Evaluate> Evaluates { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<PaymentStatus> PaymentStatuses { get; set; }

    public virtual DbSet<PurchaseTransaction> PurchaseTransactions { get; set; }

    public virtual DbSet<RegistrationStatus> RegistrationStatuses { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<RevenueSharing> RevenueSharings { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<School> Schools { get; set; }

    public virtual DbSet<Semester> Semesters { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserStatus> UserStatuses { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        IConfigurationRoot configuration = builder.Build();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DBContext"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Class>(entity =>
        {
            entity.ToTable("Class");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ClassStatusId).HasColumnName("ClassStatusID");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.SubjectId).HasColumnName("SubjectID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.ClassStatus).WithMany(p => p.Classes)
                .HasForeignKey(d => d.ClassStatusId)
                .HasConstraintName("FK_Class_ClassStatus");

            entity.HasOne(d => d.Subject).WithMany(p => p.Classes)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("FK_Class_Subject");

            entity.HasOne(d => d.User).WithMany(p => p.Classes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Class_User");
        });

        modelBuilder.Entity<ClassChat>(entity =>
        {
            entity.ToTable("ClassChat");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ChatDate).HasColumnType("datetime");
            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.ClassRegistrationId).HasColumnName("ClassRegistrationID");

            entity.HasOne(d => d.Class).WithMany(p => p.ClassChats)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK_ClassChat_Class");

            entity.HasOne(d => d.ClassRegistration).WithMany(p => p.ClassChats)
                .HasForeignKey(d => d.ClassRegistrationId)
                .HasConstraintName("FK_ClassChat_ClassRegistration");
        });

        modelBuilder.Entity<ClassRegistration>(entity =>
        {
            entity.ToTable("ClassRegistration");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.PaymentStatusId).HasColumnName("PaymentStatusID");
            entity.Property(e => e.RegistrationStatusId).HasColumnName("RegistrationStatusID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Class).WithMany(p => p.ClassRegistrations)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK_ClassRegistration_Class");

            entity.HasOne(d => d.PaymentStatus).WithMany(p => p.ClassRegistrations)
                .HasForeignKey(d => d.PaymentStatusId)
                .HasConstraintName("FK_ClassRegistration_PaymentStatus");

            entity.HasOne(d => d.RegistrationStatus).WithMany(p => p.ClassRegistrations)
                .HasForeignKey(d => d.RegistrationStatusId)
                .HasConstraintName("FK_ClassRegistration_RegistrationStatus");

            entity.HasOne(d => d.User).WithMany(p => p.ClassRegistrations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassRegistration_User");
        });

        modelBuilder.Entity<ClassStatus>(entity =>
        {
            entity.ToTable("ClassStatus");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<DataStatus>(entity =>
        {
            entity.ToTable("DataStatus");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("Department");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.SchoolId).HasColumnName("SchoolID");

            entity.HasOne(d => d.DataStatusNavigation).WithMany(p => p.Departments)
                .HasForeignKey(d => d.DataStatus)
                .HasConstraintName("FK_Department_DataStatus");

            entity.HasOne(d => d.School).WithMany(p => p.Departments)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK_Department_School");
        });

        modelBuilder.Entity<DepositTransaction>(entity =>
        {
            entity.ToTable("DepositTransaction");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.PaymentStatusId).HasColumnName("PaymentStatusID");
            entity.Property(e => e.WalletId).HasColumnName("WalletID");

            entity.HasOne(d => d.PaymentStatus).WithMany(p => p.DepositTransactions)
                .HasForeignKey(d => d.PaymentStatusId)
                .HasConstraintName("FK_DepositTransaction_PaymentStatus");

            entity.HasOne(d => d.Wallet).WithMany(p => p.DepositTransactions)
                .HasForeignKey(d => d.WalletId)
                .HasConstraintName("FK_DepositTransaction_Wallet");
        });

        modelBuilder.Entity<Evaluate>(entity =>
        {
            entity.ToTable("Evaluate");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.ToTable("Feedback");

            entity.HasIndex(e => e.RegistrationId, "IX_Feedback").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.EvaluateId).HasColumnName("EvaluateID");
            entity.Property(e => e.RegistrationId).HasColumnName("RegistrationID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Class).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK_Feedback_Class");

            entity.HasOne(d => d.Evaluate).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.EvaluateId)
                .HasConstraintName("FK_Feedback_Evaluate");

            entity.HasOne(d => d.Registration).WithOne(p => p.Feedback)
                .HasForeignKey<Feedback>(d => d.RegistrationId)
                .HasConstraintName("FK_Feedback_ClassRegistration");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Feedback_User");
        });

        modelBuilder.Entity<PaymentStatus>(entity =>
        {
            entity.ToTable("PaymentStatus");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<PurchaseTransaction>(entity =>
        {
            entity.ToTable("PurchaseTransaction");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.RegistrationId).HasColumnName("RegistrationID");
            entity.Property(e => e.WalletId).HasColumnName("WalletID");

            entity.HasOne(d => d.PaymentStatusNavigation).WithMany(p => p.PurchaseTransactions)
                .HasForeignKey(d => d.PaymentStatus)
                .HasConstraintName("FK_PurchaseTransaction_PaymentStatus");

            entity.HasOne(d => d.Registration).WithMany(p => p.PurchaseTransactions)
                .HasForeignKey(d => d.RegistrationId)
                .HasConstraintName("FK_PurchaseTransaction_ClassRegistration");

            entity.HasOne(d => d.Wallet).WithMany(p => p.PurchaseTransactions)
                .HasForeignKey(d => d.WalletId)
                .HasConstraintName("FK_PurchaseTransaction_Wallet");
        });

        modelBuilder.Entity<RegistrationStatus>(entity =>
        {
            entity.ToTable("RegistrationStatus");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.ToTable("Report");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<RevenueSharing>(entity =>
        {
            entity.ToTable("RevenueSharing");

            entity.HasIndex(e => e.PurchaseId, "IX_RevenueSharing").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MentorReceived).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ProjectReceived).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.PurchaseId).HasColumnName("PurchaseID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Purchase).WithOne(p => p.RevenueSharing)
                .HasForeignKey<RevenueSharing>(d => d.PurchaseId)
                .HasConstraintName("FK_RevenueSharing_PurchaseTransaction");

            entity.HasOne(d => d.User).WithMany(p => p.RevenueSharings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_RevenueSharing_User");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<School>(entity =>
        {
            entity.ToTable("School");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DataStatusId).HasColumnName("DataStatusID");

            entity.HasOne(d => d.DataStatus).WithMany(p => p.Schools)
                .HasForeignKey(d => d.DataStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_School_DataStatus");
        });

        modelBuilder.Entity<Semester>(entity =>
        {
            entity.ToTable("Semester");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DataStatusId).HasColumnName("DataStatusID");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.SchoolId).HasColumnName("SchoolID");

            entity.HasOne(d => d.DataStatus).WithMany(p => p.Semesters)
                .HasForeignKey(d => d.DataStatusId)
                .HasConstraintName("FK_Semester_DataStatus");

            entity.HasOne(d => d.Department).WithMany(p => p.Semesters)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_Semester_Department");

            entity.HasOne(d => d.School).WithMany(p => p.Semesters)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK_Semester_School");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.ToTable("Subject");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DataStatusId).HasColumnName("DataStatusID");
            entity.Property(e => e.DerpartmentId).HasColumnName("DerpartmentID");
            entity.Property(e => e.SchoolId).HasColumnName("SchoolID");
            entity.Property(e => e.SemesterId).HasColumnName("SemesterID");

            entity.HasOne(d => d.DataStatus).WithMany(p => p.Subjects)
                .HasForeignKey(d => d.DataStatusId)
                .HasConstraintName("FK_Subject_DataStatus");

            entity.HasOne(d => d.Derpartment).WithMany(p => p.Subjects)
                .HasForeignKey(d => d.DerpartmentId)
                .HasConstraintName("FK_Subject_Department");

            entity.HasOne(d => d.School).WithMany(p => p.Subjects)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK_Subject_School");

            entity.HasOne(d => d.Semester).WithMany(p => p.Subjects)
                .HasForeignKey(d => d.SemesterId)
                .HasConstraintName("FK_Subject_Semester");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.SchoolId).HasColumnName("SchoolID");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");

            entity.HasOne(d => d.Department).WithMany(p => p.Users)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_User_Department");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_User_Role");

            entity.HasOne(d => d.School).WithMany(p => p.Users)
                .HasForeignKey(d => d.SchoolId)
                .HasConstraintName("FK_User_School");

            entity.HasOne(d => d.Status).WithMany(p => p.Users)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK_User_UserStatus");
        });

        modelBuilder.Entity<UserStatus>(entity =>
        {
            entity.ToTable("UserStatus");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.ToTable("Wallet");

            entity.HasIndex(e => e.UserId, "UserID").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithOne(p => p.Wallet)
                .HasForeignKey<Wallet>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Wallet_User1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
