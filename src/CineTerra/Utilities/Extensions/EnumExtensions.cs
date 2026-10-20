using CineTerra.Models.Enums;

namespace CineTerra.Utilities.Extensions;

public static class EnumExtensions
{
    public static string ToFriendlyString(this Difficulty difficulty) => difficulty switch
    {
        Difficulty.EasyWalk => "Easy Walk",
        Difficulty.Moderate => "Moderate Hike",
        Difficulty.Challenging => "Challenging Terrain",
        Difficulty.PermitRequired => "Permit Required",
        _ => difficulty.ToString()
    };

    public static string ToFriendlyString(this Region region) => region switch
    {
        Region.MiddleEast => "Middle East",
        _ => region.ToString()
    };
}
