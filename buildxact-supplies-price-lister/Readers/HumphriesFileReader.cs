using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace buildxact_supplies.Readers
{
    public class HumphriesData
    {
        [Name("identifier")]
        public Guid Id { get; set; }
        [Name("desc")]
        public string Description { get; set; }
        [Name("unit")]
        public string Unit { get; set; }
        [Name("costAUD")]
        public decimal Price { get; set; }
    }
    public class HumphriesFileReader : ISuppliesFileReader
    {
        public async Task<IEnumerable<SuppliesData>> GetSuppliesDataAsync(string filePath)
        {
            List<SuppliesData> result = new List<SuppliesData>();
            using TextReader fileReader = File.OpenText(filePath);
            {
                var reader = new CsvReader(fileReader, new CsvConfiguration(CultureInfo.InvariantCulture));
                result = await reader.GetRecordsAsync<HumphriesData>()
                    .Select(Convert)
                    .ToListAsync();
            }
            return result;

        }

        public SuppliesData Convert(HumphriesData data)
        {
            return new SuppliesData()
            {
                Description = data.Description,
                Id = data.Id.ToString(),
                Price = data.Price
            };
        }

    }
}
