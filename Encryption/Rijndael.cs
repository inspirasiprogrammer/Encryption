using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

/*
   Copyright 2015 Kees van Spelde
   Licensed under The Code Project Open License (CPOL) 1.02;
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at
     http://www.codeproject.com/info/cpol10.aspx
   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

namespace Encryption
{
    #region Interface IReader
    /// <summary>
    /// Interface to make Ryndael class COM exposable
    /// </summary>
    public interface IRijndael
    {
        [DispId(1)]
        string Encrypt(string text, string salt);

        [DispId(2)]
        string Decrypt(string cipherText, string salt);
    }
    #endregion

    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Encryption.Rijndael")]
    [Guid("48C93B07-0AB6-4412-B4F6-0F9E088E5533")]
    [ComVisible(true)]
    public class Rijndael : IRijndael
    {
        #region Consts
        /// <summary>
        /// Change this Inputkey GUID with a new GUID when you use this code in your own program !!!
        /// Keep this inputkey very safe and prevent someone from decoding it some way !!!
        /// </summary>
        internal const string Inputkey = "560A18CD-6346-4CF0-A2E8-671F9B6B9EA9";
        #endregion

        #region Encryption
        /// <summary>
        /// Encrypt the given text and give the byte array back as a BASE64 string
        /// </summary>
        /// <param name="text">The text to encrypt</param>
        /// <param name="salt">The pasword salt</param>
        /// <returns>The encrypted text</returns>
        public string Encrypt(string text, string salt)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentNullException("text");

            var aesAlg = NewRijndaelManaged(salt);

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            var msEncrypt = new MemoryStream();

            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
                swEncrypt.Write(text);

            return Convert.ToBase64String(msEncrypt.ToArray());
        }
        #endregion

        #region Decrypt
        /// <summary>
        /// Checks if a string is base64 encoded
        /// </summary>
        /// <param name="base64String">The base64 encoded string</param>
        /// <returns></returns>
        private static bool IsBase64String(string base64String)
        {
            base64String = base64String.Trim();

            return (base64String.Length % 4 == 0) &&
                    Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

        /// <summary>
        /// Decrypts the given text
        /// </summary>
        /// <param name="cipherText">The encrypted BASE64 text</param>
        /// <param name="salt">The pasword salt</param>
        /// <returns>De gedecrypte text</returns>
        public string Decrypt(string cipherText, string salt)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");

            if (!IsBase64String(cipherText))
                throw new Exception("The cipherText input parameter is not base64 encoded");

            var aesAlg = NewRijndaelManaged(salt);
            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            var cipher = Convert.FromBase64String(cipherText);

            using (var msDecrypt = new MemoryStream(cipher))
            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (var srDecrypt = new StreamReader(csDecrypt))
                return srDecrypt.ReadToEnd();
        }
        #endregion

        #region NewRijndaelManaged
        /// <summary>
        /// Create a new RijndaelManaged class and initialize it
        /// </summary>
        /// <param name="salt">The pasword salt</param>
        /// <returns></returns>
        private static RijndaelManaged NewRijndaelManaged(string salt)
        {
            if (salt == null) throw new ArgumentNullException("salt");
            var saltBytes = Encoding.ASCII.GetBytes(salt);
            var key = new Rfc2898DeriveBytes(Inputkey, saltBytes);

            var aesAlg = new RijndaelManaged();
            aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
            aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

            return aesAlg;
        }
        #endregion
    }
}
