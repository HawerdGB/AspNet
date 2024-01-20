
using AppStore.Models.Domain;
using AppStore.Models.DTO;

namespace AppStore.Repositories.Abstract
{
    public interface ILibroService
    {
        bool Add(Libro libro);
        bool Update(Libro libro);
        Libro GetById(int id);
        bool Delete(int id);
        LibroListVm Listar(string term, bool paging, int currentPage,int pageSize);
        List<int> GetCatecoriaByLibroId (int libroId);


    }
} 