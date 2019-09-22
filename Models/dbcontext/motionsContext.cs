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

        public virtual DbSet<Att> Att { get; set; }
        public virtual DbSet<Committee> Committee { get; set; }
        public virtual DbSet<Motion> Motion { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

                entity.Property(e => e.Author)
                    .HasColumnName("author")
                    .IsUnicode(false);

                entity.Property(e => e.MainProposal)
                    .HasColumnName("main_proposal")
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.MotionId)
                    .HasColumnName("motion_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SuggestedVote)
                    .HasColumnName("suggested_vote")
                    .HasMaxLength(6)
                    .IsUnicode(false);

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

                entity.Property(e => e.Argument)
                    .HasColumnName("argument")
                    .IsUnicode(false);

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
        }
    }
}
