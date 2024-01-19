using AppStore.Models.Domain;

namespace AppStore.Models.DTO
{
    public class CategoriaListDto
    {
        public IQueryable<Categoria>? CategoriaList { get; set; }
        // public int PageSize { get; set; }

        // public int CurrentPage { get; set; }

        // public int TotalPages { get; set; }

        // public string? Term { get; set; }
    }
}