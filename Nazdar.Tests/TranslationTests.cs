using Nazdar.Shared.Translation;

namespace Nazdar.Tests
{
    public class TranslationTests
    {
        [Fact]
        public void DictionaryIsValid()
        {
            TranslationDictionary.Validate();
        }

        [Fact]
        public void EveryKeyIsTranslatedToBothLanguages()
        {
            foreach (var item in TranslationDictionary.Values)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Value.cs), "Missing Czech text for " + item.Key);
                Assert.False(string.IsNullOrWhiteSpace(item.Value.en), "Missing English text for " + item.Key);
            }
        }

        [Fact]
        public void UnknownKeyFallsBackToKey()
        {
            Assert.Equal("no.such.key", Translation.Get("no.such.key"));
        }

        [Fact]
        public void UnsupportedLanguageFallsBackToEnglish()
        {
            Translation.CurrentLanguage = "de";
            Assert.Equal("en", Translation.CurrentLanguage);
        }

        [Fact]
        public void SwitchesLanguage()
        {
            try
            {
                Translation.CurrentLanguage = "cs";
                Assert.Equal("Den ", Translation.Get("stats.day"));
            }
            finally
            {
                Translation.CurrentLanguage = "en";
            }

            Assert.Equal("Day ", Translation.Get("stats.day"));
        }
    }
}
