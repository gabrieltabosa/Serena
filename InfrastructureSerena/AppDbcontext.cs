using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DominioSerena;

namespace InfrastructureSerena
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Ativo> Ativos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Localizacao> Localizacoes { get; set; }
        public DbSet<Responsavel> Responsaveis { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
