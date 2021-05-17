using Jobsity.StockChat.Application.Constants;

namespace Jobsity.StockChat.Application.Models
{
    public class StockQuote
    {
        public string Symbol { get; }
        public string Quote { get; }
        public string OutputMessage { get; private set; }

        public static StockQuote InvalidSymbol(string symbol)
        {
            return new StockQuote(symbol.ToUpper());
        }

        public StockQuote(string symbol, string quote)
        {
            Symbol = symbol;
            Quote = quote;
            OutputMessage = $"{Symbol.ToUpper()} quote is ${Quote} per share";
        }

        private StockQuote(string symbol)
        {
            Symbol = symbol;
            OutputMessage = string.Format(ErrorMessages.InvalidSymbol, symbol);
        }
    }
}
