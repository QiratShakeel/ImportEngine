using ImportEngine.Features.Import.Models;
using System.ComponentModel.DataAnnotations;

namespace ImportEngine.Features.Import.Interfaces
{
    public interface IDynamicValidator
    {
        ValidationResult ValidateHeaders(
            List<string> excelHeaders,
            Dictionary<string, ColumnSchema> schema);

        ValidationResult ValidateRow(
            Dictionary<string, object> row,
            Dictionary<string, ColumnSchema> schema);
    }
}