using CatalogoApp.Application.Services;
using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoApp.Presentation.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly ItemService _service;
        private readonly IReviewService _reviewService; 

        public CatalogoController(ItemService service, IReviewService reviewService) 
        {
            _service = service;
            _reviewService = reviewService; 
        }

        public IActionResult Index(string? genero)
        {
            var items = string.IsNullOrEmpty(genero)
                ? _service.ObtenerTodos()
                : _service.ObtenerPorGenero(genero);

            ViewBag.Generos = _service.ObtenerGeneros();
            ViewBag.GeneroActual = genero;

            return View(items);
        }

        public IActionResult Detalle(int id)
        {
            var item = _service.ObtenerPorId(id);
            if (item == null) return NotFound();

            ViewBag.Reviews = _reviewService.ObtenerReviewsPorItem(id);
            return View(item);
        }

        public IActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Agregar(Item item)
        {
            _service.Agregar(item);
            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(int id)
        {
            _service.Eliminar(id);
            return RedirectToAction("Index");
        }

        public IActionResult AgregarReview(int itemId)
        {
            var review = new Review { ItemId = itemId };
            return View(review);
        }

        [HttpPost]
        public IActionResult AgregarReview(Review review)
        {
            _reviewService.AgregarReview(review);
            return RedirectToAction("Detalle", new { id = review.ItemId });
        }
    }
}