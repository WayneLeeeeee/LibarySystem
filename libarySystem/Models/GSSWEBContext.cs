using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace libarySystem.Models;

public partial class GSSWEBContext : DbContext
{
    public GSSWEBContext(DbContextOptions<GSSWEBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BOOK_CLASS> BOOK_CLASS { get; set; }

    public virtual DbSet<BOOK_CODE> BOOK_CODE { get; set; }

    public virtual DbSet<BOOK_DATA> BOOK_DATA { get; set; }

    public virtual DbSet<BOOK_LEND_RECORD> BOOK_LEND_RECORD { get; set; }

    public virtual DbSet<MEMBER_M> MEMBER_M { get; set; }

    public virtual DbSet<SPAN_TABLE> SPAN_TABLE { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BOOK_CLASS>(entity =>
        {
            entity.HasKey(e => e.BOOK_CLASS_ID).IsClustered(false);

            entity.Property(e => e.BOOK_CLASS_ID).HasMaxLength(4);
            entity.Property(e => e.BOOK_CLASS_NAME).HasMaxLength(60);
            entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");
            entity.Property(e => e.CREATE_USER).HasMaxLength(12);
            entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");
            entity.Property(e => e.MODIFY_USER).HasMaxLength(12);
        });

        modelBuilder.Entity<BOOK_CODE>(entity =>
        {
            entity.HasKey(e => new { e.CODE_TYPE, e.CODE_ID });

            entity.Property(e => e.CODE_TYPE).HasMaxLength(50);
            entity.Property(e => e.CODE_ID).HasMaxLength(100);
            entity.Property(e => e.CODE_NAME).HasMaxLength(200);
            entity.Property(e => e.CODE_TYPE_DESC).HasMaxLength(200);
            entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");
            entity.Property(e => e.CREATE_USER).HasMaxLength(10);
            entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");
            entity.Property(e => e.MODIFY_USER).HasMaxLength(10);
        });

        modelBuilder.Entity<BOOK_DATA>(entity =>
        {
            entity.HasKey(e => e.BOOK_ID).HasName("PK_BOOK_DATA_1");

            entity.Property(e => e.BOOK_AUTHOR).HasMaxLength(30);
            entity.Property(e => e.BOOK_BOUGHT_DATE).HasColumnType("datetime");
            entity.Property(e => e.BOOK_CLASS_ID).HasMaxLength(4);
            entity.Property(e => e.BOOK_KEEPER).HasMaxLength(12);
            entity.Property(e => e.BOOK_NAME).HasMaxLength(200);
            entity.Property(e => e.BOOK_NOTE).HasMaxLength(1200);
            entity.Property(e => e.BOOK_PUBLISHER).HasMaxLength(20);
            entity.Property(e => e.BOOK_STATUS)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");
            entity.Property(e => e.CREATE_USER).HasMaxLength(12);
            entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");
            entity.Property(e => e.MODIFY_USER).HasMaxLength(12);
        });

        modelBuilder.Entity<BOOK_LEND_RECORD>(entity =>
        {
            entity.HasKey(e => e.IDENTITY_FILED);

            entity.Property(e => e.CRE_DATE).HasColumnType("datetime");
            entity.Property(e => e.CRE_USR).HasMaxLength(12);
            entity.Property(e => e.KEEPER_ID).HasMaxLength(12);
            entity.Property(e => e.LEND_DATE).HasColumnType("datetime");
            entity.Property(e => e.MOD_DATE).HasColumnType("datetime");
            entity.Property(e => e.MOD_USR).HasMaxLength(12);
        });

        modelBuilder.Entity<MEMBER_M>(entity =>
        {
            entity.HasKey(e => e.USER_ID);

            entity.Property(e => e.USER_ID).HasMaxLength(12);
            entity.Property(e => e.CREATE_DATE).HasColumnType("datetime");
            entity.Property(e => e.CREATE_USER).HasMaxLength(12);
            entity.Property(e => e.MODIFY_DATE).HasColumnType("datetime");
            entity.Property(e => e.MODIFY_USER).HasMaxLength(12);
            entity.Property(e => e.USER_CNAME).HasMaxLength(50);
            entity.Property(e => e.USER_ENAME).HasMaxLength(50);
        });

        modelBuilder.Entity<SPAN_TABLE>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.CRE_DATE).HasColumnType("datetime");
            entity.Property(e => e.CRE_USR).HasMaxLength(12);
            entity.Property(e => e.IDENTITY_FILED).ValueGeneratedOnAdd();
            entity.Property(e => e.MOD_DATE).HasColumnType("datetime");
            entity.Property(e => e.MOD_USR).HasMaxLength(12);
            entity.Property(e => e.NOTE).HasMaxLength(12);
            entity.Property(e => e.SPAN_END).HasMaxLength(12);
            entity.Property(e => e.SPAN_START).HasMaxLength(12);
            entity.Property(e => e.SPAN_YEAR).HasMaxLength(12);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
