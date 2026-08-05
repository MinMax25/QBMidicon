#nullable enable

using System.Globalization;
using System.IO;

namespace QBMidicon.Class.ValueConverters;

public static class FlagImageProvider
{
    public static string? Resolve(string cultureName)
    {
        if (string.IsNullOrWhiteSpace(cultureName)) return null;

        try
        {
            var countryCode = new RegionInfo(cultureName).TwoLetterISORegionName.ToLowerInvariant();
            var builtInPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Resources",
                "Flags",
                $"{countryCode}.png");
            if (File.Exists(builtInPath))
                return builtInPath;

            var packDirectory = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Languages",
                cultureName);
            var packPath = Path.Combine(packDirectory, $"{countryCode}.png");
            if (File.Exists(packPath))
                return packPath;

            return Directory.Exists(packDirectory)
                ? Directory.EnumerateFiles(packDirectory, "*.png", SearchOption.TopDirectoryOnly)
                    .FirstOrDefault()
                : null;
        }
        catch (ArgumentException)
        {
            return null;
        }
    }
}
