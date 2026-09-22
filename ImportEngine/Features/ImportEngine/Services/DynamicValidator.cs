using ImportEngine.Features.Import.Interfaces;
using ImportEngine.Features.Import.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ImportEngine.Features.Import.Services
{
    public class DynamicValidator : IDynamicValidator
    {
        public ValidationResult ValidateHeaders(
            List<string> excelHeaders,
            Dictionary<string, ColumnSchema> schema)
        {
            var result = new ValidationError();

            // Check Excel columns
            foreach (var header in excelHeaders)
            {
                if (!schema.ContainsKey(header))
                {
                    result.Errors.Add(
                        $"Excel column '{header}' does not exist in target table.");
                }
            }

            // Check required database columns
            foreach (var column in schema)
            {
                bool existsInExcel = excelHeaders.Any(
                    x => x.Equals(
                        column.Key,
                        StringComparison.OrdinalIgnoreCase));

                if (!existsInExcel && !column.Value.IsNullable)
                {
                    result.Errors.Add(
                        $"Required database column '{column.Key}' is missing from Excel.");
                }
            }

            return result;
        }

        public ValidationResult ValidateRow(
            Dictionary<string, object> row,
            Dictionary<string, ColumnSchema> schema)
        {
            var result = new ValidationError();

            foreach (var column in schema)
            {
                string columnName = column.Key;
                ColumnSchema columnSchema = column.Value;

                bool exists = row.TryGetValue(
                    columnName,
                    out object? value);

                if (!exists)
                {
                    if (!columnSchema.IsNullable)
                    {
                        result.Errors.Add(
                            $"Column '{columnName}' is missing.");
                    }

                    continue;
                }

                // NULL / empty value
                if (IsEmpty(value))
                {
                    if (!columnSchema.IsNullable)
                    {
                        result.Errors.Add(
                            $"Column '{columnName}' cannot be empty.");
                    }

                    continue;
                }

                // Datatype validation
                if (!IsValidDataType(
                        value,
                        columnSchema.DataType))
                {
                    result.Errors.Add(
                        $"Column '{columnName}' contains invalid value '{value}' " +
                        $"for database type '{columnSchema.DataType}'.");
                }
            }

            return result;
        }

        private static bool IsEmpty(object? value)
        {
            if (value == null || value == DBNull.Value)
                return true;

            if (value is string stringValue)
                return string.IsNullOrWhiteSpace(stringValue);

            return false;
        }

        private static bool IsValidDataType(
            object value,
            string dataType)
        {
            string stringValue = value.ToString() ?? string.Empty;

            return dataType.ToLowerInvariant() switch
            {
                "int" => int.TryParse(
                    stringValue,
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out _),

                "bigint" => long.TryParse(
                    stringValue,
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out _),

                "smallint" => short.TryParse(
                    stringValue,
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out _),

                "tinyint" => byte.TryParse(
                    stringValue,
                    NumberStyles.Integer,
                    CultureInfo.InvariantCulture,
                    out _),

                "decimal" => decimal.TryParse(
                    stringValue,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out _),

                "numeric" => decimal.TryParse(
                    stringValue,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out _),

                "float" => double.TryParse(
                    stringValue,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out _),

                "real" => float.TryParse(
                    stringValue,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out _),

                "bit" => IsValidBoolean(stringValue),

                "date" => DateTime.TryParse(
                    stringValue,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out _),

                "datetime" => DateTime.TryParse(
                    stringValue,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out _),

                "datetime2" => DateTime.TryParse(
                    stringValue,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out _),

                "smalldatetime" => DateTime.TryParse(
                    stringValue,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out _),

                "time" => TimeSpan.TryParse(
                    stringValue,
                    CultureInfo.InvariantCulture,
                    out _),

                "uniqueidentifier" => Guid.TryParse(
                    stringValue,
                    out _),

                "varchar" => true,
                "nvarchar" => true,
                "char" => true,
                "nchar" => true,
                "text" => true,
                "ntext" => true,

                _ => true
            };
        }

        private static bool IsValidBoolean(string value)
        {
            return value.Equals("true", StringComparison.OrdinalIgnoreCase)
                || value.Equals("false", StringComparison.OrdinalIgnoreCase)
                || value == "1"
                || value == "0";
        }
    }
}