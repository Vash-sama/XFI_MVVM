namespace XFI_MVVM.Core
{
    using XFI_MVVM.Enums;

    internal static class Defaults
    {
        public static bool AllowMultiple { get; set; } = true;

        public static bool ReplaceInstance { get; set; } = false;

        public static bool IsModal { get; set; } = false;

        public static Idiom Idiom { get; set; } = Idiom.Phone;

        public static Idiom IdiomOverride { get; set; } = null;

        public static Orientation Orientation { get; set; } = Orientation.Portrait;

        public static bool HandleOrientationChange { get; set; } = true;

        public static bool TryToKeepViewModelOnOrientationChange { get; set; } = true;
    }
}
