using System.IO;

using JetBrains.Annotations;

namespace Stove.IO.Extensions
{
    public static class StreamExtensions
    {
        [NotNull]
        public static byte[] GetAllBytes([NotNull] this Stream stream)
        {
            byte[] streamBytes;

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                streamBytes = memoryStream.ToArray();
            }

            return streamBytes;
        }
    }
}
