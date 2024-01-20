
using AppStore.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;


namespace AppStore.Controllers;

public class HomeController : Controller
{
    private readonly ILibroService _libroService;

    public HomeController(ILibroService libroService)
    {
      _libroService = libroService;
    }

    public IActionResult Index(string term = "", int pagina = 1)
    {
         Console.WriteLine($"Término de búsqueda: {term}");
        var libros = _libroService.Listar(term, true,pagina,5);
        return View(libros);
    }
 
 public IActionResult LibroDetail(int libroId)
 {
    var libro = _libroService.GetById(libroId);

    return View(libro);
 }

 public IActionResult About()
 {
    return View();
 }

}
