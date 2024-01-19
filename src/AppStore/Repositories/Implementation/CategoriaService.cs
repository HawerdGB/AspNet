using AppStore.Models.DBContext;
using AppStore.Models.Domain;
using AppStore.Models.DTO;
using AppStore.Repositories.Abstract;

namespace AppStore.Repositories.Implementation
{
    public class CategoriaService : ICategoriaService
    {
        private readonly DataBaseContext ctx;
        public CategoriaService(DataBaseContext _ctx)
        {
            ctx = _ctx;
        }

        public bool Add(Categoria categoria)
        {
            try
            {
              ctx.Categorias.Add(categoria);
              ctx.SaveChanges();

              return true;
            }
            catch (Exception)
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
             
            ctx.Categorias.Remove(data);
            
            ctx.SaveChanges();

             return true;
           }
           catch (Exception)
            {
                return false;
            }
          
        }

        public Categoria GetById(int id)
        {
            var categoria = ctx.Categorias.Find(id)!;
            
           return categoria;
        }

        // public IQueryable<Categoria> List()
        // {
        //    return ctx.Categorias.AsQueryable();
        // }

        public IQueryable<Categoria> List()
        {
           
           return ctx.Categorias.AsQueryable();
        }

        public bool Update(Categoria categoria)
        {
            try
            {
               ctx.Categorias!.Update(categoria);
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