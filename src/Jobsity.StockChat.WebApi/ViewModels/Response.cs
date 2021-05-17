namespace Jobsity.StockChat.WebApi.ViewModels
{
    public class Response
    {
        public Response(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }

    public class Response<T>
    {
        public Response(T data)
        {
            Data = data;
        }

        public T Data { get; }
    }
}
