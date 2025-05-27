using System;
using System.Collections.Generic;
using CredWiseCustomer.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CredWiseCustomer.Infrastructure.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DecisionAppLog> DecisionAppLogs { get; set; }

    public virtual DbSet<Fdapplication> Fdapplications { get; set; }

    public virtual DbSet<Fdtransaction> Fdtransactions { get; set; }

    public virtual DbSet<Fdtype> Fdtypes { get; set; }

    public virtual DbSet<GoldLoanDetail> GoldLoanDetails { get; set; }

    public virtual DbSet<HomeLoanDetail> HomeLoanDetails { get; set; }

    public virtual DbSet<LoanApplication> LoanApplications { get; set; }

    public virtual DbSet<LoanBankStatement> LoanBankStatements { get; set; }

    public virtual DbSet<LoanEnquiry> LoanEnquiries { get; set; }

    public virtual DbSet<LoanProduct> LoanProducts { get; set; }

    public virtual DbSet<LoanProductDocument> LoanProductDocuments { get; set; }

    public virtual DbSet<LoanRepaymentSchedule> LoanRepaymentSchedules { get; set; }

    public virtual DbSet<PaymentTransaction> PaymentTransactions { get; set; }

    public virtual DbSet<PersonalLoanDetail> PersonalLoanDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DecisionAppLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Decision__5E5486481A38EF74");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ProcessedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.LoanApplication).WithMany(p => p.DecisionAppLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DecisionA__LoanA__0A9D95DB");
        });

        modelBuilder.Entity<Fdapplication>(entity =>
        {
            entity.HasKey(e => e.FdapplicationId).HasName("PK__FDApplic__5A2486C0BE43A7EA");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Fdtype).WithMany(p => p.Fdapplications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FDApplica__FDTyp__7D439ABD");

            entity.HasOne(d => d.User).WithMany(p => p.Fdapplications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FDApplica__UserI__7C4F7684");
        });

        modelBuilder.Entity<Fdtransaction>(entity =>
        {
            entity.HasKey(e => e.FdtransactionId).HasName("PK__FDTransa__76CF381FB3F3BBF4");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.TransactionDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Fdapplication).WithMany(p => p.Fdtransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FDTransac__FDApp__04E4BC85");
        });

        modelBuilder.Entity<Fdtype>(entity =>
        {
            entity.HasKey(e => e.FdtypeId).HasName("PK__FDTypes__FFD0291BCA507D6E");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<GoldLoanDetail>(entity =>
        {
            entity.HasKey(e => e.LoanProductId).HasName("PK__GoldLoan__0D22CCC24C8D2942");

            entity.Property(e => e.LoanProductId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.LoanProduct).WithOne(p => p.GoldLoanDetail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GoldLoanD__LoanP__4F7CD00D");
        });

        modelBuilder.Entity<HomeLoanDetail>(entity =>
        {
            entity.HasKey(e => e.LoanProductId).HasName("PK__HomeLoan__0D22CCC20F178757");

            entity.Property(e => e.LoanProductId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.LoanProduct).WithOne(p => p.HomeLoanDetail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HomeLoanD__LoanP__45F365D3");
        });

        modelBuilder.Entity<LoanApplication>(entity =>
        {
            entity.HasKey(e => e.LoanApplicationId).HasName("PK__LoanAppl__F60027BD0C29866F");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.LoanProduct).WithMany(p => p.LoanApplications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LoanAppli__LoanP__5CD6CB2B");

            entity.HasOne(d => d.User).WithMany(p => p.LoanApplications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LoanAppli__UserI__5BE2A6F2");
        });

        modelBuilder.Entity<LoanBankStatement>(entity =>
        {
            entity.HasKey(e => e.BankStatementId).HasName("PK__LoanBank__D4AD9FA43B4B18D8");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Status).HasDefaultValue("Pending");

            entity.HasOne(d => d.LoanApplication).WithMany(p => p.LoanBankStatements)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LoanBankS__LoanA__6383C8BA");

            entity.HasOne(d => d.VerifiedByNavigation).WithMany(p => p.LoanBankStatements).HasConstraintName("FK__LoanBankS__Verif__6477ECF3");
        });

        modelBuilder.Entity<LoanEnquiry>(entity =>
        {
            entity.HasKey(e => e.EnquiryId).HasName("PK__LoanEnqu__0A019B7D858FC94C");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<LoanProduct>(entity =>
        {
            entity.HasKey(e => e.LoanProductId).HasName("PK__LoanProd__0D22CCC2A00DD44A");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<LoanProductDocument>(entity =>
        {
            entity.HasKey(e => e.LoanProductDocumentId).HasName("PK__LoanProd__D85B694F3B716D93");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.LoanProduct).WithMany(p => p.LoanProductDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LoanProdu__LoanP__5441852A");
        });

        modelBuilder.Entity<LoanRepaymentSchedule>(entity =>
        {
            entity.HasKey(e => e.RepaymentId).HasName("PK__LoanRepa__10AD21F2F3D7760E");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Status).HasDefaultValue("Pending");

            entity.HasOne(d => d.LoanApplication).WithMany(p => p.LoanRepaymentSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LoanRepay__LoanA__6B24EA82");
        });

        modelBuilder.Entity<PaymentTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__PaymentT__55433A6BC0272BFC");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PaymentDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.LoanApplication).WithMany(p => p.PaymentTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PaymentTr__LoanA__71D1E811");

            entity.HasOne(d => d.Repayment).WithMany(p => p.PaymentTransactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PaymentTr__Repay__72C60C4A");
        });

        modelBuilder.Entity<PersonalLoanDetail>(entity =>
        {
            entity.HasKey(e => e.LoanProductId).HasName("PK__Personal__0D22CCC25BEB7D71");

            entity.Property(e => e.LoanProductId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.LoanProduct).WithOne(p => p.PersonalLoanDetail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PersonalL__LoanP__4AB81AF0");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C27F40B78");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
