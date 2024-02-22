using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.ResponseHandler.Model
{
    public class Response
    {
        public int statusCode { get; set; }
        //public object meta { get; set; }
        public bool succeeded { get; set; }
        public string? message { get; set; }
        public object? errors { get; set; }
        public object? data { get; set; }

        public Response CreateResponse(int statusCode, bool succeeded, string? message, object? errors, object? data)
        {
            return new Response
            {
                statusCode = statusCode,
                succeeded = succeeded,
                message = message,
                errors = errors,
                data = data
            };
        }
    }
}
