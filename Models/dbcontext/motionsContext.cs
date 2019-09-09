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

        public virtual DbSet<AttProposition> AttProposition { get; set; }
        public virtual DbSet<Committee> Committee { get; set; }
        public virtual DbSet<Motion> Motion { get; set; }
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
            modelBuilder.Entity<AttProposition>(entity =>
            {
                entity.ToTable("att_proposition", "motions");

                entity.HasIndex(e => e.MotionId)
                    .HasName("att_proposition_motion_id_fk");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AttPropositionNumber)
                    .HasColumnName("att_proposition_number")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AttPropositionText)
                    .HasColumnName("att_proposition_text")
                    .IsUnicode(false);

                entity.Property(e => e.MotionId)
                    .HasColumnName("motion_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Motion)
                    .WithMany(p => p.AttProposition)
                    .HasForeignKey(d => d.MotionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
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
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("motion_committee_id_fk");
            });

            modelBuilder.Entity<SuggestedVote>(entity =>
            {
                entity.ToTable("suggested_vote", "motions");

                entity.HasIndex(e => e.AttPropositionId)
                    .HasName("suggested_vote_att_proposition_id_fk");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Actor)
                    .IsRequired()
                    .HasColumnName("actor")
                    .IsUnicode(false);

                entity.Property(e => e.AttPropositionId)
                    .HasColumnName("att_proposition_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SuggestedVote1)
                    .IsRequired()
                    .HasColumnName("suggested_vote")
                    .IsUnicode(false);

                entity.HasOne(d => d.AttProposition)
                    .WithMany(p => p.SuggestedVote)
                    .HasForeignKey(d => d.AttPropositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("suggested_vote_att_proposition_id_fk");
            });
        }
    }
}
