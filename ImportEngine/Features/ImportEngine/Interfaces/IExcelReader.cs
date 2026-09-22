using System.Collections.Generic;
using System.IO;

namespace ImportEngine.Features.Import.Interfaces
{
    public interface IExcelReader
    {
        // Excel file ki pehli row se columns ke names (Headers) read karne ke liye
        List<string> ReadHeaders(Stream fileStream);

        // Pura data line-by-line forward-only stream karne ke liye
        IEnumerable<Dictionary<string, object>> StreamRows(Stream fileStream, List<string> headers);
    }
}
