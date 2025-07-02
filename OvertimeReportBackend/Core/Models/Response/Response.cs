using System.Net;

namespace Core.Models.Response
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public List<string>? Errors { get; set; } = [];
        public T? Data { get; set; } = default;
        public HttpStatusCode StatusCode { get; set; }

    }

}