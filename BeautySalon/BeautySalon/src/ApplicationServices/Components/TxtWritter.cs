using BeautySalon.DataAcces.Data.Entities;
using System.Text.Json;

namespace BeautySalon.ApplicationServices.Components
{
    public class TxtWritter : ITxtWritter
    {
        public static List<string> auditInMemory = new();
        public void AddItemToFile<T>(T? item) where T : IEntity
        {
            string path = $@"Entities\{item.GetType().Name}.txt";

            var json = JsonSerializer.Serialize(item);
            using (var sw = File.AppendText(path))
            {
                sw.WriteLine(json);
            }
        }

        public void AuditSaveInMemory<T>(T item, string operation, object itemProperty) where T : IEntity
        {
            DateTime now = DateTime.Now;
            auditInMemory.Add($"{{{now}}}-{{{item.GetType().Name} {operation}}}-{{{itemProperty}}}");

        }

        public void AuditSaveInFileFromList()
        {
            string path = @"Entities\Audit.txt";
            using (var sw = File.AppendText(path))
            {
                foreach (var operation in auditInMemory)
                {
                    sw.WriteLine(operation);
                }
            }
        }
        public void AuditSaveInFile<T>(T item, string operation, object itemProperty) where T : IEntity
        {
            string path = @"Entities\Audit.txt";
            DateTime now = DateTime.Now;

            using (var sw = File.AppendText(path))
            {
                sw.WriteLine($"{{{now}}}-{{{item.GetType().Name} {operation}}}-{{{itemProperty}}}");
            }
        }
    }
}
