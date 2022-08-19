
using BeautySalon.DataAcces.Data.Entities;

namespace BeautySalon.ApplicationServices.Components
{
    public interface ITxtWritter
    {
        public static List<string> auditInMemory = new();
        public void AddItemToFile<T>(T? item) where T : IEntity;
        public void AuditSaveInMemory<T>(T item, string operation, object itemProperty) where T : IEntity;
        public void AuditSaveInFileFromList();
        public void AuditSaveInFile<T>(T item, string operation, object itemProperty) where T : IEntity;

    }
}
