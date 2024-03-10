namespace GraduationProject.ResponseHandler.Model
{
    public class Response<T>
    {
        public int StatusCode { get; set; }
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public object? Errors { get; set; }
        public int Count { get; set; }
        public T? Data { get; set; }

        public static Response<T> CreateResponse(ResponseType responseType, string? message, object? errors, T? data)
        {
            var statusCode = (int)responseType;

            return new Response<T>
            {
                StatusCode = statusCode,
                Succeeded = responseType == ResponseType.Success || responseType == ResponseType.Created || responseType == ResponseType.NoContent,
                Message = message,
                Errors = errors,
                Count = 0,
                Data = data
            };
        }

        public static Response<T> Success(T data, string? message = null)
        {
            return CreateResponse(ResponseType.Success, message, null, data);
        }

        public static Response<T> Created(string? message = null)
        {
            return CreateResponse(ResponseType.Created, message, null, default(T));
        }

        public static Response<T> Deleted(string? message = null)
        {
            return CreateResponse(ResponseType.NoContent, message, null, default(T));
        }

        public static Response<T> Updated(string? message = null)
        {
            return CreateResponse(ResponseType.Success, message, null, default(T));
        }

        public static Response<T> NoContent(string? message = null)
        {
            return CreateResponse(ResponseType.NoContent, message, null, default(T));
        }

        public static Response<T> BadRequest(string? message = null)
        {
            return CreateResponse(ResponseType.BadRequest, message, null, default(T));
        }

        public static Response<T> ServerError(string? message = null, object? errors = null)
        {
            return CreateResponse(ResponseType.InternalServerError, message, errors, default(T));
        }

        public Response<T> WithCount(int? initialCount = null)
        {
            if (initialCount.HasValue)
            {
                Count = initialCount.Value;
            }
            else if (Data == null)
            {
                Count = 0;
            }
            else if (Data is ICollection<object> collection)
            {
                Count = collection.Count;
            }
            else if (Data is IEnumerable<object> enumerable)
            {
                Count = enumerable.Count();
            }
            else
            {
                Count = 1;
            }

            return this;
        }

    }
}
