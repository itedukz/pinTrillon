namespace ms.MainApi.Entity.Models.Enums;

public enum ColorEnum
{
    White = 1,  //белый
    Black = 2,  //черный
    Red = 3,    //красный
    Yellow = 4, //желтый
    Orange = 5, //оранжевый
    Green = 6,  //зеленый
    Blue = 7,   //синий
    Purple = 8, //фиолетовый
    Pink = 9,   //розовый
    Brown = 10, //коричневый
    Grey = 11,  //серый
}

public class ColorInfo
{
    public int id { get; set; }
    public string nameRus { get; set; } = $"";
    public string nameEng { get; set; } = $"";
    public string color { get; set; } = $"";
}

public static class ColorEnumMethod
{
    public static List<ColorInfo> List
    {
        get
        {
            List<ColorInfo> list = new List<ColorInfo>();
            list.Add(new ColorInfo { id =  (int)ColorEnum.White, nameEng = ColorEnum.White.ToString(), nameRus = "Белый", color = "#f4f4f4" });
            list.Add(new ColorInfo { id =  (int)ColorEnum.Black, nameEng = ColorEnum.Black.ToString(), nameRus = "Черный", color = "#000000" });
            list.Add(new ColorInfo { id =  (int)ColorEnum.Red, nameEng = ColorEnum.Red.ToString(), nameRus = "Красный", color = "#ff5252 " });
            list.Add(new ColorInfo { id =  (int)ColorEnum.Yellow, nameEng = ColorEnum.Yellow.ToString(), nameRus = "Желтый", color = "#eeee22" });
            list.Add(new ColorInfo { id =  (int)ColorEnum.Orange, nameEng = ColorEnum.Orange.ToString(), nameRus = "Оранжевый", color = "#FFB800" });
            list.Add(new ColorInfo { id =  (int)ColorEnum.Green, nameEng = ColorEnum.Green.ToString(), nameRus = "Зеленый", color = "#52FF00" });
            list.Add(new ColorInfo { id =  (int)ColorEnum.Blue, nameEng = ColorEnum.Blue.ToString(), nameRus = "Синий", color = "#001AFF" });
            list.Add(new ColorInfo { id =  (int)ColorEnum.Purple, nameEng = ColorEnum.Purple.ToString(), nameRus = "Фиолетовый", color = "#800080" });
            list.Add(new ColorInfo { id =  (int)ColorEnum.Pink, nameEng = ColorEnum.Pink.ToString(), nameRus = "Розовый", color = "#ffc0cb" });
            list.Add(new ColorInfo { id =  (int)ColorEnum.Brown, nameEng = ColorEnum.Brown.ToString(), nameRus = "Коричневый", color = "#966f33" });
            list.Add(new ColorInfo { id =  (int)ColorEnum.Grey, nameEng = ColorEnum.Grey.ToString(), nameRus = "Серый", color = "#cccccc" });

            return list;
        }
    }

    public static List<ColorInfo> getByList(List<int>? colorsId)
    {
        if (colorsId == null || colorsId.Count == 0)
            return new List<ColorInfo>();

        return List.Where(i => colorsId.Contains(i.id)).ToList();
    }
}