using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStore.Models.Domain;
using AppStore.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace AppStore.Controllers
{
    public class CategoriaController: Controller
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
           _categoriaService = categoriaService;
        }

         // Acción para mostrar la lista de categorías
    public IActionResult Index()
    {
        var categorias = _categoriaService.List();
        return View(categorias);
    }

    // Acción para mostrar los detalles de una categoría
    public IActionResult Detalles(int id)
    {
        var categoria = _categoriaService.GetById(id);

        if (categoria == null)
        {
            return NotFound(); // Devolver una respuesta HTTP 404 si la categoría no se encuentra
        }

        return View(categoria);
    }

    // Acción para mostrar el formulario de creación de una nueva categoría
    public IActionResult Crear()
    {
        return View();
    }

    // Acción para procesar la creación de una nueva categoría
    [HttpPost]
    public IActionResult Crear(Categoria categoria)
    {
        if (ModelState.IsValid)
        {
            if (_categoriaService.Add(categoria))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Error al agregar la categoría. Por favor, inténtelo de nuevo.");
            }
        }

        return View(categoria);
    }

    // Acción para mostrar el formulario de edición de una categoría existente
    public IActionResult Editar(int id)
    {
        var categoria = _categoriaService.GetById(id);

        if (categoria == null)
        {
            return NotFound();
        }

        return View(categoria);
    }

    // Acción para procesar la edición de una categoría existente
    [HttpPost]
    public IActionResult Editar(int id, Categoria categoria)
    {
        if (id != categoria.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (_categoriaService.Update(categoria))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Error al actualizar la categoría. Por favor, inténtelo de nuevo.");
            }
        }

        return View(categoria);
    }

    // Acción para procesar la eliminación de una categoría
    public IActionResult Eliminar(int id)
    {
        var categoria = _categoriaService.GetById(id);

        if (categoria == null)
        {
            return NotFound();
        }

        return View(categoria);
    }

    [HttpPost, ActionName("Eliminar")]
    public IActionResult ConfirmarEliminar(int id)
    {
        if (_categoriaService.Delete(id))
        {
            return RedirectToAction("Index");
        }
        else
        {
            ModelState.AddModelError("", "Error al eliminar la categoría. Por favor, inténtelo de nuevo.");
            var categoria = _categoriaService.GetById(id);
            return View(categoria);
        }
    }
}
}