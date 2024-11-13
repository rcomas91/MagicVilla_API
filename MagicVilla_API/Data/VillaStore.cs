using MagicVilla_API.Models.Dto;

namespace MagicVilla_API.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>
        {
         new VillaDto{Id=1,Name="Vista a la piscina",Ocupants=2,MetersCuadrados=300},
                new VillaDto{Id=1,Name="Vista a la playa",Ocupants=3,MetersCuadrados=300}
        };
    }
}
