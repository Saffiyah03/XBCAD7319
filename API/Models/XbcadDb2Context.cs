﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace api.Models;

public partial class XbcadDb2Context : DbContext
{
    public XbcadDb2Context()
    {
    }

    public XbcadDb2Context(DbContextOptions<XbcadDb2Context> options)
        : base(options)
    {
    }
   public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<TeamDev> TeamDevs { get; set; }

    public virtual DbSet<TicketDetail> TicketDetails { get; set; }

    public virtual DbSet<TicketResponse> TicketResponses { get; set; }

    public virtual DbSet<UserInfo> UserInfos { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:xbcad-server.database.windows.net,1433;Initial Catalog=XBCAD_DB2;Persist Security Info=False;User ID=admin@st10083751@xbcad-server;Password=yusra@24;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TeamDev>(entity =>
        {
            entity.HasKey(e => e.DevId);
        });

        modelBuilder.Entity<TicketDetail>(entity =>
        {
            entity.HasKey(e => e.TicketId);
        });

        modelBuilder.Entity<TicketResponse>(entity =>
        {
            entity.HasKey(e => e.ResponseId);

            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Sender).HasColumnName("sender");
            entity.Property(e => e.TicketId).IsRequired();
        });

        modelBuilder.Entity<UserInfo>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("UserInfo");

            entity.Property(e => e.Email).IsRequired();
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => e.Email);

            entity.ToTable("UserLogin");

            entity.Property(e => e.Password).IsRequired();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
