using System.Net;

namespace TaskManagementSystem.Models.DTOs
{
    public class ResponseAPI
    {
        public bool Status {  get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public dynamic Data { get; set; }
        public List<string> Errors { get; set; }
    }
}
