using System.IO;

namespace Snappy.Net.Test.Common
{
    public static class TestData
    {
        public static Stream GetResourceStream(string fileName)
        {
            var stream = typeof(TestData).Assembly.GetManifestResourceStream(string.Format("Snappy.Net.Test.Common.testdata.{0}", fileName));
            
            if (stream == null)
            {
                throw new FileNotFoundException("resource not found");
            }

            return stream;
        }

        public static byte[] GetResourceBytes(string fileName)
        {
            using (var resourceStream = GetResourceStream(fileName))
            {
                var buffer = new byte[resourceStream.Length];

                using (var memStream = new MemoryStream(buffer))
                {
                    resourceStream.CopyTo(memStream);
                }

                return buffer;
            }
        }
    }
}
