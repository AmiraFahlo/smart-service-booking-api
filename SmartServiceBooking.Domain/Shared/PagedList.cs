using System;
using System.Collections.Generic;

namespace SmartServiceBooking.Domain.Shared;

public class PagedList<T>
{
    private PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = count;
        CurrentPage = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }

    public List<T> Items { get; }
    public int TotalCount { get; }
    public int CurrentPage { get; }
    public int TotalPages { get; }
    public int PageSize { get; }

    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;

    public static PagedList<T> Create(List<T> items, int count, int pageNumber, int pageSize)
    {
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}
