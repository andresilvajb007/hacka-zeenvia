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
        public virtual DbSet<MensagemZAP> MensagemZAP { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produto>(ConfigureProduto);
            modelBuilder.Entity<Feirante>(ConfigureFeirante);
            modelBuilder.Entity<Cliente>(ConfigureCliente);
            modelBuilder.Entity<MensagemZAP>(ConfigureMensagemZAP);
        }

        private void ConfigureProduto(EntityTypeBuilder<Produto> builder)
        {
            builder.HasKey(x => x.ProdutoId);


        }

        private void ConfigureFeirante(EntityTypeBuilder<Feirante> builder)
        {

            builder.HasKey(x => x.FeiranteId);

            
        }

        private void ConfigureCliente(EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(x => x.ClienteId);
        }

        private void ConfigureMensagemZAP(EntityTypeBuilder<MensagemZAP> builder)
        {
            builder.HasKey(x => x.MensagemZAPId);
        }

    }
}
