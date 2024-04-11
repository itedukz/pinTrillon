namespace ms.MainApi.Core.GeneralHelpers;

public class PaginationReturnModel
{
    public int currentPage { get; set; }
    public int pageSize { get; set; }
    public int totalItems { get; set; }
    public int totalPages { get; set; }
}