using BeautySalon.Entities;
using BeautySalon.Repositories;
using System.Text.Json;

namespace BeautySalon.Data
{
    public class SaveToFile
    {
        public static List<string> auditInMemory = new();
        public void AddToFile<T>(T? item) where T : IEntity
        {
            string path = $@"Entities\{item.GetType().Name}.txt";

            var json = JsonSerializer.Serialize(item);
            using (var sw = File.AppendText(path))
            {
                sw.WriteLine(json);
            }
        }

        public void ReadFromFile<T>(IRepository<T> repository, string path) where T : class, IEntity
        {
            using (var reader = File.OpenText(path))
            {
                var line = reader.ReadLine();
                while (!String.IsNullOrEmpty(line))
                {
                    var item = JsonSerializer.Deserialize<T>(line);                    
                    repository.Add(item);
                    line = reader.ReadLine();
                }
                repository.Save();
            }
        }
        
        public static void AuditSaveToMemory<T>(T item, string operation, object itemProperty) where T : IEntity
        {
            DateTime now = DateTime.Now;
            auditInMemory.Add($"{{{now}}}-{{{item.GetType().Name} {operation}}}-{{{itemProperty}}}");

        }

        public void AuditSaveInFile()
        {
            string path = @"Entities\Audit.txt";
            using (var sw = File.AppendText(path))
            {
                foreach(var operation in auditInMemory)
                {
                    sw.WriteLine(operation);
                }
            }
        }
    }
}
