

using System.IO.Compression;
using AppStore.Models.DBContext;
using AppStore.Models.Domain;
using AppStore.Models.DTO;
using AppStore.Repositories.Abstract;

namespace AppStore.Repositories.Implementation
{
    public class LibroService : ILibroService
    {
       private readonly DataBaseContext ctx;

       public LibroService(DataBaseContext _ctx)
       {
        ctx = _ctx;
       }
        public bool Add(Libro libro)
        {
          try
          {
            ctx.Libros!.Add(libro);
            ctx.SaveChanges();

            foreach(int categoriaId in libro.Categorias!)
            {
                var libroCategoria = new LibroCategoria
                {
                    LibroId = libro.Id,
                    CategoriaId = categoriaId
                };

                ctx.LibroCategorias.Add(libroCategoria);
            }
            ctx.SaveChanges();

            return true;

          }
          catch(Exception)
          {
            return false;
          }
          
        }

        public bool Delete(int id)
        {
           try
           {
             var data = GetById(id);

             if(data is null)
             {
                return false;
             }
             
             var libroCategorias = ctx.LibroCategorias.Where(x => x.LibroId == id);
             ctx.LibroCategorias.RemoveRange(libroCategorias);
             ctx.Libros.Remove(data);
             ctx.SaveChanges();

             return true;
           }
           catch (Exception)
            {
                return false;
            }
          
        }

        public Libro GetById(int id)
        {
           var libro = ctx.Libros.Find(id)!;
            
           return libro;
        
        }

        public List<int> GetCatecoriaByLibroId(int libroId)
        {
        return [.. ctx.LibroCategorias.Where(x => x.LibroId == libroId).Select(x => x.CategoriaId)];

        }

        public LibroListVm Listar(string term = "", bool paging = true, int currentPage = 1,int pageSize = 5)
        {
            var data = new LibroListVm();
            var query = ctx.Libros.AsQueryable();

            if(!string.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                query = query.Where(x => x.Titulo!.ToLower().Contains(term));
            }

            if(paging)
            {
               
                int count = query.Count();
                int totalPages = (int)Math.Ceiling(count/(double)pageSize);
                currentPage = Math.Max(1, Math.Min(currentPage, totalPages));

                query = query.Skip((currentPage-1)*pageSize).Take(pageSize);
                data.PageSize = pageSize;
                data.CurrentPage = currentPage;
                data.TotalPages = totalPages;

            }
            var list = query.ToList();
                    foreach (var libro in list)
                    {
                        var categorias = (
                            from categoria in ctx.Categorias
                            join lc in ctx.LibroCategorias
                            on categoria.Id equals lc.CategoriaId  
                            where lc.LibroId == libro.Id 
                            select categoria.Nombre
                         ).ToList();

                         string categoriaNombres = string.Join(",",categorias);
                         libro.CategoriaNames = categoriaNombres;
                     }

             data.LibroList = list.AsQueryable();

            return data;
        }

        public bool Update(Libro libro)
        {
          try
          {
            var categoriasParaEliminar = ctx.LibroCategorias.Where(x => x.LibroId == libro.Id);
            foreach (var categoria in categoriasParaEliminar)
            {
                ctx.LibroCategorias.Remove(categoria);
            }
            foreach (var categoriaId in libro.Categorias!)
            {
                var libroCategoria = new LibroCategoria{CategoriaId = categoriaId, LibroId = libro.Id};
                ctx.LibroCategorias.Add(libroCategoria);
            }

            ctx.Libros!.Update(libro);
            ctx.SaveChanges();

            return true;


          }
          catch (Exception)
          {
            
           return false;
          }
        }
    }
}