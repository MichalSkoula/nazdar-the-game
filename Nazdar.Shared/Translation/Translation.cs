using System.Collections.Generic;
using System.Globalization;

namespace Nazdar.Shared.Translation
{
    public static class Translation
    {
        private static Dictionary<string, string> translations = new Dictionary<string, string>();
        private static string currentLanguage = "en";

        public static string CurrentLanguage
        {
            get { return currentLanguage; }
            set
            {
                currentLanguage = value;
                LoadLanguage(value);
            }
        }

        public static void Initialize()
        {
            // Detect system language
            string systemLanguage = DetectSystemLanguage();
            LoadLanguage(systemLanguage);
        }

        private static string DetectSystemLanguage()
        {
            try
            {
                string culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLower();
                // Support Czech and English, default to English
                if (culture == "cs")
                {
                    return "cs";
                }
            }
            catch
            {
                // If detection fails, default to English
            }
            return "en";
        }

        private static void LoadLanguage(string language)
        {
            currentLanguage = language;
            translations.Clear();

            // Load translations from appropriate class
            if (language == "cs")
            {
                translations = TranslationCS.GetTranslations();
            }
            else
            {
                translations = TranslationEN.GetTranslations();
            }
        }

        public static string Get(string key, params object[] args)
        {
            if (translations.TryGetValue(key, out string value))
            {
                if (args.Length > 0)
                {
                    return string.Format(value, args);
                }
                return value;
            }
            return key; // Return the key if translation not found
        }

        public static string GetLanguageName(string langCode)
        {
            switch (langCode)
            {
                case "en":
                    return "English";
                case "cs":
                    return "Cestina";
                default:
                    return langCode;
            }
        }
    }
}
