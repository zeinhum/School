using System.Text.Json;

namespace SchoolResultSystem.Web.Services
{
    public class JsonStorageService<T>
    {
        private readonly string _filePath;

        public JsonStorageService(string filePath)
        {
            _filePath = filePath;

            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
        }

        public List<T> Load()
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }

        public void Save(List<T> items)
        {
            var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        // ✅ optional: alias for compatibility
        public List<T> GetAll()
        {
            return Load();
        }

    }
}
