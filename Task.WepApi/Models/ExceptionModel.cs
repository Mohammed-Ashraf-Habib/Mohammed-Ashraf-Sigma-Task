using System;

namespace Task.WepApi.Models
{
    public class ExceptionModel
    {
        public DateTime? RequestTime { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public int ErrorCode { get; set; }
    }
}
