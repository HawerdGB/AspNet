using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStore.Models.Domain;
using AppStore.Models.DTO;

namespace AppStore.Repositories.Abstract
{
    public interface ICategoriaService
    {
        bool Add(Categoria categoria);
        bool Update(Categoria categoria);
        Categoria GetById(int id);
        bool Delete(int id);
        IQueryable<Categoria> List();
    }
}