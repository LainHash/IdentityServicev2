namespace API.Domain.Common.Models
{
    public abstract record PaginationQuery
    {
        public string? Keyword { get; init; }
        public string SortField { get; set; } = "created_at";
        public bool IsAscending { get; set; } = true;
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 12;
        public DateTime From { get; init; } = DateTime.UtcNow.AddDays(-30);
        public DateTime To { get; init; } = DateTime.UtcNow;
    }
}
