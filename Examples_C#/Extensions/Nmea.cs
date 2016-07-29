/* Copyright André Spitzner 2003 - 2016 */
namespace bcplanet.IOT.Base.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class Nmea
    {
        /// <summary>
        /// Adds the NMEA defined checksum to string.
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        /// <returns></returns>
        public static string AddChecksum(this string sentence)
        {
            int i;
            int iXor;
            // Calculate checksum ignoring any $'s in the string
            for (iXor = 0, i = 0; i < sentence.Length; i++)
            {
                int c = sentence[i];
                if (c == '*') break;
                if (c != '$') iXor ^= c;
            }
            return sentence + iXor.ToString("X2");
        }
    }
}
