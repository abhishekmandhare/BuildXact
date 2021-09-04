using Microsoft.Extensions.Configuration;

namespace buildxact_supplies.CurrencyConverter
{
    public class USDToAUDCurrencyConverter: ICurrencyConverter
    {
        private readonly decimal _rate;

        public USDToAUDCurrencyConverter(IConfiguration config)
        {
            _rate = decimal.Parse(config["audUsdExchangeRate"]);
        }
        public decimal Convert(decimal from)
        {
            return from / _rate;
        }
    }
}
