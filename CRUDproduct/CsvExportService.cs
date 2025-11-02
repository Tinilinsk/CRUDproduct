using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUDproduct.Models;

namespace CRUDproduct
{

    public class CsvExportService
    {
        public async Task<bool> ExportToCsvAsync(IEnumerable<Product> products)
        {
            try
            {
                if (products == null || !products.Any())
                {
                    return false;
                }

                var csvContent = GenerateCsvContent(products);
                var fileName = $"products_export_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                return await SaveCsvFile(csvContent, fileName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Export failed: {ex.Message}");
                return false;
            }
        }

        public string GenerateCsvContent(IEnumerable<Product> products)
        {
            var csv = new StringBuilder();

            csv.AppendLine("ID,Name,Price,Category,CreatedDate");

            foreach (var product in products)
            {
                var line = string.Join(",",
                    product.Id,
                    EscapeCsvField(product.Name),
                    product.Price.ToString("F2", CultureInfo.InvariantCulture),
                    EscapeCsvField(product.Category),
                    product.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss")
                );
                csv.AppendLine(line);
            }

            return csv.ToString();
        }

        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field)) return "";

            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n") || field.Contains("\r"))
            {
                return $"\"{field.Replace("\"", "\"\"")}\"";
            }

            return field;
        }

        private async Task<bool> SaveCsvFile(string csvContent, string fileName)
        {
            try
            {
                var folderResult = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Select folder to save CSV file"
                });

                if (folderResult != null)
                {
                    var folderPath = Path.GetDirectoryName(folderResult.FullPath);
                    var fullPath = Path.Combine(folderPath, fileName);

                    await File.WriteAllTextAsync(fullPath, csvContent, Encoding.UTF8);

                    await Application.Current.MainPage.DisplayAlert("Success",
                        $"File saved to:\n{fullPath}", "OK");
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"File save failed: {ex.Message}");
                return false;
            }
        }

        public async Task<string> GetCsvAsStringAsync(IEnumerable<Product> products)
        {
            return await Task.Run(() => GenerateCsvContent(products));
        }
    }
}
