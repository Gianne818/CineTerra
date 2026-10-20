namespace CineTerra.Utilities;

public static class AppRoutes
{
    public const string Home = "/";
    public const string Explore = "/explore";
    public const string Productions = "/productions";
    public const string Pilgrimage = "/pilgrimage";
    public const string Passport = "/passport";

    public static string Location(string id) => $"/locations/{Uri.EscapeDataString(id)}";
    public static string Production(string id) => $"/productions/{Uri.EscapeDataString(id)}";
}
