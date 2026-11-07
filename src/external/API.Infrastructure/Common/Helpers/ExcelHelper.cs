using MiniExcelLibs;
using System.Globalization;

namespace API.Infrastructure.Common.Helpers
{
    public static class ExcelHelper
    {
        public static string? FindMatchingSheet(Stream stream, string sheetName)
        {
            var sheetNames = stream.GetSheetNames();
            stream.Position = 0;
            var cleanTarget = sheetName.Replace(" ", "");
            return sheetNames.FirstOrDefault(s =>
                s.Equals(sheetName, StringComparison.OrdinalIgnoreCase) ||
                s.Replace(" ", "").Equals(cleanTarget, StringComparison.OrdinalIgnoreCase));
        }

        public static (IEnumerable<IDictionary<string, object>> Rows, string? Error) ReadRows(Stream stream, string sheetName)
        {
            try
            {
                var rows = stream.Query(sheetName: sheetName, useHeaderRow: true)
                    .Cast<IDictionary<string, object>>()
                    .ToList();

                return (rows, null);
            }
            catch (Exception ex)
            {
                return ([], ex.Message);
            }
        }

        /// <summary>
        /// Reads and maps rows directly to strongly-typed row models using MiniExcel.
        /// </summary>
        public static (IEnumerable<T> Rows, string? Error) ReadRows<T>(Stream stream, string sheetName) where T : class, new()
        {
            try
            {
                var rows = stream.Query<T>(sheetName: sheetName).ToList();
                return (rows, null);
            }
            catch (Exception ex)
            {
                return ([], ex.Message);
            }
        }
    }

    public static class ExcelRowExtensions
    {
        public static string? GetString(this IDictionary<string, object> row, string columnName)
        {
            if (TryGetValue(row, columnName, out var val) && val is not null)
            {
                var trimmed = val.ToString()?.Trim();
                return string.IsNullOrWhiteSpace(trimmed) ? null : trimmed;
            }

            return null;
        }

        public static decimal? GetDecimal(this IDictionary<string, object> row, string columnName)
        {
            var str = row.GetString(columnName);
            return decimal.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var value) ? value : null;
        }

        public static int? GetInt(this IDictionary<string, object> row, string columnName)
        {
            var str = row.GetString(columnName);
            return int.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var value) ? value : null;
        }

        public static long? GetLong(this IDictionary<string, object> row, string columnName)
        {
            var str = row.GetString(columnName);
            return long.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out var value) ? value : null;
        }

        public static Guid? GetGuid(this IDictionary<string, object> row, string columnName)
        {
            var str = row.GetString(columnName);
            return Guid.TryParse(str, out var value) ? value : null;
        }

        public static bool? GetBool(this IDictionary<string, object> row, string columnName)
        {
            var str = row.GetString(columnName);
            if (string.IsNullOrWhiteSpace(str)) return null;

            if (bool.TryParse(str, out var boolVal)) return boolVal;
            if (str == "1") return true;
            if (str == "0") return false;

            return null;
        }

        public static DateTime? GetDateTime(this IDictionary<string, object> row, string columnName)
        {
            if (TryGetValue(row, columnName, out var val) && val is DateTime dt)
            {
                return dt;
            }

            var str = row.GetString(columnName);
            return DateTime.TryParse(str, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDt) ? parsedDt : null;
        }

        private static bool TryGetValue(IDictionary<string, object> row, string columnName, out object? value)
        {
            if (row.TryGetValue(columnName, out value))
                return true;

            var key = row.Keys.FirstOrDefault(k => k.Trim().Equals(columnName, StringComparison.OrdinalIgnoreCase));
            if (key is not null)
                return row.TryGetValue(key, out value);

            value = null;
            return false;
        }
    }
}
