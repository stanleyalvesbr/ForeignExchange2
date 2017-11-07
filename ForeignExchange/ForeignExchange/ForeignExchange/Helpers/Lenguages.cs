namespace ForeignExchange.Helpers
{
    using Xamarin.Forms;
    using Interfaces;
    using Resources;

    public static class Lenguages
    {
        static Lenguages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept
        {
            get { return Resource.Accept; }
        }

        public static string AmountValidation
        {
            get { return Resource.AmountValidation; }
        }

        public static string Error
        {
            get { return Resource.Error; }
        }

        public static string Title
        {
            get { return Resource.Title; }
        }

        
        public static string NumericValue
        {
            get { return Resource.NumericValue; }
        }
        
        public static string SourceRate
        {
            get { return Resource.SourceRate; }
        }
        
        public static string TargetRate
        {
            get { return Resource.TargetRate; }
        }

        public static string Loading
        {
            get { return Resource.Loading; }
        }

        public static string ResultConvert
        {
            get { return Resource.ResultConvert; }
        }

        public static string Status
        {
            get { return Resource.Status; }
        }

        public static string Amounts
        {
            get { return Resource.Amounts; }
        }

        public static string Amountt
        {
            get { return Resource.Amountt; }
        }

    }
}
