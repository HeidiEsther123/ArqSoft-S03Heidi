using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Domain.Models;

namespace CatalogoApp.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _repo;

        public ReviewService(IReviewRepository repo) => _repo = repo;

        public void AgregarReview(Review review) => _repo.Agregar(review);

        public IEnumerable<Review> ObtenerReviewsPorItem(int itemId)
            => _repo.ObtenerPorItem(itemId);
    }
}