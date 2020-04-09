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
        public virtual DbSet<TOnlineUsers> TOnlineUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=zxcv1234");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("adminpack");

            modelBuilder.Entity<TComments>(entity =>
            {
                entity.ToTable("t_Comments");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.TComments)
                    .HasForeignKey(d => d.IdProduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_Comments_IdProduct_fkey");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.TComments)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_Comments_IdUser_fkey");
            });

            modelBuilder.Entity<TDeals>(entity =>
            {
                entity.ToTable("t_Deals");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.IdBuyersNavigation)
                    .WithMany(p => p.TDeals)
                    .HasForeignKey(d => d.IdBuyers)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_Deals_IdBuyers_fkey");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.TDeals)
                    .HasForeignKey(d => d.IdProduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_Deals_IdProduct_fkey");
            });

            modelBuilder.Entity<TOnlineUsers>(entity =>
            {
                entity.ToTable("t_OnlineUsers");

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.HasOne(d => d.IdUsersNavigation)
                   .WithMany(p => p.TOnlineUsers)
                   .HasForeignKey(d => d.Id)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("t_OnlineUsers_IdUser_fkey");
            });

            modelBuilder.Entity<TDeveloper>(entity =>
            {
                entity.ToTable("t_Developer");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.TDeveloper)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_Developer_IdUser_fkey");
            });

            modelBuilder.Entity<TGameGenre>(entity =>
            {
                entity.HasKey(e => new { e.IdGame, e.IdGenre })
                    .HasName("t_GameGenre_pkey");

                entity.ToTable("t_GameGenre");

                entity.HasOne(d => d.IdGameNavigation)
                    .WithMany(p => p.TGameGenre)
                    .HasForeignKey(d => d.IdGame)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_GameGenre_IdGame_fkey");

                entity.HasOne(d => d.IdGenreNavigation)
                    .WithMany(p => p.TGameGenre)
                    .HasForeignKey(d => d.IdGenre)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_GameGenre_IdGenre_fkey");
            });

            modelBuilder.Entity<TGenre>(entity =>
            {
                entity.ToTable("t_Genre");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<TLogin>(entity =>
            {
                entity.ToTable("t_Login");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login")
                    .HasColumnType("character varying");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.IdOwnerNavigation)
                    .WithMany(p => p.TLogin)
                    .HasForeignKey(d => d.IdOwner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_Login_IdOwner_fkey");
            });

            modelBuilder.Entity<TMinGameSysReq>(entity =>
            {
                entity.HasKey(e => new { e.IdGame, e.IdSysReq })
                    .HasName("t_MinGameSysReq_pkey");

                entity.ToTable("t_MinGameSysReq");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.HasOne(d => d.IdGameNavigation)
                    .WithMany(p => p.TMinGameSysReq)
                    .HasForeignKey(d => d.IdGame)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_MinGameSysReq_IdGame_fkey");

                entity.HasOne(d => d.IdSysReqNavigation)
                    .WithMany(p => p.TMinGameSysReq)
                    .HasForeignKey(d => d.IdSysReq)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_MinGameSysReq_IdSysReq_fkey");
            });

            modelBuilder.Entity<TModerateEmployers>(entity =>
            {
                entity.HasKey(e => new { e.IdEmployee, e.IdModerateProduct })
                    .HasName("t_ModerateEmployers_pkey");

                entity.ToTable("t_ModerateEmployers");

                entity.HasOne(d => d.IdEmployeeNavigation)
                    .WithMany(p => p.TModerateEmployers)
                    .HasForeignKey(d => d.IdEmployee)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_ModerateEmployers_IdEmployee_fkey");

                entity.HasOne(d => d.IdModerateProductNavigation)
                    .WithMany(p => p.TModerateEmployers)
                    .HasForeignKey(d => d.IdModerateProduct)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_ModerateEmployers_IdModerateProduct_fkey");
            });

            modelBuilder.Entity<TModerateProducts>(entity =>
            {
                entity.ToTable("t_ModerateProducts");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.RequestDate).HasColumnType("date");

                entity.HasOne(d => d.IdDeveloperNavigation)
                    .WithMany(p => p.TModerateProducts)
                    .HasForeignKey(d => d.IdDeveloper)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_ModerateProducts_IdDeveloper_fkey");

                entity.HasOne(d => d.IdPublisherNavigation)
                    .WithMany(p => p.TModerateProducts)
                    .HasForeignKey(d => d.IdPublisher)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_ModerateProducts_IdPublisher_fkey");
            });

            modelBuilder.Entity<TProducts>(entity =>
            {
                entity.ToTable("t_Products");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.Property(e => e.ReleaseDate).HasColumnType("date");

                entity.HasOne(d => d.IdDeveloperNavigation)
                    .WithMany(p => p.TProducts)
                    .HasForeignKey(d => d.IdDeveloper)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_Products_IdDeveloper_fkey");

                entity.HasOne(d => d.IdPublisherNavigation)
                    .WithMany(p => p.TProducts)
                    .HasForeignKey(d => d.IdPublisher)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_Products_IdPublisher_fkey");
            });

            modelBuilder.Entity<TPublisher>(entity =>
            {
                entity.ToTable("t_Publisher");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.TPublisher)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_Publisher_IdUser_fkey");
            });

            modelBuilder.Entity<TRecGameSysReq>(entity =>
            {
                entity.HasKey(e => new { e.IdGame, e.IdSysReq })
                    .HasName("t_RecGameSysReq_pkey");

                entity.ToTable("t_RecGameSysReq");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.HasOne(d => d.IdGameNavigation)
                    .WithMany(p => p.TRecGameSysReq)
                    .HasForeignKey(d => d.IdGame)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_RecGameSysReq_IdGame_fkey");

                entity.HasOne(d => d.IdSysReqNavigation)
                    .WithMany(p => p.TRecGameSysReq)
                    .HasForeignKey(d => d.IdSysReq)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("t_RecGameSysReq_IdSysReq_fkey");
            });

            modelBuilder.Entity<TSysReq>(entity =>
            {
                entity.ToTable("t_SysReq");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<TUsers>(entity =>
            {
                entity.ToTable("t_Users");

                entity.Property(e => e.Id).ValueGeneratedNever();

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
