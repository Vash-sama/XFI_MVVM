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

        public static Orientation Portrait = new Orientation(0, nameof(Portrait));
        public static Orientation Landscape = new Orientation(1, nameof(Landscape));

        /// <summary>
        /// Get the current orientation of the device.
        /// </summary>
        /// <returns>The current orientation as <see cref="Orientation"/> />
        public static Orientation GetOrientation()
        {
            if (DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Portrait)
            {
                return Portrait;
            }

            if (DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Landscape)
            {
                return Landscape;
            }

            throw new Exceptions.OrientationNotSupportedException($"Orientation {DeviceDisplay.MainDisplayInfo.Orientation} is not supported yet.");
        }
    }
}
