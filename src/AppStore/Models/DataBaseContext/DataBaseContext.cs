
using AppStore.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppStore.Models.DBContext
{
    public class DataBaseContext : IdentityDbContext<ApplicationUser>
    {

        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Libro>()
                    .HasMany(x => x.CategoriaRelationList)
                    .WithMany(y => y.LibroRelationList)
                    .UsingEntity<LibroCategoria>(j => j.HasOne(p => p.Categoria).WithMany(p => p.LibroCategoriaRelationList).HasForeignKey(p => p.CategoriaId),
                                                 j => j.HasOne(p => p.Libro).WithMany(p => p.LibroCategoriaRelationList).HasForeignKey(p => p.LibroId),
                                                 j => {j.HasKey(t => new{t.LibroId, t.CategoriaId});}
                                                 );

        }

        public DbSet<Categoria> Categorias {get; set;}
         public DbSet<Libro> Libros {get; set;}
         public DbSet<LibroCategoria> LibroCategorias {get; set;}
    }
    
}
