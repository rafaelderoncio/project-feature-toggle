using System;

namespace Project.FeatureToggle.Domain.Responses;

public record PaginationResponse<T> where T : class
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public long TotalRecords { get; init; }
    public int TotalPages { get; init; }
    public int Page { get; init; }
    public int Quantity { get; init; }
    public int? PreviousPage { get; init; }
    public int? NextPage { get; init; }
}
