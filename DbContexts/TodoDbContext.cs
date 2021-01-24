using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

using Todo.AppCtx;
using Todo.Entities;
// using Todo.Services;

namespace Todo.DbContexts
{
    public class TodoDbContext : DbContext
    {
        public virtual DbSet<TodoEntity> Todos { get; set; }

        public TodoDbContext()
        {
            ChangeTracker.StateChanged += UpdateTimestamps;
            ChangeTracker.Tracked += UpdateTimestamps;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseLoggerFactory(new Serilog.Extensions.Logging.SerilogLoggerFactory(LoggingCtx.LogSql));

            options
                .UseSqlite(Program.ConfigCtx.AppDbConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (IMutableForeignKey fk in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
        protected void UpdateTimestamps(object sender, EntityEntryEventArgs e)
        {
            e.Entry.Entity.GetType().GetProperty("Updated").SetValue(e.Entry.Entity, DateTime.UtcNow);
        }        
    }
}