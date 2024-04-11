using ms.MainApi.Entity.Models.Dtos;

namespace ms.MainApi.Entity.Models.Enums;

public enum ReferenceTypes
{
    product = 1,
    project = 2,
}

public static class ReferenceTypesMethod
{
    public static EnumItemDto toBaseClass(int referenceType)
    {
        return referenceType switch
        {
            (int)ReferenceTypes.product => new EnumItemDto() { id = (int)ReferenceTypes.product, name = ReferenceTypes.product.ToString() },
            (int)ReferenceTypes.project => new EnumItemDto() { id = (int)ReferenceTypes.project, name = ReferenceTypes.project.ToString() },
            
            _ => new EnumItemDto() { id = 0, name = "" }
        };
    }


}