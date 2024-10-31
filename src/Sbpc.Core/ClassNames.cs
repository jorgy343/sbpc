namespace Sbpc.Core;

public static class ClassNames
{
    public static class Desc
    {
        public const string Cement = "/Game/FactoryGame/Resource/Parts/Cement/Desc_Cement.Desc_Cement_C";
        public const string SteelPlate = "/Game/FactoryGame/Resource/Parts/SteelPlate/Desc_SteelPlate.Desc_SteelPlate_C";
    }

    public static class Recipe
    {
        public static class Foundation
        {
            public const string Asphalt8x2 = "/Game/FactoryGame/Buildable/Building/Foundation/AsphaltSet/Recipe_Foundation_Asphalt_8x2.Recipe_Foundation_Asphalt_8x2_C";
        }

        public static class SS
        {
            public const string EmmisiveBeam2xHalf = "/SS_Mod/Recipes/Beams/Recipe_SS_EmissveBeam_2xHalf.Recipe_SS_EmissveBeam_2xHalf_C"; // Note: Class name is correctly missing an "i".
        }
    }

    public static class Build
    {
        public static class Foundation
        {
            public const string Asphalt8x2 = "/Game/FactoryGame/Buildable/Building/Foundation/AsphaltSet/Build_Foundation_Asphalt_8x2.Build_Foundation_Asphalt_8x2_C";
        }
    }
}