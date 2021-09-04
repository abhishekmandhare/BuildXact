using buildxact_supplies.Readers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity;
using System.IO;
using CsvHelper;
using System.Globalization;
using buildxact_supplies.CurrencyConverter;
using Microsoft.Extensions.Configuration;
using CsvHelper.Configuration;

namespace buildxact_supplies
{
    public sealed class SuppliesDataMapper : ClassMap<SuppliesData>
    {
        public SuppliesDataMapper()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.Price).Convert(
                d => d.Value.Price.ToString("C2", new CultureInfo("EN-au")));
        }
    }
    public class Application
    {
        private readonly IUnityContainer _container;
        private readonly IConfiguration _config;


        public async Task Run()
        {
            var data = await ReadDataFromFiles();

            await WriteOutput(data);
        }

        public Application(IUnityContainer container, IConfiguration config)
        {
            _container = container;
            _config = config;
        }

        public async Task<List<SuppliesData>> ReadDataFromFiles()
        {
            var data = new List<SuppliesData>();

            var humphriesReader = _container.Resolve<ISuppliesFileReader>(Constant.HumphriesReaderName);
            data.AddRange(await humphriesReader.GetSuppliesDataAsync(_config["HumphriesFileName"]));

            var meaCorpReader = _container.Resolve<ISuppliesFileReader>(Constant.MegacorpReaderName);
            data.AddRange(await meaCorpReader.GetSuppliesDataAsync(_config["MegacorpFileName"]));

            data.Sort((first, second) => second.Price.CompareTo(first.Price));
            return data;
        }

        public async Task WriteOutput(List<SuppliesData> data)
        {
            using (var csv = new CsvWriter(Console.Out, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<SuppliesDataMapper>();
                await csv.WriteRecordsAsync(data);
            }
        }
    }
}
