using buildxact_supplies;
using buildxact_supplies.CurrencyConverter;
using buildxact_supplies.Readers;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Unity;

namespace SuppliesPriceLister
{
    class Program
    {
        static void Main()
        {
            try
            {
                IUnityContainer container = new UnityContainer();
                ConfigureDependancies(container);

                new Application(container, container.Resolve<IConfiguration>()).Run().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
            }
        }

        public static void ConfigureDependancies(IUnityContainer container)
        {
            var cb = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false);

            container.RegisterType<ICurrencyConverter, USDToAUDCurrencyConverter>();
            container.RegisterType<ISuppliesFileReader, HumphriesFileReader>(Constant.HumphriesReaderName);
            container.RegisterType<ISuppliesFileReader, MegaCorpFileReader>(Constant.MegacorpReaderName);
            container.RegisterInstance<IConfiguration>(cb.Build());
        }
    }
}
