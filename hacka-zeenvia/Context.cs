using System;
using hacka_zeenvia.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace hacka_zeenvia
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options)
          : base(options)
        {

        }


        public virtual DbSet<Produto> Produto { get; set; }
        public virtual DbSet<Feirante> Feirante { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<FeiranteProduto> FeiranteProduto { get; set; }
        public virtual DbSet<MensagemZAP> MensagemZAP { get; set; }
        public virtual DbSet<Pedido> Pedido { get; set; }
        public virtual DbSet<ItemPedido> ItemPedido { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>(ConfigureProduto);
            modelBuilder.Entity<Feirante>(ConfigureFeirante);
            modelBuilder.Entity<FeiranteProduto>(ConfigureFeiranteProduto);
            modelBuilder.Entity<Cliente>(ConfigureCliente);
            modelBuilder.Entity<MensagemZAP>(ConfigureMensagemZAP);
            modelBuilder.Entity<Pedido>(ConfigurePedido);
        }

        private void ConfigureProduto(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(x => x.ProdutoId);
        }

        private void ConfigureFeirante(EntityTypeBuilder<Feirante> builder)
        {

            builder.HasKey(x => x.FeiranteId);

        }

        private void ConfigureFeiranteProduto(EntityTypeBuilder<FeiranteProduto> builder)
        {
            builder.HasKey(x => x.FeiranteProdutoId);

            builder.HasOne(x => x.Feirante)
                   .WithMany(x => x.FeiranteProdutos)
                   .HasForeignKey(x => x.FeiranteId);

            builder.HasOne(x => x.Produto)
                   .WithMany(x => x.FeiranteProdutos)
                   .HasForeignKey(x => x.ProdutoId);
        }

        private void ConfigureCliente(EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(x => x.ClienteId);
        }

        private void ConfigureMensagemZAP(EntityTypeBuilder<MensagemZAP> builder)
        {
            builder.HasKey(x => x.MensagemZAPId);
        }

        private void ConfigurePedido(EntityTypeBuilder<Pedido> builder)
        {
            builder.HasKey(x => x.PedidoId);

            builder.HasOne(x => x.Feirante)
                   .WithMany(x => x.Pedidos)
                   .HasForeignKey(x => x.FeiranteId);

            builder.HasOne(x => x.Cliente)
                   .WithMany(x => x.Pedidos)
                   .HasForeignKey(x => x.ClienteId);
        }

        private void ConfigureItemPedido(EntityTypeBuilder<ItemPedido> builder)
        {
            builder.HasKey(x => x.ItemPedidoId);

            builder.HasOne(x => x.Pedido)
                   .WithMany(x => x.ItensPedido)
                   .HasForeignKey(x => x.PedidoId);

            builder.HasOne(x => x.FeiranteProduto)
                   .WithMany(x => x.ItensPedido)
                   .HasForeignKey(x => x.FeiranteProdutoId);
        }

    }
}
