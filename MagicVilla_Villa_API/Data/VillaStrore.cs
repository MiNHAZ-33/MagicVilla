﻿using MagicVilla_Villa_API.Models.DTO;

namespace MagicVilla_Villa_API.Data
{
    public static class VillaStrore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>
        {
                new VillaDTO {Id=1, Name="Heaven", Occupancy=2,Sqft=200},
                new VillaDTO {Id=2, Name="Second", Sqft=300, Occupancy=5}
        };
    }
}
