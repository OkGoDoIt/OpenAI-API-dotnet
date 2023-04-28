using System.IO;

namespace OpenAI_API.Audio
{
    /// <summary>
    /// Audio file object for transcript and translate requests.
    /// </summary>
    public class AudioFile
    {
        /// <summary>
        /// Stream of the file.
        /// </summary>
        public Stream File { get; set; }

        /// <summary>
        /// Content length of the file
        /// </summary>
        public long ContentLength { get { return File.Length; } }

        /// <summary>
        /// Type of audio file.Must be mp3, mp4, mpeg, mpga, m4a, wav, or webm.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Full name of the file. such as test.mp3
        /// </summary>
        public string Name { get; set; }
    }
}
