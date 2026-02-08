using System.Globalization;

namespace SchoolProject.Data.Commons
{
    public class GeneralLocalizableEntity
    {
        public string GetLocalized(string textEn, string textAr)
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            if (currentCulture.TwoLetterISOLanguageName.ToLower().Equals("en"))
            {
                return textEn;
            }
            else
            {
                return textAr;
            }
        }
    }
}
