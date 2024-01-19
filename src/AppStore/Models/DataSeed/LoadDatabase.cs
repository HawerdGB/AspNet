
using AppStore.Models.DBContext;
using AppStore.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace AppStore.Models.DataSeed
{
    public class LoadDatabase
    {
        public static async Task InsertarData(DataBaseContext context, UserManager<ApplicationUser> UsuarioManager, RoleManager<IdentityRole> roleManager)
        {
             if(!roleManager.Roles.Any())
             {
                await roleManager.CreateAsync(new IdentityRole("ADMIN"));
             }

             if(!UsuarioManager.Users.Any())
             {
                var usuario = new ApplicationUser 
                {
                     Nombre = "Hawerd Gutierrez",
                     Email = "hawerdgutierrez@gmail.com",
                     UserName = "HawerdG"
                };

                await UsuarioManager.CreateAsync(usuario,"Hlgb211121$");
                await UsuarioManager.AddToRoleAsync(usuario, "ADMIN");
             }

             if(!context.Categorias!.Any())
             {
                 await context.Categorias.AddRangeAsync(
                    new Categoria() {Nombre="Drama"},
                    new Categoria() {Nombre="Comedia"},
                    new Categoria() {Nombre="Terror"},
                    new Categoria() {Nombre="Accion"},
                    new Categoria() {Nombre="Aventura"},
                    new Categoria() {Nombre="Anime"}
                   );
                    await context.SaveChangesAsync();

             }

             if(!context.Libros!.Any()){
               await context.AddRangeAsync(
                    new Libro {
                        Titulo ="Don Quijote de la Mancha",
                        CreateDate="06/06/2020",
                         Imagen="quijote.jpg",
                         Autor="Miguel Cervantes"
                        },
                         new Libro {
                        Titulo ="Harry Potter",
                        CreateDate="05/04/2022",
                         Imagen="harry.jpg",
                         Autor="Juan de la Vega"
                        }
                        
                );

                 await context.SaveChangesAsync();
             }

             if(!context.LibroCategorias.Any())
             {
                await context.LibroCategorias.AddRangeAsync(
                    new LibroCategoria { CategoriaId= 1, LibroId=1},
                    new LibroCategoria { CategoriaId= 1, LibroId=2}
                        
                );

                await context.SaveChangesAsync();
             }
             
        }
    }
}