namespace ms.MainApi.Entity.Models.Dtos.Measures;

public class MeasureCreateDto
{
    public decimal height { get; set; }  //высота
    public decimal width { get; set; }   //ширина
    public decimal length { get; set; }  //длина
    public int measureType { get; set; } //mm = 1, милиметр;    cm = 2, сантиметр;      m = 3, метр     MeasureEnum
}

public class MeasureShortCreateDto
{
    public decimal height { get; set; }  //высота
    public decimal width { get; set; }   //ширина
    public decimal length { get; set; }  //длина
}