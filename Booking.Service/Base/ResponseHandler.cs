using Booking.Data.Entities;

namespace Restaurant.Service
{
    public class ResponseHandler
    {

        public ResponseHandler()
        {

        }
        public Response<T> Deleted<T>()
        {
            return new Response<T>()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = "Deleted Successfully"
            };
        }
        public Response<T> Success<T>(T entity, string message = "Successfully", object Meta = null)
        {
            return new Response<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.OK,
                Succeeded = true,
                Message = message,
                Meta = Meta,

            };
        }
        public Response<T> Unauthorized<T>(string message = "Unauthorized")
        {
            return new Response<T>()
            {
                StatusCode = System.Net.HttpStatusCode.Unauthorized,
                Succeeded = true,
                Message = message
            };
        }
        public Response<T> BadRequest<T>(string message = "Bad Request")
        {
            return new Response<T>()
            {
                StatusCode = System.Net.HttpStatusCode.BadRequest,
                Succeeded = false,
                Message = message
            };
        }

        public Response<T> NotFound<T>(string message = null)
        {
            return new Response<T>()
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Succeeded = false,
                Message = message == null ? "Not Found" : message
            };
        }

        public Response<T> Created<T>(T entity, object Meta = null)
        {
            return new Response<T>()
            {
                Data = entity,
                StatusCode = System.Net.HttpStatusCode.Created,
                Succeeded = true,
                Message = "Created",
                Meta = Meta
            };
        }
        public Response<T> Forbidden<T>(string message = "Forbidden")
        => new Response<T>() { StatusCode = System.Net.HttpStatusCode.Forbidden, Succeeded = false, Message = message };

        public Response<T> Conflict<T>(string message)
       => new Response<T>() { StatusCode = System.Net.HttpStatusCode.Conflict, Succeeded = false, Message = message };

        internal Response<(IReadOnlyList<Reservation>, int Count)> Success<T>((Task<IReadOnlyList<Reservation>> reservation, Task<int> Count) value)
        {
            throw new NotImplementedException();
        }
    }
}
