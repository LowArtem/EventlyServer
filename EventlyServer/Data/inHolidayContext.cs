using EventlyServer.Data.Entities;
using EventlyServer.Data.Mappers;
using Microsoft.EntityFrameworkCore;

namespace EventlyServer.Data;

public partial class InHolidayContext : DbContext
{
    public InHolidayContext()
    {
    }

    public InHolidayContext(DbContextOptions<InHolidayContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Guest> Guests { get; set; } = null!;
    public virtual DbSet<LandingInvitation> LandingInvitations { get; set; } = null!;
    public virtual DbSet<Response> Responses { get; set; } = null!;
    public virtual DbSet<Template> Templates { get; set; } = null!;
    public virtual DbSet<TypesOfEvent> TypesOfEvents { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=inHoliday;Username=postgres;Password=root");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Guest>(entity =>
        {
            entity.ToTable("guests");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(11)
                .HasColumnName("phone_number")
                .IsFixedLength();
        });

        modelBuilder.Entity<LandingInvitation>(entity =>
        {
            entity.ToTable("landing_invitations");

            entity.HasIndex(e => e.Link, "landing_invitations_link_key")
                .IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.FinishDate).HasColumnName("finish_date");

            entity.Property(e => e.IdClient).HasColumnName("id_client");

            entity.Property(e => e.IdTemplate).HasColumnName("id_template");

            entity.Property(e => e.Link)
                .HasMaxLength(100)
                .HasColumnName("link");

            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.Property(e => e.OrderStatus)
                .HasConversion(
                    v => v.ToString(), 
                    v => v.ToOrderStatuses())
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.Client)
                .WithMany(p => p.LandingInvitations)
                .HasForeignKey(d => d.IdClient)
                .HasConstraintName("landing_invitations_id_client_fkey");

            entity.HasOne(d => d.ChosenTemplate)
                .WithMany(p => p.LandingInvitations)
                .HasForeignKey(d => d.IdTemplate)
                .HasConstraintName("landing_invitations_id_template_fkey");
        });

        modelBuilder.Entity<Response>(entity =>
        {
            entity.HasKey(e => new { e.IdGuest, e.IdLandingInvitation })
                .HasName("responses_pkey");

            entity.ToTable("responses");

            entity.Property(e => e.IdGuest).HasColumnName("id_guest");

            entity.Property(e => e.IdLandingInvitation).HasColumnName("id_landing_invitation");

            entity.Property(e => e.Date).HasColumnName("date");

            entity.HasOne(d => d.GuestNavigation)
                .WithMany(p => p.Responses)
                .HasForeignKey(d => d.IdGuest)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("responses_id_guest_fkey");

            entity.HasOne(d => d.LandingInvitationNavigation)
                .WithMany(p => p.Responses)
                .HasForeignKey(d => d.IdLandingInvitation)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("responses_id_landing_invitation_fkey");
        });

        modelBuilder.Entity<Template>(entity =>
        {
            entity.ToTable("templates");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.IdTypeOfEvent).HasColumnName("id_type_of_event");

            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");

            entity.Property(e => e.FilePath)
                .HasMaxLength(100)
                .HasColumnName("file_path");
            
            entity.Property(e => e.PreviewPath)
                .HasMaxLength(100)
                .HasColumnName("preview_path");

            entity.HasOne(d => d.ChosenTypeOfEvent)
                .WithMany(p => p.Templates)
                .HasForeignKey(d => d.IdTypeOfEvent)
                .HasConstraintName("templates_id_type_of_event_fkey");
        });

        modelBuilder.Entity<TypesOfEvent>(entity =>
        {
            entity.ToTable("types_of_event");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key")
                .IsUnique();

            entity.HasIndex(e => e.Password, "users_password_key")
                .IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");

            entity.Property(e => e.IsAdmin).HasColumnName("is_admin");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.Property(e => e.OtherCommunication)
                .HasMaxLength(300)
                .HasColumnName("other_communication");

            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");

            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(11)
                .HasColumnName("phone_number")
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}