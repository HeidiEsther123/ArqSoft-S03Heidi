using CatalogoApp.Domain.Models;

namespace CatalogoApp.Domain.Interfaces
{
    public interface IReviewRepository
    {
        void Agregar(Review review);
        IEnumerable<Review> ObtenerPorItem(int itemId);
    }
}
