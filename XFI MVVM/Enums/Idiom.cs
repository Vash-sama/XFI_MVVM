using Xamarin.Essentials;

namespace XFI_MVVM.Enums
{
    public class Idiom : Enumeration
    {
        private Idiom(int id, string name)
                 : base(id, name)
        {
        }

        public static Idiom Phone = new Idiom(0, nameof(Phone));
        public static Idiom Tablet = new Idiom(1, nameof(Tablet));
        public static Idiom Desktop = new Idiom(2, nameof(Desktop));
        public static Idiom TV = new Idiom(3, nameof(TV));
        public static Idiom Watch = new Idiom(4, nameof(Watch));

        public static Idiom GetIdiom()
        {
            if (DeviceInfo.Idiom == DeviceIdiom.Phone)
            {
                return Phone;
            }

            if (DeviceInfo.Idiom == DeviceIdiom.Tablet)
            {
                return Tablet;
            }

            if (DeviceInfo.Idiom == DeviceIdiom.Desktop)
            {
                return Desktop;
            }

            if (DeviceInfo.Idiom == DeviceIdiom.TV)
            {
                return TV;
            }

            if (DeviceInfo.Idiom == DeviceIdiom.Watch)
            {
                return Watch;
            }

            throw new Exceptions.IdiomNotSupportedException($"Idiom {DeviceInfo.Idiom} is not supported yet.");
        }
    }
}
