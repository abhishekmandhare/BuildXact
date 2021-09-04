using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace buildxact_supplies.Readers
{
    public class SuppliesData
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
    interface ISuppliesFileReader
    {

        Task<IEnumerable<SuppliesData>> GetSuppliesDataAsync(string filePath);
    }
}
