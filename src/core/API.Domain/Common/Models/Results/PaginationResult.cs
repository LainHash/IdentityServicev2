using System.Net;

namespace API.Domain.Common.Models.Results
{
    public class PaginationResult<T> : Result<T>
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int IndexPage { get; set; }
        public int PageSize { get; set; }

        private PaginationResult(T? data, bool isSucceed, string message, HttpStatusCode statusCode)
            : base(data, isSucceed, message, statusCode)
        {
        }

        private PaginationResult(T? data,
            bool isSucceed,
            string message,
            int totalItems,
            int skip,
            int take,
            HttpStatusCode statusCode)
            : base(data, isSucceed, message, statusCode)
        {
            TotalItems = totalItems;
            TotalPages = (int)Math.Ceiling((decimal)totalItems / take);
            IndexPage = skip / take + 1;
            PageSize = take;
        }

        public static PaginationResult<T> Succeed(
            T? data,
            string message,
            int totalItems,
            int skip,
            int take,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new(data, true, message, totalItems, skip, take, statusCode);
        }

        public new static PaginationResult<T> Fail(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new(default, false, message, statusCode);
    }
}
