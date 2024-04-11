using ms.MainApi.Entity.Models.Enums;

namespace ms.MainApi.Entity.Models.Dtos.Measures;

public class MeasureDto
{
    public decimal height { get; set; }             //высота
    public decimal width { get; set; }              //ширина
    public decimal length { get; set; }             //длина
    public EnumItemDto measureType { get; set; }    //mm = 1, милиметр;    cm = 2, сантиметр;      m = 3, метр

    public MeasureDto()
    {
        height = 0;
        width = 0;
        length = 0;
        measureType = MeasureEnumMethod.toBaseClass(0);
    }

    public MeasureDto(decimal _height, decimal _width, decimal _length, int _measureType)
    {
        height = _height;
        width = _width;
        length = _length;
        measureType = MeasureEnumMethod.toBaseClass(_measureType);
    }
}