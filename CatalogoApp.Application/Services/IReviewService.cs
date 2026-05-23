using CatalogoApp.Domain.Models;

namespace CatalogoApp.Application.Services
{
    public interface IReviewService
    {
        void AgregarReview(Review review);
        IEnumerable<Review> ObtenerReviewsPorItem(int itemId);
    }
}