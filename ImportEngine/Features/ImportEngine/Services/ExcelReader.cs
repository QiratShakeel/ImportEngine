using ImportEngine.Features.Import.Interfaces;
using ExcelDataReader;
using ImportEngine.Features.Import.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace ImportEngine.Features.Import.Services
{
    public class ExcelReader : IExcelReader
    {
        public ExcelReader()
        {
            // ExcelDataReader ko .NET Core mein register karne ke liye yeh line lazmi hai
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public List<string> ReadHeaders(Stream fileStream)
        {
            var headers = new List<string>();

            // OpenReader file ko memory mein load nahi karta, direct stream read karta hai
            using (var reader = ExcelReaderFactory.CreateReader(fileStream))
            {
                if (reader.Read()) // Pehli row par pointer lekar jao
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var headerValue = reader.GetValue(i)?.ToString()?.Trim();
                        if (!string.IsNullOrEmpty(headerValue))
                        {
                            headers.Add(headerValue);
                        }
                    }
                }
            }

            // Stream position ko reset karna zaroori hai taake data read karte waqt pointer phir shuru se chalay
            fileStream.Position = 0;
            return headers;
        }

        public IEnumerable<Dictionary<string, object>> StreamRows(Stream fileStream, List<string> headers)
        {
            using (var reader = ExcelReaderFactory.CreateReader(fileStream))
            {
                // Skip the first row (headers row) kyunke humen sirf data rows chahiye
                if (!reader.Read()) yield break;

                while (reader.Read()) // Line-by-line forward loop [O(1) Memory Footprint]
                {
                    var rowData = new Dictionary<string, object>();
                    bool isRowEmpty = true;

                    for (int i = 0; i < headers.Count; i++)
                    {
                        var cellValue = reader.GetValue(i);
                        rowData[headers[i]] = cellValue;

                        if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            isRowEmpty = false;
                        }
                    }

                    // Agar poori row khali (blank) hai tou ignore karein, warna pipeline mein bhej dein
                    if (!isRowEmpty)
                    {
                        yield return rowData; // yield return data memory save rakhta hai
                    }
                }
            }
        }
    }
}
