namespace ms.MainApi.Core.GeneralHelpers;


public interface IPaginationRepository
{
    PaginationReturnModel GetPagination(int totalItems, int page, int pageSize);
}

public class PaginationRepository : IPaginationRepository
{
    public PaginationReturnModel GetPagination(int totalItems, int page, int pageSize) 
        => new PaginationReturnModel{
        currentPage = page,
        pageSize = pageSize,
        totalItems = totalItems,
        totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize)
    };
}