using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace buildxact_supplies.Readers
{
    class MegacorpData
    {
        public List<PartnerItems> Partners { get; set; }
        public class PartnerItems
        {
            public string Name { get; set; }
            public string PartnerType { get; set; }
            public string PartnerAddress { get; set; }
            public List<SuppliesItem> Supplies { get; set; }
            public class SuppliesItem
            {
                public int Id { get; set; }
                public string Description { get; set; }
                public string Uom { get; set; }
                public int PriceInCents { get; set; }
                public Guid ProviderId { get; set; }
                public string MaterialType { get; set; }
            }
        }
    }

    public class MegaCorpFileReader : ISuppliesFileReader
    {

        public async Task<IEnumerable<SuppliesData>> GetSuppliesDataAsync(string filePath)
        {
            var result = new List<SuppliesData>();

            using TextReader fileReader = File.OpenText(filePath);
            {
                string jsonText = await fileReader.ReadToEndAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                var MegaCorpData = JsonSerializer.Deserialize<MegacorpData>(jsonText, options);
                foreach(var partnerItem in MegaCorpData.Partners)
                {
                    foreach(var supplies in partnerItem.Supplies)
                    {
                        result.Add(Convert(supplies));
                    }
                }
            }

            return result;
        }

        private SuppliesData Convert(MegacorpData.PartnerItems.SuppliesItem data)
        {
            return new SuppliesData()
            {
                Description = data.Description,
                Id = data.Id.ToString(),
                Price = data.PriceInCents
            };
        }
    }
}
