using System.Globalization;

namespace Nazdar.Shared.Translation
{
    public static class Translation
    {
        private static string currentLanguage = "en";

        public static string CurrentLanguage
        {
            get { return currentLanguage; }
            set
            {
                currentLanguage = value == "cs" ? "cs" : "en";
            }
        }

        public static void Initialize()
        {
            TranslationDictionary.Validate();

            // Detect system language
            string systemLanguage = DetectSystemLanguage();
            CurrentLanguage = systemLanguage;
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

        public static string Get(string key, params object[] args)
        {
            if (TranslationDictionary.Values.TryGetValue(key, out var values))
            {
                string value = currentLanguage == "cs" ? values.cs : values.en;
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
