namespace Jobsity.StockChat.Application.Models.Factories
{
    public static class StockQuoteFactory
    {
        public static StockQuote Create(string csv)
        {
            var lines = csv.Split("\n");
            var columns = lines[1].Split(',');

            if (csv.Contains("N/D"))
                return StockQuote.InvalidSymbol(columns[0]);

            return new StockQuote(columns[0], columns[4]);
        }
    }
}
