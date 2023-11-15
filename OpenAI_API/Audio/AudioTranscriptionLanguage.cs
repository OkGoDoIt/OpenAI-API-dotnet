using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// ISO-639-1 Language codes supported by Whisper-1 model for transcription
    /// </summary>
    public class AudioTranscriptionLanguage
    {
        private AudioTranscriptionLanguage(string value) { Value = value; }

        private string Value { get; set; }

        /// <summary>
        /// Set language to English
        /// </summary>
        public static AudioTranscriptionLanguage English { get { return new AudioTranscriptionLanguage("en"); } }

        /// <summary>
        /// Set language to Afrikaans
        /// </summary>
        public static AudioTranscriptionLanguage Afrikaans { get { return new AudioTranscriptionLanguage("af"); } }

        /// <summary>
        /// Set language to Arabic
        /// </summary>
        public static AudioTranscriptionLanguage Arabic { get { return new AudioTranscriptionLanguage("ar"); } }    

        /// <summary>
        /// Set language to Armenian
        /// </summary>
        public static AudioTranscriptionLanguage Armenian { get { return new AudioTranscriptionLanguage("hy"); } }

        /// <summary>
        /// Set language to Azerbaijani
        /// </summary>
        public static AudioTranscriptionLanguage Azerbaijani { get { return new AudioTranscriptionLanguage("az"); } }

        /// <summary>
        /// Set language to Belarusian
        /// </summary>
        public static AudioTranscriptionLanguage Belarusian { get { return new AudioTranscriptionLanguage("be"); } }

        /// <summary>
        /// Set language to Bosnian
        /// </summary>
        public static AudioTranscriptionLanguage Bosnian { get { return new AudioTranscriptionLanguage("bs"); } }

        /// <summary>
        /// Set language to Bulgarian
        /// </summary>
        public static AudioTranscriptionLanguage Bulgarian { get { return new AudioTranscriptionLanguage("bg"); } }

        /// <summary>
        /// Set language to Catalan
        /// </summary>
        public static AudioTranscriptionLanguage Catalan { get { return new AudioTranscriptionLanguage("ca"); } }

        /// <summary>
        /// Set language to Chinese
        /// </summary>
        public static AudioTranscriptionLanguage Chinese { get { return new AudioTranscriptionLanguage("zh"); } }

        /// <summary>
        /// Set language to Croatian
        /// </summary>
        public static AudioTranscriptionLanguage Croatian { get { return new AudioTranscriptionLanguage("hr"); } }

        /// <summary>
        /// Set language to Czech
        /// </summary>
        public static AudioTranscriptionLanguage Czech { get { return new AudioTranscriptionLanguage("cs"); } }

        /// <summary>
        /// Set language to Danish
        /// </summary>
        public static AudioTranscriptionLanguage Danish { get { return new AudioTranscriptionLanguage("da"); } }

        /// <summary>
        /// Set language to Dutch
        /// </summary>
        public static AudioTranscriptionLanguage Dutch { get { return new AudioTranscriptionLanguage("nl"); } }

        /// <summary>
        /// Set language to Estonian
        /// </summary>
        public static AudioTranscriptionLanguage Estonian { get { return new AudioTranscriptionLanguage("et"); } }

        /// <summary>
        /// Set language to Finnish
        /// </summary>
        public static AudioTranscriptionLanguage Finnish { get { return new AudioTranscriptionLanguage("fi"); } }

        /// <summary>
        /// Set language to French
        /// </summary>
        public static AudioTranscriptionLanguage French { get { return new AudioTranscriptionLanguage("fr"); } }

        /// <summary>
        /// Set language to Galician
        /// </summary>
        public static AudioTranscriptionLanguage Galician { get { return new AudioTranscriptionLanguage("gl"); } }

        /// <summary>
        /// Set language to German
        /// </summary>
        public static AudioTranscriptionLanguage German { get { return new AudioTranscriptionLanguage("de"); } }

        /// <summary>
        /// Set language to Greek
        /// </summary>
        public static AudioTranscriptionLanguage Greek { get { return new AudioTranscriptionLanguage("el"); } }

        /// <summary>
        /// Set language to Hebrew
        /// </summary>
        public static AudioTranscriptionLanguage Hebrew { get { return new AudioTranscriptionLanguage("he"); } }

        /// <summary>
        /// Set language to Hindi
        /// </summary>
        public static AudioTranscriptionLanguage Hindi { get { return new AudioTranscriptionLanguage("hi"); } }

        /// <summary>
        /// Set language to Hungarian
        /// </summary>
        public static AudioTranscriptionLanguage Hungarian { get { return new AudioTranscriptionLanguage("hu"); } }

        /// <summary>
        /// Set language to Icelandic
        /// </summary>
        public static AudioTranscriptionLanguage Icelandic { get { return new AudioTranscriptionLanguage("is"); } }

        /// <summary>
        /// Set language to Indonesian
        /// </summary>
        public static AudioTranscriptionLanguage Indonesian { get { return new AudioTranscriptionLanguage("id"); } }

        /// <summary>
        /// Set language to Italian
        /// </summary>
        public static AudioTranscriptionLanguage Italian { get { return new AudioTranscriptionLanguage("it"); } }

        /// <summary>
        /// Set language to Japanese
        /// </summary>
        public static AudioTranscriptionLanguage Japanese { get { return new AudioTranscriptionLanguage("ja"); } }

        /// <summary>
        /// Set language to Kannada
        /// </summary>
        public static AudioTranscriptionLanguage Kannada { get { return new AudioTranscriptionLanguage("kn"); } }

        /// <summary>
        /// Set language to Kazakh
        /// </summary>
        public static AudioTranscriptionLanguage Kazakh { get { return new AudioTranscriptionLanguage("kk"); } }

        /// <summary>
        /// Set language to Korean
        /// </summary>
        public static AudioTranscriptionLanguage Korean { get { return new AudioTranscriptionLanguage("ko"); } }

        /// <summary>
        /// Set language to Latvian
        /// </summary>
        public static AudioTranscriptionLanguage Latvian { get { return new AudioTranscriptionLanguage("lv"); } }

        /// <summary>
        /// Set language to Lithuanian
        /// </summary>
        public static AudioTranscriptionLanguage Lithuanian { get { return new AudioTranscriptionLanguage("lt"); } }

        /// <summary>
        /// Set language to Macedonian
        /// </summary>
        public static AudioTranscriptionLanguage Macedonian { get { return new AudioTranscriptionLanguage("mk"); } }

        /// <summary>
        /// Set language to Malay
        /// </summary>
        public static AudioTranscriptionLanguage Malay { get { return new AudioTranscriptionLanguage("ms"); } }

        /// <summary>
        /// Set language to Marathi
        /// </summary>
        public static AudioTranscriptionLanguage Marathi { get { return new AudioTranscriptionLanguage("mr"); } }

        /// <summary>
        /// Set language to Maori
        /// </summary>
        public static AudioTranscriptionLanguage Maori { get { return new AudioTranscriptionLanguage("mi"); } }

        /// <summary>
        /// Set language to Nepali
        /// </summary>
        public static AudioTranscriptionLanguage Nepali { get { return new AudioTranscriptionLanguage("ne"); } }

        /// <summary>
        /// Set language to Norwegian
        /// </summary>
        public static AudioTranscriptionLanguage Norwegian { get { return new AudioTranscriptionLanguage("no"); } }

        /// <summary>
        /// Set language to Persian
        /// </summary>
        public static AudioTranscriptionLanguage Persian { get { return new AudioTranscriptionLanguage("fa"); } }

        /// <summary>
        /// Set language to Polish
        /// </summary>
        public static AudioTranscriptionLanguage Polish { get { return new AudioTranscriptionLanguage("pl"); } }

        /// <summary>
        /// Set language to Portuguese
        /// </summary>
        public static AudioTranscriptionLanguage Portuguese { get { return new AudioTranscriptionLanguage("pt"); } }

        /// <summary>
        /// Set language to Romanian
        /// </summary>
        public static AudioTranscriptionLanguage Romanian { get { return new AudioTranscriptionLanguage("ro"); } }

        /// <summary>
        /// Set language to Russian
        /// </summary>
        public static AudioTranscriptionLanguage Russian { get { return new AudioTranscriptionLanguage("ru"); } }

        /// <summary>
        /// Set language to Serbian
        /// </summary>
        public static AudioTranscriptionLanguage Serbian { get { return new AudioTranscriptionLanguage("sr"); } }

        /// <summary>
        /// Set language to Slovak
        /// </summary>
        public static AudioTranscriptionLanguage Slovak { get { return new AudioTranscriptionLanguage("sk"); } }

        /// <summary>
        /// Set language to Slovenian
        /// </summary>
        public static AudioTranscriptionLanguage Slovenian { get { return new AudioTranscriptionLanguage("sl"); } }

        /// <summary>
        /// Set language to Spanish
        /// </summary>
        public static AudioTranscriptionLanguage Spanish { get { return new AudioTranscriptionLanguage("es"); } }

        /// <summary>
        /// Set language to Swahili
        /// </summary>
        public static AudioTranscriptionLanguage Swahili { get { return new AudioTranscriptionLanguage("sw"); } }

        /// <summary>
        /// Set language to Swedish
        /// </summary>
        public static AudioTranscriptionLanguage Swedish { get { return new AudioTranscriptionLanguage("sv"); } }

        /// <summary>
        /// Set language to Tagalog
        /// </summary>
        public static AudioTranscriptionLanguage Tagalog { get { return new AudioTranscriptionLanguage("tl"); } }

        /// <summary>
        /// Set language to Tamil
        /// </summary>
        public static AudioTranscriptionLanguage Tamil { get { return new AudioTranscriptionLanguage("ta"); } }

        /// <summary>
        /// Set language to Thai
        /// </summary>
        public static AudioTranscriptionLanguage Thai { get { return new AudioTranscriptionLanguage("th"); } }

        /// <summary>
        /// Set language to Turkish
        /// </summary>
        public static AudioTranscriptionLanguage Turkish { get { return new AudioTranscriptionLanguage("tr"); } }

        /// <summary>
        /// Set language to Ukrainian
        /// </summary>
        public static AudioTranscriptionLanguage Ukrainian { get { return new AudioTranscriptionLanguage("uk"); } }

        /// <summary>
        /// Set language to Urdu
        /// </summary>
        public static AudioTranscriptionLanguage Urdu { get { return new AudioTranscriptionLanguage("ur"); } }

        /// <summary>
        /// Set language to Vietnamese
        /// </summary>
        public static AudioTranscriptionLanguage Vietnamese { get { return new AudioTranscriptionLanguage("vi"); } }

        /// <summary>
        /// Set language to Welsh
        /// </summary>
        public static AudioTranscriptionLanguage Welsh { get { return new AudioTranscriptionLanguage("cy"); } }

        /// <summary>
        /// Gets the string value for this response format to pass to the API
        /// </summary>
        /// <returns>The response format as a string</returns>
        public override string ToString()
        {
            return Value;
        }

        /// <summary>
        /// Gets the string value for this response format to pass to the API
        /// </summary>
        /// <param name="value">The AudioTransFileFormat to convert</param>
        public static implicit operator String(AudioTranscriptionLanguage value) { return value; }

        internal class AudioTranscriptionLangJsonConverter : JsonConverter<AudioTranscriptionLanguage>
        {
            public override AudioTranscriptionLanguage ReadJson(JsonReader reader, Type objectType, AudioTranscriptionLanguage existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return new AudioTranscriptionLanguage(reader.ReadAsString());
            }

            public override void WriteJson(JsonWriter writer, AudioTranscriptionLanguage value, JsonSerializer serializer)
            {
                writer.WriteValue(value.ToString());
            }
        }
    }
}
