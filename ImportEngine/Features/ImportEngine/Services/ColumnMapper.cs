using ImportEngine.Features.Import.Interfaces;
using ImportEngine.Features.Import.Models;
using Microsoft.Data.SqlClient;

namespace ImportEngine.Features.Import.Services
{
    public class ColumnMapper : IColumnMapper
    {
        private readonly string _connectionString;

        public ColumnMapper(string connectionString)
        {
            _connectionString =
                connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<Dictionary<string, ColumnSchema>> GetTableSchemaAsync(
            string tableName)
        {
            var schema = new Dictionary<string, ColumnSchema>(
                StringComparer.OrdinalIgnoreCase);

            if (string.IsNullOrWhiteSpace(tableName) ||
                tableName.Contains(" "))
            {
                throw new ArgumentException(
                    "Invalid target table configuration detected.");
            }

            const string query = @"
                SELECT 
                    COLUMN_NAME,
                    DATA_TYPE,
                    IS_NULLABLE
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_NAME = @TableName;
            ";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TableName", tableName);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                string columnName = reader.GetString(0);
                string dataType = reader.GetString(1);
                string nullable = reader.GetString(2);

                schema[columnName] = new ColumnSchema
                {
                    DataType = dataType,
                    IsNullable = nullable.Equals(
                        "YES",
                        StringComparison.OrdinalIgnoreCase)
                };
            }

            return schema;
        }
    }
}