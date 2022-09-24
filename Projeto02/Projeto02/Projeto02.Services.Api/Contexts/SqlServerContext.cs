using Microsoft.EntityFrameworkCore;
using Projeto02.Services.Api.Entities;

namespace Projeto02.Services.Api.Contexts
{
    /// <summary>
    /// Classe para acesso ao SqlServer
    /// </summary>
    public class SqlServerContext : DbContext
    {
        //construtor para injeção de dependência
        public SqlServerContext(DbContextOptions<SqlServerContext> options)
            : base(options)
        {

        }

        //mapeamento / configuração das entidades
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

        //propriedades DbSet para cada entidade
        public DbSet<Usuario>? Usuarios { get; set; }
    }
}
