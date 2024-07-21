using System.ComponentModel;
using System.Runtime.CompilerServices;
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

            /*switch (language)
            {
                case "en":
                    dictionary.Source = new Uri("..\\Languages\\Dictionary-en-US.xaml", UriKind.Relative);
                    SetEnLanguageLock = false;
                    SetRuLanguageLock = true;
                    break;
                case "ru":
                    dictionary.Source = new Uri("..\\Languages\\Dictionary-ru-RU.xaml", UriKind.Relative);
                    SetEnLanguageLock = true;
                    SetRuLanguageLock = false;
                    break;
                default:
                    dictionary.Source = new Uri("..\\Languages\\Dictionary-en-US.xaml", UriKind.Relative);
                    SetEnLanguageLock = false;
                    SetRuLanguageLock = true;
                    break;
            }*/
            Application.Current.Resources.MergedDictionaries.Add(dictionary);
        }
        /*
        private static bool _setEnLanguageLock;
        public static bool SetEnLanguageLock
        {
            get => _setEnLanguageLock;
            set
            {
                _setEnLanguageLock = value;
                OnPropertyChanged(nameof(SetEnLanguageLock));
            }
        }

        private static bool _setRuLanguageLock;
        public static bool SetRuLanguageLock
        {
            get => _setRuLanguageLock;
            set
            {
                _setRuLanguageLock = value;
                OnPropertyChanged(nameof(SetRuLanguageLock));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }*/
    }
}
