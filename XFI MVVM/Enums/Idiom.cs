namespace XFI_MVVM.Enums
{
    using Xamarin.Essentials;

    public class Idiom : Enumeration
    {
        private Idiom(int id, string name)
                 : base(id, name)
        {
        }

        public static Idiom Phone = new(0, nameof(Phone));
        public static Idiom Tablet = new(1, nameof(Tablet));
        public static Idiom Desktop = new(2, nameof(Desktop));
        public static Idiom TV = new(3, nameof(TV));
        public static Idiom Watch = new(4, nameof(Watch));
        
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

            // TODO : change to system default.
            return Phone;
        }
    }
}
