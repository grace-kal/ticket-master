using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TicketMaster.Models;

namespace TicketMaster.Data
{
    public class TicketMasterDbContext:IdentityDbContext
    {

        public TicketMasterDbContext(DbContextOptions<TicketMasterDbContext> options) : base(options)
        {


        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }  //!
        public DbSet<Project> Projects { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }
        public DbSet<WorkingTime> WorkingTimes { get; set; }
        public DbSet<File> Files { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Company>()
                .HasMany(c => c.Workers)
                .WithOne(w => w.Company)
                .HasForeignKey(w => w.CompanyId);
            builder.Entity<UserProject>()
                .HasKey(u => new { u.UserId, u.ProjectId });
            builder.Entity<Project>()
                .HasOne(p => p.Company)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.CompanyId);

            builder.Entity<Project>()
                .HasMany(p => p.IncomingTickets)
                .WithOne(t => t.Project)
                .HasForeignKey(p => p.ProjectId);
            builder.Entity<User>()
                .HasMany(u => u.SendTickets)
                .WithOne(s => s.Author)
                .HasForeignKey(u => u.AuthorId);
            builder.Entity<User>()
                .HasMany(u => u.AnsweredTickets)
                .WithOne(a => a.Agent)
                .HasForeignKey(u => u.AgentId);
          
            
            
            builder.Entity<UserProject>()
                .HasOne<Project>(u => u.Project)
                .WithMany(p => p.Workers)
                .HasForeignKey(p => p.ProjectId).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<UserProject>()
                .HasOne<User>(p => p.Worker)
                .WithMany(w => w.Projects)
                .HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Ticket>()
                .HasMany(o => o.WorkingTimes)
                .WithOne(w => w.Ticket)
                .HasForeignKey(o => o.TicketId);
            builder.Entity<File>()
                .HasOne(f => f.Ticket)
                .WithMany(t => t.FilesToUpload)
                .HasForeignKey(f => f.TicketId);


            base.OnModelCreating(builder);
        }
    }
}
