using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Service
{
    public partial class postgresContext : DbContext
    {
        public postgresContext()
        {
        }

        public postgresContext(DbContextOptions<postgresContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TComments> TComments { get; set; }
        public virtual DbSet<TDeals> TDeals { get; set; }
        public virtual DbSet<TDeveloper> TDeveloper { get; set; }
        public virtual DbSet<TGameGenre> TGameGenre { get; set; }
        public virtual DbSet<TGenre> TGenre { get; set; }
        public virtual DbSet<TLogin> TLogin { get; set; }
        public virtual DbSet<TMinGameSysReq> TMinGameSysReq { get; set; }
        public virtual DbSet<TModerateEmployers> TModerateEmployers { get; set; }
        public virtual DbSet<TModerateProducts> TModerateProducts { get; set; }
        public virtual DbSet<TProducts> TProducts { get; set; }
        public virtual DbSet<TPublisher> TPublisher { get; set; }
        public virtual DbSet<TRecGameSysReq> TRecGameSysReq { get; set; }
        public virtual DbSet<TSysReq> TSysReq { get; set; }
        public virtual DbSet<TUsers> TUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=zxcv1234");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("adminpack");

            modelBuilder.Entity<TComments>(entity =>
            {
                entity.ToTable("t_Comments");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.Idproduct).HasColumnName("IDProduct");

                entity.Property(e => e.Iduser).HasColumnName("IDUser");

                entity.HasOne(d => d.IdproductNavigation)
                    .WithMany(p => p.TComments)
                    .HasForeignKey(d => d.Idproduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_Comments_IDProduct_fkey");

                entity.HasOne(d => d.IduserNavigation)
                    .WithMany(p => p.TComments)
                    .HasForeignKey(d => d.Iduser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_Comments_IDUser_fkey");
            });

            modelBuilder.Entity<TDeals>(entity =>
            {
                entity.ToTable("t_Deals");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Count)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Idbuyers).HasColumnName("IDBuyers");

                entity.Property(e => e.Idproduct).HasColumnName("IDProduct");

                entity.HasOne(d => d.IdbuyersNavigation)
                    .WithMany(p => p.TDeals)
                    .HasForeignKey(d => d.Idbuyers)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_Deals_IDBuyers_fkey");

                entity.HasOne(d => d.IdproductNavigation)
                    .WithMany(p => p.TDeals)
                    .HasForeignKey(d => d.Idproduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_Deals_IDProduct_fkey");
            });

            modelBuilder.Entity<TDeveloper>(entity =>
            {
                entity.ToTable("t_Developer");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Iduser).HasColumnName("IDUser");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.HasOne(d => d.IduserNavigation)
                    .WithMany(p => p.TDeveloper)
                    .HasForeignKey(d => d.Iduser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_Developer_IDUser_fkey");
            });
            modelBuilder.Entity<TGameGenre>(entity =>
            {
                entity.HasKey(e => new { e.Idgame, e.Idgenre })
                    .HasName("t_GameGenre_pkey");

                entity.ToTable("t_GameGenre");

                entity.Property(e => e.Idgame).HasColumnName("IDGame");

                entity.Property(e => e.Idgenre).HasColumnName("IDGenre");

                entity.HasOne(d => d.IdgameNavigation)
                    .WithMany(p => p.TGameGenre)
                    .HasForeignKey(d => d.Idgame)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_GameGenre_IDGame_fkey");

                entity.HasOne(d => d.IdgenreNavigation)
                    .WithMany(p => p.TGameGenre)
                    .HasForeignKey(d => d.Idgenre)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_GameGenre_IDGenre_fkey");
            });

            modelBuilder.Entity<TGenre>(entity =>
            {
                entity.ToTable("t_Genre");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<TLogin>(entity =>
            {
                entity.ToTable("t_Login");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Idowner).HasColumnName("IDOwner");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login")
                    .HasColumnType("character varying");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.IdownerNavigation)
                    .WithMany(p => p.TLogin)
                    .HasForeignKey(d => d.Idowner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_Login_IDOwner_fkey");
            });

            modelBuilder.Entity<TMinGameSysReq>(entity =>
            {
                entity.HasKey(e => new { e.Idgame, e.IdsysReq })
                    .HasName("t_MinGameSysReq_pkey");

                entity.ToTable("t_MinGameSysReq");

                entity.Property(e => e.Idgame).HasColumnName("IDGame");

                entity.Property(e => e.IdsysReq).HasColumnName("IDSysReq");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.HasOne(d => d.IdgameNavigation)
                    .WithMany(p => p.TMinGameSysReq)
                    .HasForeignKey(d => d.Idgame)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_MinGameSysReq_IDGame_fkey");

                entity.HasOne(d => d.IdsysReqNavigation)
                    .WithMany(p => p.TMinGameSysReq)
                    .HasForeignKey(d => d.IdsysReq)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_MinGameSysReq_IDSysReq_fkey");
            });

            modelBuilder.Entity<TModerateEmployers>(entity =>
            {
                entity.HasKey(e => new { e.Idemployee, e.IdmoderateProduct })
                    .HasName("t_ModerateEmployers_pkey");

                entity.ToTable("t_ModerateEmployers");

                entity.Property(e => e.Idemployee).HasColumnName("IDEmployee");

                entity.Property(e => e.IdmoderateProduct).HasColumnName("IDModerateProduct");

                entity.HasOne(d => d.IdemployeeNavigation)
                    .WithMany(p => p.TModerateEmployers)
                    .HasForeignKey(d => d.Idemployee)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_ModerateEmployers_IDEmployee_fkey");

                entity.HasOne(d => d.IdmoderateProductNavigation)
                    .WithMany(p => p.TModerateEmployers)
                    .HasForeignKey(d => d.IdmoderateProduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_ModerateEmployers_IDModerateProduct_fkey");
            });

            modelBuilder.Entity<TModerateProducts>(entity =>
            {
                entity.ToTable("t_ModerateProducts");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.Iddeveloper).HasColumnName("IDDeveloper");

                entity.Property(e => e.Idpublisher).HasColumnName("IDPublisher");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.RequestDate).HasColumnType("date");

                entity.HasOne(d => d.IddeveloperNavigation)
                    .WithMany(p => p.TModerateProducts)
                    .HasForeignKey(d => d.Iddeveloper)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_ModerateProducts_IDDeveloper_fkey");

                entity.HasOne(d => d.IdpublisherNavigation)
                    .WithMany(p => p.TModerateProducts)
                    .HasForeignKey(d => d.Idpublisher)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_ModerateProducts_IDPublisher_fkey");
            });

            modelBuilder.Entity<TProducts>(entity =>
            {
                entity.ToTable("t_Products");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.Iddeveloper).HasColumnName("IDDeveloper");

                entity.Property(e => e.Idpublisher).HasColumnName("IDPublisher");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.ReleaseDate).HasColumnType("date");

                entity.HasOne(d => d.IddeveloperNavigation)
                    .WithMany(p => p.TProducts)
                    .HasForeignKey(d => d.Iddeveloper)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_Products_IDDeveloper_fkey");

                entity.HasOne(d => d.IdpublisherNavigation)
                    .WithMany(p => p.TProducts)
                    .HasForeignKey(d => d.Idpublisher)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_Products_IDPublisher_fkey");
            });

            modelBuilder.Entity<TPublisher>(entity =>
            {
                entity.ToTable("t_Publisher");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Iduser).HasColumnName("IDUser");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.HasOne(d => d.IduserNavigation)
                    .WithMany(p => p.TPublisher)
                    .HasForeignKey(d => d.Iduser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_Publisher_IDUser_fkey");
            });

            modelBuilder.Entity<TRecGameSysReq>(entity =>
            {
                entity.HasKey(e => new { e.Idgame, e.IdsysReq })
                    .HasName("t_RecGameSysReq_pkey");

                entity.ToTable("t_RecGameSysReq");

                entity.Property(e => e.Idgame).HasColumnName("IDGame");

                entity.Property(e => e.IdsysReq).HasColumnName("IDSysReq");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.HasOne(d => d.IdgameNavigation)
                    .WithMany(p => p.TRecGameSysReq)
                    .HasForeignKey(d => d.Idgame)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_RecGameSysReq_IDGame_fkey");

                entity.HasOne(d => d.IdsysReqNavigation)
                    .WithMany(p => p.TRecGameSysReq)
                    .HasForeignKey(d => d.IdsysReq)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_RecGameSysReq_IDSysReq_fkey");
            });

            modelBuilder.Entity<TSysReq>(entity =>
            {
                entity.ToTable("t_SysReq");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<TUsers>(entity =>
            {
                entity.ToTable("t_Users");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.Name).HasColumnType("character varying");

                entity.Property(e => e.Telephone).HasColumnType("character varying");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
