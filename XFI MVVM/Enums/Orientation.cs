namespace XFI_MVVM.Enums
{
    using Xamarin.Essentials;

    /// <summary>
    /// The device orientation types.
    /// </summary>
    public class Orientation : Enumeration
    {
        private Orientation(int id, string name)
                 : base(id, name)
        {
        }

        public static Orientation Portrait = new(0, nameof(Portrait));
        public static Orientation Landscape = new(1, nameof(Landscape));

        /// <summary>
        /// Get the current orientation of the device.
        /// </summary>
        /// <returns>The current orientation as <see cref="Orientation"/> />
        public static Orientation GetOrientation()
        {
            try
            {
                if (DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Portrait)
                {
                    return Portrait;
                }

                if (DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Landscape)
                {
                    return Landscape;
                }
            }
            catch (System.Exception)
            {
                // Swallow exception for unknown.
            }

            // TODO : change to system default.
            return Portrait;
        }
    }
}
