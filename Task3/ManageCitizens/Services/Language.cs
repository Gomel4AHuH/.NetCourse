using System.Windows;

namespace ManageCitizens.Services
{
    class Language
    {
        public static void SetLanguage(string language)
        {
            ResourceDictionary dictionary = [];

            dictionary.Source = language switch
            {
                "en" => new Uri("..\\Languages\\Dictionary-en-US.xaml", UriKind.Relative),
                "ru" => new Uri("..\\Languages\\Dictionary-ru-RU.xaml", UriKind.Relative),
                _ => new Uri("..\\Languages\\Dictionary-en-US.xaml", UriKind.Relative),
            };
                        
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }        
    }
}
