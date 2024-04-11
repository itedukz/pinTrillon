using ms.MainApi.Entity.Models.Dtos;

namespace ms.MainApi.Entity.Models.Enums;

public enum MeasureEnum
{
    mm = 1, //милиметр
    cm = 2, //сантиметр
    m = 3,  //метр
}

public static class MeasureEnumMethod
{
    public static string toString(int measureType)
    {
        return measureType switch
        {
            (int)MeasureEnum.mm => MeasureEnum.mm.ToString(),
            (int)MeasureEnum.cm => MeasureEnum.cm.ToString(),
            (int)MeasureEnum.m => MeasureEnum.m.ToString(),            
            _ => ""
        };
    }

    public static EnumItemDto toBaseClass(int measureType)
    {
        return measureType switch
        {
            (int)MeasureEnum.mm => new EnumItemDto() { id = (int)MeasureEnum.mm, name = MeasureEnum.mm.ToString() },
            (int)MeasureEnum.cm => new EnumItemDto() { id = (int)MeasureEnum.cm, name = MeasureEnum.cm.ToString() },
            (int)MeasureEnum.m => new EnumItemDto() { id = (int)MeasureEnum.m, name = MeasureEnum.m.ToString() },
            
            _ => new EnumItemDto() { id = 0, name = "" }
        };
    }

    public static List<EnumItemDto> list
    {
        get
        {
            List<EnumItemDto> list = new List<EnumItemDto>();
            list.Add(new EnumItemDto { id = (int)MeasureEnum.mm, name = MeasureEnum.mm.ToString() });
            list.Add(new EnumItemDto { id = (int)MeasureEnum.cm, name = MeasureEnum.cm.ToString() });
            list.Add(new EnumItemDto { id = (int)MeasureEnum.m, name = MeasureEnum.m.ToString() });

            return list;
        }
    }
}