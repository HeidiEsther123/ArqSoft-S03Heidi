using System.Text.Json;
using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Domain.Models;

namespace CatalogoApp.Infrastructure.Repositories
{
    public class JsonReviewRepository : IReviewRepository
    {
        private readonly string _filePath;

        public JsonReviewRepository(string filePath)
        {
            _filePath = filePath;

            var carpeta = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(carpeta))
                Directory.CreateDirectory(carpeta);
        }

        public IEnumerable<Review> ObtenerPorItem(int itemId)
            => ObtenerTodos().Where(r => r.ItemId == itemId);

        public void Agregar(Review review)
        {
            var reviews = ObtenerTodos();

            review.Id = reviews.Count > 0
                        ? reviews.Max(r => r.Id) + 1
                        : 1;

            reviews.Add(review);
            Guardar(reviews);
        }

        private List<Review> ObtenerTodos()
        {
            if (!File.Exists(_filePath))
                return new List<Review>();

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Review>>(json)
                   ?? new List<Review>();
        }

        private void Guardar(List<Review> reviews)
        {
            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(reviews, opciones);
            File.WriteAllText(_filePath, json);
        }
    }
}