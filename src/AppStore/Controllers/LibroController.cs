
using AppStore.Models.Domain;
using AppStore.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace AppStore.Controllers
{
   [Authorize]
    public class LibroController : Controller
    {
        private readonly ILibroService _libroService;
        private readonly IFileService _fileservice;
        private readonly ICategoriaService _categoriaService;
        public LibroController(ILibroService libroService, IFileService fileservice, ICategoriaService categoriaService)
        {
            _libroService = libroService;
            _categoriaService = categoriaService;
            _fileservice = fileservice;
        }

        [HttpPost]
        public IActionResult Add(Libro libro)
        {  libro.CategoriasList = _categoriaService.List()
        .Select( x => new SelectListItem{Text = x.Nombre, Value = x.Id.ToString()});


            if(!ModelState.IsValid)
            {
                return View(libro);
            }


            if(libro.ImageFile != null)
            {
              var resultado =  _fileservice.SaveImage(libro.ImageFile);

              if(resultado.Item1 == 0)
              {
                TempData["msg"] = "La imagen no pudo guardarse exitosamente";
                return View(libro);
              }

              var imagenName = resultado.Item2;
              libro.Imagen = imagenName;
            }

            var resultadoLibro = _libroService.Add(libro);
            if(resultadoLibro)
            {
                TempData["msg"] = "Se agrego el libro correctamente";
                return RedirectToAction(nameof(Add));
            }
             
              TempData["msg"] = "Errores guardando el libro";
              return View(libro);
         
        }
      
        public IActionResult Add()
        {
           
           var libro = new Libro();
           libro.CategoriasList = _categoriaService.List()
           .Select(x => new SelectListItem{Text = x.Nombre, Value = x.Id.ToString()});
            return View(libro);
        }
          public IActionResult Edit(int id)
        {
           var libro = _libroService.GetById(id);
            var categoriasDelLibro = _libroService.GetCatecoriaByLibroId(id);
           var multiSelectListCategorias = new MultiSelectList(_categoriaService.List(), "Id", "Nombre", categoriasDelLibro);
           libro.MultiCategoriasList = multiSelectListCategorias;
           
            return View(libro);
        }


        [HttpPost]
        public IActionResult Edit(Libro libro)
        {
             var categoriasDelLibro = _libroService.GetCatecoriaByLibroId(libro.Id);
              var multiSelectListCategorias = new MultiSelectList(_categoriaService.List(), "Id", "Nombre", categoriasDelLibro);
           libro.MultiCategoriasList = multiSelectListCategorias;

           if(!ModelState.IsValid)
           {
            return View(libro);
           }

           if(libro.ImageFile != null)
           {
            var fileResultado = _fileservice.SaveImage(libro.ImageFile);
            if(fileResultado.Item1 == 0)
            {
              TempData["msg"]= "La imagen no fue guardada";
              return View(libro);
            }
            var imagenName = fileResultado.Item2;
            libro.Imagen = imagenName;
           }

           var resultadoLibro = _libroService.Update(libro);
           if(!resultadoLibro)
           {
             TempData["msg"]= "Errores, no se pudo actualizar el libro";
             return View(libro);
           }
            TempData["msg"]= "Se actualizo exitosamente el libro";
             return View(libro);
           
        }


          public IActionResult LibroList()
        {
            var libros = _libroService.List();
            return View(libros);
        }

        public IActionResult Delete(int id)
        {
           _libroService.Delete(id);
            return RedirectToAction(nameof(LibroList));
        }

    }
}