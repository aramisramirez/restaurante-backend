using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace backendrestaurante.Models;

public partial class RestauranteContext : DbContext
{
    public RestauranteContext()
    {
    }

    public RestauranteContext(DbContextOptions<RestauranteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Consumo> Consumos { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Mesa> Mesas { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Clientes__71ABD0A72A36D254");

            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<Consumo>(entity =>
        {
            entity.HasKey(e => e.ConsumoId).HasName("PK__Consumos__206D9CC66B71EEE5");

            entity.Property(e => e.ConsumoId).HasColumnName("ConsumoID");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.ReservaId).HasColumnName("ReservaID");

            entity.HasOne(d => d.Item).WithMany(p => p.Consumos)
                .HasForeignKey(d => d.ItemId)
                .HasConstraintName("FK__Consumos__ItemID__440B1D61");

            entity.HasOne(d => d.Reserva).WithMany(p => p.Consumos)
                .HasForeignKey(d => d.ReservaId)
                .HasConstraintName("FK__Consumos__Reserv__4316F928");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.FacturaId).HasName("PK__Facturas__5C0248059F995A20");

            entity.Property(e => e.FacturaId).HasColumnName("FacturaID");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReservaId).HasColumnName("ReservaID");
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Reserva).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.ReservaId)
                .HasConstraintName("FK__Facturas__Reserv__47DBAE45");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PK__Menu__727E83EBB2A01C8A");

            entity.ToTable("Menu");

            entity.Property(e => e.ItemId).HasColumnName("ItemID");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Tipo).HasMaxLength(50);
        });

        modelBuilder.Entity<Mesa>(entity =>
        {
            entity.HasKey(e => e.MesaId).HasName("PK__Mesas__6A4196C84CBD1EE3");

            entity.HasIndex(e => e.Numero, "UQ__Mesas__7E532BC6AC44BEEA").IsUnique();

            entity.Property(e => e.MesaId).HasColumnName("MesaID");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.ReservaId).HasName("PK__Reservas__C399370316EA9E22");

            entity.Property(e => e.ReservaId).HasColumnName("ReservaID");
            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.MesaId).HasColumnName("MesaID");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("FK__Reservas__Client__3C69FB99");

            entity.HasOne(d => d.Mesa).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.MesaId)
                .HasConstraintName("FK__Reservas__MesaID__3D5E1FD2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
