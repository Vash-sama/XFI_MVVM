
using Xamarin.Essentials;

namespace XFI_MVVM.Enums
{
    public class Orientation : Enumeration
    {
        private Orientation(int id, string name)
                 : base(id, name)
        {
        }

        public static Orientation Portrait = new Orientation(0, nameof(Portrait));
        public static Orientation Landscape = new Orientation(1, nameof(Landscape));

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
