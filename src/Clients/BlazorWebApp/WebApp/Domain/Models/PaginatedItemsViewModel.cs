namespace WebApp.Domain.Models;

public class PaginatedItemsViewModel<TEnity> where TEnity : class
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public long Count { get; set; }
    public IEnumerable<TEnity> Data { get; set; }

    public PaginatedItemsViewModel(int pageIndex, int pageSize, long count, IEnumerable<TEnity> data)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Count = count;
        Data = data;
    }

    public PaginatedItemsViewModel()
    {
        
    }
}