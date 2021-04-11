using Microsoft.EntityFrameworkCore;
using Ncua.Logging.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Ncua.Logging.DAL
{
    public class LoggingContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["Logging"].ConnectionString);
        }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventAttribute> EventAttributes { get; set; }
        public virtual DbSet<EventDetail> EventDetails { get; set; }
        public virtual DbSet<EventType> EventTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("Event");
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => new { e.EventId })
                    .HasName("PK_Event");
                entity.ToTable("Event");
                entity.Property(e => e.CorrelationId).HasColumnType("int");
                entity.Property(e => e.AppDomainName).HasColumnType("varchar(1024)");
                entity.Property(e => e.MachineName).HasColumnType("varchar(1024)");
                entity.Property(e => e.Message).HasColumnType("varchar(4000)");
                entity.Property(e => e.Priority).HasColumnType("int");
                entity.Property(e => e.ProcessId).HasColumnType("varchar(256)");
                entity.Property(e => e.ProcessName).HasColumnType("varchar(512)");
                entity.Property(e => e.Severity).HasColumnType("varchar(50)");
                entity.Property(e => e.ThreadName).HasColumnType("varchar(512)");
                entity.Property(e => e.GenerateDate).HasColumnType("datetime");
                entity.Property(e => e.CreateId).HasColumnType("varchar(50)");
                entity.Property(e => e.CreateDate).HasColumnType("datetime");
                entity.Property(e => e.UpdateId).HasColumnType("varchar(50)");
                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
                entity.HasOne(d => d.EventType)
           .WithMany(p => p.Events)
           .HasForeignKey(d => d.EventTypeId)
           .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EventAttribute>(entity =>
            {
                entity.HasKey(e => new { e.EventAttributeId })
                   .HasName("PK_EventAttribute");
                entity.ToTable("EventAttribute");
                entity.Property(e => e.EventTypeId).HasColumnType("int");
                entity.Property(e => e.Desc).HasColumnType("varchar(50)");
                entity.Property(e => e.CreateId).HasColumnType("varchar(50)");
                entity.Property(e => e.CreateDate).HasColumnType("datetime");
                entity.Property(e => e.UpdateId).HasColumnType("varchar(50)");
                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
                entity.HasOne(d => d.EventType)
             .WithMany(p => p.EventAttributes)
             .HasForeignKey(d => d.EventTypeId)
             .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EventDetail>(entity =>
            {
                entity.HasKey(e => new { e.EventDetailId })
                   .HasName("PK_EventDetail");
                entity.ToTable("EventDetail");
                entity.Property(e => e.Value).HasColumnType("varchar(max)");
                entity.Property(e => e.EventAttributeId).HasColumnType("int");
                entity.Property(e => e.CreateId).HasColumnType("varchar(50)");
                entity.Property(e => e.CreateDate).HasColumnType("datetime");
                entity.Property(e => e.UpdateId).HasColumnType("varchar(50)");
                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
                entity.HasOne(d => d.Event)
              .WithMany(p => p.EventDetails)
              .HasForeignKey(d => d.EventId)
              .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EventType>(entity =>
            {
                entity.HasKey(e => new { e.EventTypeId })
                   .HasName("PK_EventType");
                entity.ToTable("EventType");
                entity.Property(e => e.Desc).HasColumnType("varchar(30)");
                entity.Property(e => e.CreateId).HasColumnType("varchar(50)");
                entity.Property(e => e.CreateDate).HasColumnType("datetime");
                entity.Property(e => e.UpdateId).HasColumnType("varchar(50)");
                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });
        }
    }
}
