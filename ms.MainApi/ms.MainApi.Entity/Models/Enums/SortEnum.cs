using ms.MainApi.Entity.Models.Dtos;

namespace ms.MainApi.Entity.Models.Enums;

public enum SortEnum
{
    orderByName = 0,
    orderByNameDesc = 1,
    orderByPrice = 2,
    orderByPriceDesc = 3,
    orderByDate = 4,
    orderByDateDesc = 5,
}

public static class SortEnumMethod
{
    public static List<EnumItemDto> List
    {
        get
        {
            List<EnumItemDto> list = new List<EnumItemDto>();
            //list.Add(new EnumItemDto { id = (int)SortEnum.orderByName, name = SortEnum.orderByName.ToString() });
            //list.Add(new EnumItemDto { id = (int)SortEnum.orderByNameDesc, name = SortEnum.orderByNameDesc.ToString() });
            //list.Add(new EnumItemDto { id = (int)SortEnum.orderByPrice, name = SortEnum.orderByPrice.ToString() });
            //list.Add(new EnumItemDto { id = (int)SortEnum.orderByPriceDesc, name = SortEnum.orderByPriceDesc.ToString() });
            //list.Add(new EnumItemDto { id = (int)SortEnum.orderByDate, name = SortEnum.orderByDate.ToString() });
            //list.Add(new EnumItemDto { id = (int)SortEnum.orderByDateDesc, name = SortEnum.orderByDateDesc.ToString() });
            
            list.Add(new EnumItemDto { id = (int)SortEnum.orderByName, name = "От А до Я" });
            list.Add(new EnumItemDto { id = (int)SortEnum.orderByNameDesc, name = "От Я до А" });
            list.Add(new EnumItemDto { id = (int)SortEnum.orderByPrice, name = "По возрастанию цены" });
            list.Add(new EnumItemDto { id = (int)SortEnum.orderByPriceDesc, name = "По убыванию цены" });
            list.Add(new EnumItemDto { id = (int)SortEnum.orderByDate, name = "Сначала новые" });
            list.Add(new EnumItemDto { id = (int)SortEnum.orderByDateDesc, name = "Сначала старые" });

            return list;
        }
    }
}