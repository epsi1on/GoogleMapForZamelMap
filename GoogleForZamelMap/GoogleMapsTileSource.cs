using System;
using System.Security.Cryptography;
using System.Text;
using MapControl;

namespace GoogleForZamelMap
{
    internal class GoogleMapsTileSource : TileSource
    {
        public override Uri GetUri(int x, int y, int zoomLevel)
        {
            var language = "en";

            string sec1 = string.Empty; // after &x=...
            string sec2 = string.Empty; // after &zoom=...
            GetSecureWords(x, y, out sec1, out sec2);

            var url = string.Format(UrlFormat, UrlFormatServer, GetServerNum(x, y, 4), UrlFormatRequest, Version,
                language, x, sec1, y, zoomLevel, sec2, Server);

            return new Uri(url);
        }

        protected static int GetServerNum(int x, int y, int max)
        {
            return (int) (x + 2*y)%max;
        }

        public string SecureWord = "Galileo";

        private static readonly string Sec1 = "&s=";
        public readonly string Server /* ;}~~~~ */ = GString( /*{^_^}*/"gosr2U13BoS+bXaIxt6XWg==" /*d{'_'}b*/);
        private static readonly string UrlFormatServer = "mt";
        private static readonly string UrlFormatRequest = "vt";
        private static readonly string UrlFormat = "http://{0}{1}.{10}/{2}/lyrs={3}&hl={4}&x={5}{6}&y={7}&z={8}&s={9}";
        public string Version = "m@318000000";

        private static readonly string manifesto =
            "GMap.NET is great and Powerful, Free, cross platform, open source .NET control.";


        internal void GetSecureWords(int x, int y, out string sec1, out string sec2)
        {
            sec1 = string.Empty; // after &x=...
            sec2 = string.Empty; // after &zoom=...
            int seclen = (int) ((x*3) + y)%8;
            sec2 = SecureWord.Substring(0, seclen);
            if (y >= 10000 && y < 100000)
            {
                sec1 = Sec1;
            }
        }

        public static string GString(string Message)
        {
            var ret = DecryptString(Message, manifesto);

            return ret;
        }

        private static string DecryptString(string Message, string Passphrase)
        {
            byte[] Results;

            using (var HashProvider = new SHA1CryptoServiceProvider())
            {
                byte[] TDESKey = HashProvider.ComputeHash(Encoding.UTF8.GetBytes(Passphrase));
                Array.Resize(ref TDESKey, 16);

                // Step 2. Create a new TripleDESCryptoServiceProvider object
                using (TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider())
                {
                    // Step 3. Setup the decoder
                    TDESAlgorithm.Key = TDESKey;
                    TDESAlgorithm.Mode = CipherMode.ECB;
                    TDESAlgorithm.Padding = PaddingMode.PKCS7;

                    // Step 4. Convert the input string to a byte[]
                    byte[] DataToDecrypt = Convert.FromBase64String(Message);

                    // Step 5. Attempt to decrypt the string
                    try
                    {
                        using (ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor())
                        {
                            Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
                        }
                    }
                    finally
                    {
                        // Clear the TripleDes and Hashprovider services of any sensitive information
                        TDESAlgorithm.Clear();
                        HashProvider.Clear();
                    }
                }
            }

            // Step 6. Return the decrypted string in UTF8 format
            return Encoding.UTF8.GetString(Results, 0, Results.Length);
        }
    }
}