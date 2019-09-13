using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CsAspnet.Models.dbcontext
{
    public partial class motionsContext : DbContext
    {
        public motionsContext()
        {
        }

        public motionsContext(DbContextOptions<motionsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Actor> Actor { get; set; }
        public virtual DbSet<Att> Att { get; set; }
        public virtual DbSet<Committee> Committee { get; set; }
        public virtual DbSet<Motion> Motion { get; set; }
        public virtual DbSet<Response> Response { get; set; }
        public virtual DbSet<SuggestedVote> SuggestedVote { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySQL("server=localhost;user=root;database=motions");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>(entity =>
            {
                entity.ToTable("actor", "motions");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ActorName)
                    .IsRequired()
                    .HasColumnName("actor_name")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Att>(entity =>
            {
                entity.ToTable("att", "motions");

                entity.HasIndex(e => e.MotionId)
                    .HasName("att_proposition_motion_id_fk");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AttNumber)
                    .HasColumnName("att_number")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AttText)
                    .IsRequired()
                    .HasColumnName("att_text")
                    .IsUnicode(false);

                entity.Property(e => e.MotionId)
                    .HasColumnName("motion_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Motion)
                    .WithMany(p => p.Att)
                    .HasForeignKey(d => d.MotionId)
                    .HasConstraintName("att_proposition_motion_id_fk");
            });

            modelBuilder.Entity<Committee>(entity =>
            {
                entity.ToTable("committee", "motions");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CommitteeName)
                    .IsRequired()
                    .HasColumnName("committee_name")
                    .IsUnicode(false);

                entity.Property(e => e.CommitteeNumber)
                    .HasColumnName("committee_number")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Motion>(entity =>
            {
                entity.ToTable("motion", "motions");

                entity.HasIndex(e => e.CommitteeId)
                    .HasName("motion_committee_id_fk");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CommitteeId)
                    .HasColumnName("committee_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MotionName)
                    .IsRequired()
                    .HasColumnName("motion_name")
                    .IsUnicode(false);

                entity.Property(e => e.MotionNumber)
                    .HasColumnName("motion_number")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MotionText)
                    .IsRequired()
                    .HasColumnName("motion_text")
                    .IsUnicode(false);

                entity.HasOne(d => d.Committee)
                    .WithMany(p => p.Motion)
                    .HasForeignKey(d => d.CommitteeId)
                    .HasConstraintName("motion_committee_id_fk");
            });

            modelBuilder.Entity<Response>(entity =>
            {
                entity.ToTable("response", "motions");

                entity.HasIndex(e => e.ActorId)
                    .HasName("response_actor_id_fk");

                entity.HasIndex(e => e.MotionId)
                    .HasName("response_motion_id_fk");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ActorId)
                    .HasColumnName("actor_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.MotionId)
                    .HasColumnName("motion_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ResponseText)
                    .IsRequired()
                    .HasColumnName("response_text")
                    .IsUnicode(false);

                entity.HasOne(d => d.Actor)
                    .WithMany(p => p.Response)
                    .HasForeignKey(d => d.ActorId)
                    .HasConstraintName("response_actor_id_fk");

                entity.HasOne(d => d.Motion)
                    .WithMany(p => p.Response)
                    .HasForeignKey(d => d.MotionId)
                    .HasConstraintName("response_motion_id_fk");
            });

            modelBuilder.Entity<SuggestedVote>(entity =>
            {
                entity.ToTable("suggested_vote", "motions");

                entity.HasIndex(e => e.ActorId)
                    .HasName("suggested_vote_actor_id_fk");

                entity.HasIndex(e => e.AttId)
                    .HasName("suggested_vote_att_id_fk");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ActorId)
                    .HasColumnName("actor_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AttId)
                    .HasColumnName("att_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Vote)
                    .IsRequired()
                    .HasColumnName("vote")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.HasOne(d => d.Actor)
                    .WithMany(p => p.SuggestedVote)
                    .HasForeignKey(d => d.ActorId)
                    .HasConstraintName("suggested_vote_actor_id_fk");

                entity.HasOne(d => d.Att)
                    .WithMany(p => p.SuggestedVote)
                    .HasForeignKey(d => d.AttId)
                    .HasConstraintName("suggested_vote_att_id_fk");
            });
        }
    }
}
