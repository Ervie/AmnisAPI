using API.Model;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace API.Utilities
{
    public static class MetadataWorker
    {
        public static SongMetadata SendRequest(string channelUrl)
        {
            Stream socketStream = null;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(channelUrl);
                request.Headers.Clear();
                request.Headers.Add("GET", " HTTP/1.0");
                // Necessary to receive metadata with song title.
                request.Headers.Add("Icy-MetaData", "1");
                request.UserAgent = "WinampMPEG/5.09";
                var response = (HttpWebResponse)(request.GetResponse());

                var metaInt = Convert.ToInt32(response.GetResponseHeader("icy-metaint"));
                socketStream = response.GetResponseStream();

                // 4080 - Max metadata length = ( byte size (255) - 1) * length of single word (16)
                var buffer = new byte[metaInt + 4080];

                var bufLen = ReadAll(socketStream, buffer, 0, buffer.Length);
                if (bufLen < 0)
                    throw new IOException();

                var metadataLength = Convert.ToInt32(buffer[metaInt]) * 16;
                var metadataHeader = Encoding.UTF8.GetString(buffer, metaInt + 1, metadataLength)
                    .Trim().Replace("\0", string.Empty);

                // Empty header - sometimes it just happens, not really an issue
                if (string.IsNullOrWhiteSpace(metadataHeader))
                    return new SongMetadata();

                string[] metadataChunks = metadataHeader.Split('\'');

                string channelMetadata = string.Empty;

                // Skip not needed parts (first and last)
                for (int i = 1; i < metadataChunks.Length - 1; i++)
                {
                    // Some channel contain extra expendable informations at the end
                    if (metadataChunks[i].Equals(";StreamUrl=") || metadataChunks[i].Equals(";StreamNext="))
                        break;

                    channelMetadata += metadataChunks[i];
                    channelMetadata += "'";
                }

                channelMetadata = WebUtility.HtmlDecode(channelMetadata.TrimEnd('\''));
                
                return new SongMetadata(channelMetadata);
            }
            catch (Exception)
            {
                return new SongMetadata();
            }
        }

        /// <summary>
        /// Read stream until buffer length will be equal expected length.
        /// </summary>
        /// <param name="stream">Stream to read</param>
        /// <param name="buffer">Buffer to save</param>
        /// <param name="offset">Offset</param>
        /// <param name="length">Amount of bytes to read</param>
        /// <returns>Amount of read bytes</returns>
        private static int ReadAll(Stream stream, byte[] buffer, int offset, int length)
        {
            if ((offset + length) > buffer.Length)
            {
                throw new ArgumentException();
            }
            var bytesReadSoFar = 0;
            while (bytesReadSoFar < length)
            {
                var bytes = stream.Read(
                    buffer, offset + bytesReadSoFar, length - bytesReadSoFar);
                if (bytes == 0)
                {
                    break;
                }
                bytesReadSoFar += bytes;
            }
            return bytesReadSoFar;
        }
    }
}
