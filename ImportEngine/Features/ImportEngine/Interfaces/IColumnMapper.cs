using ImportEngine.Features.Import.Models;

namespace ImportEngine.Features.Import.Interfaces
{
    public interface IColumnMapper
    {
        // Database se targeted table ka exact schema configuration structure fetch karne ke liye
        Task<Dictionary<string, ColumnSchema>> GetTableSchemaAsync(string tableName);
    }
}
