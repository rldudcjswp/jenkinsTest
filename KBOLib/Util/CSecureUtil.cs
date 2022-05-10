using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Web.Security;

namespace KBOLib.Util
{
    /// <summary>
    /// 암호화 유틸 클래스
    /// 모든 함수는 정적으로 선언 되어 있다.
    /// </summary>
    public class CSecureUtil
    {
        public static string AesKey = "df3gfdrsdfgfsdhgf33ewnbseetr467d";     // 웹 프로토콜 key
        public static string AesKeyDB = "1dfghwhfdsdfas4gff332dfgfds3ghfa";   // DB 프로토콜 key

        [DllImport(@"AESDll.dll", CharSet = CharSet.Unicode)]
        extern public static int test_add(int a, int b);

        [DllImport(@"AESDll.dll", CharSet = CharSet.Unicode)]
        extern public static int test(StringBuilder buffer);

        /// <summary>
        /// SQL injection 방지를 위한 문자 변경 함수 
        /// </summary>
        /// <param name="src">검사 대상 문자열</param>
        /// <returns>안전하게 가공된 문자열</returns>
        public static string CheckBadString(string src)
        {
            string result = "";
            string[] badToken = { "‘", "“", "/", "\\", "--", ";", "%", "Union", "waitfor", "order by", "#", "xp_", "char(", "delete from", "drop table", "null", "sysobjects", "@@VERSION" };

            if (src == null)
            {
                return result;
            }

            result = src.Replace("'", "''");

            for (int i = 0; i < badToken.Length; i++)
            {
                result = result.Replace(badToken[i], "");
            }

            return result;
        }

        /// <summary>
        /// SHA512 Hash를 사용한 암호화
        /// </summary>
        /// <param name="data">암호화 대상 문자열</param>
        /// <returns>암호화된 문자열</returns>
        public static string SHA512Hash(string data)
        {
            SHA512 sha = new SHA512Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(data));
            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// AES256 암호화
        /// </summary>
        /// <param name="input">암호화 대상 문자열</param>
        /// <param name="key">암호화 비밀키</param>
        /// <returns>암호화된 문자열</returns>
        public static string AESEncrypt(string input, string key)
        {
            string output = "";

            if (!string.IsNullOrEmpty(input))
            {
                RijndaelManaged aes = new RijndaelManaged();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] xBuff = null;
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = Encoding.UTF8.GetBytes(input);
                        cs.Write(xXml, 0, xXml.Length);
                    }

                    xBuff = ms.ToArray();
                }

                output = Convert.ToBase64String(xBuff);
            }

            return output;
        }

        /// <summary>
        /// AES256 복호화
        /// </summary>
        /// <param name="input">복호화 대상 문자열</param>
        /// <param name="key">복호화 비밀키</param>
        /// <returns>복호화된 문자열</returns>
        public static string AESDecrypt(string input, string key)
        {
            string output = "";

            if (!string.IsNullOrEmpty(input))
            {
                RijndaelManaged aes = new RijndaelManaged();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                var decrypt = aes.CreateDecryptor();
                byte[] xBuff = null;

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = Convert.FromBase64String(input);
                        cs.Write(xXml, 0, xXml.Length);
                    }

                    xBuff = ms.ToArray();
                }

                output = Encoding.UTF8.GetString(xBuff);
            }

            return output;
        }

        #region KBO 홈페이지 웹사이트 적용된 암호화
        private static string CryptKeySting = "19820828";

        #region  String을 byte로 단순변환
        /// <summary>
        /// String을 byte[]로 단순변환
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>

        private static byte[] stringToByte(string value)
        {
            Byte[] buffer = new byte[value.Length];

            for (int i = 0; i < value.Length; i++)
            {
                buffer[i] = Convert.ToByte(value.Substring(i, 1));
            }
            return buffer;
        }
        #endregion

        #region 실제로 복호화하는 함수
        // Decrypt the byte array.
        /// <summary>
        /// 복호화 주요 함수
        /// </summary>
        /// <param name="CypherText">해독할값</param>
        /// <param name="key">키값</param>
        /// <returns></returns>
        private static String Decrypt(byte[] CypherText, SymmetricAlgorithm key)
        {
            // Create a memory stream to the passed buffer.
            MemoryStream ms = new MemoryStream(CypherText);

            // Create a CryptoStream using the memory stream and the 
            // CSP DES key. 
            CryptoStream encStream = new CryptoStream(ms, key.CreateDecryptor(), CryptoStreamMode.Read);

            // Create a StreamReader for reading the stream.
            StreamReader sr = new StreamReader(encStream);

            // Read the stream as a string.
            string val = sr.ReadLine();

            // Close the streams.
            sr.Close();
            encStream.Close();
            ms.Close();

            return val;
        }
        #endregion

        #region 암호화를 수행하는 중요함수
        /// <summary>
        /// 암호화를 수행하는 중요함수
        /// </summary>
        /// <param name="PlainText"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        // Encrypt the string.
        private static byte[] Encrypt(String PlainText, SymmetricAlgorithm key)
        {
            // Create a memory stream.
            MemoryStream ms = new MemoryStream();

            // Create a CryptoStream using the memory stream and the 
            // CSP DES key.  
            CryptoStream encStream = new CryptoStream(ms, key.CreateEncryptor(), CryptoStreamMode.Write);

            // Create a StreamWriter to write a string
            // to the stream.
            StreamWriter sw = new StreamWriter(encStream);

            // Write the plaintext to the stream.
            sw.WriteLine(PlainText);

            // Close the StreamWriter and CryptoStream.
            sw.Close();
            encStream.Close();

            // Get an array of bytes that represents
            // the memory stream.
            byte[] buffer = ms.ToArray();

            // Close the memory stream.
            ms.Close();

            // Return the encrypted byte array.
            return buffer;
        }
        #endregion

        #region 암호화 호출함수
        /// <summary>
        /// 실제 암호화할때 호출하는 함수
        /// </summary>
        /// <param name="inputvalue"></param>
        /// <returns></returns>
        public static String DesEncrypt(string inputvalue)
        {
            //string output;

            // Create a new DES key.
            DESCryptoServiceProvider key = new DESCryptoServiceProvider();

            key.Key = stringToByte(CryptKeySting);
            key.IV = stringToByte(CryptKeySting);

            // Encrypt a string to a byte array.
            byte[] buffer = Encrypt(inputvalue, key);

            return Convert.ToBase64String(buffer);
        }
        #endregion

        #region 복호화 호출함수 byte[] -> string
        /// <summary>
        /// 복호화를 위한 호출 함수
        /// </summary>
        /// <param name="inputvalue"></param>
        /// <returns></returns>
        public static String DesDecrypt(String inputvalue)
        {
            String output;

            // Create a new DES key.
            DESCryptoServiceProvider key = new DESCryptoServiceProvider();

            key.Key = stringToByte(CryptKeySting);
            key.IV = stringToByte(CryptKeySting);

            // Encrypt a string to a byte array.
            output = Decrypt(Convert.FromBase64String(inputvalue), key);

            return output;
        }
        #endregion

        #region MD5 암호화
        /// <summary>
        /// MD5 암호화
        /// </summary>
        /// <param name="val">변경전 데이터</param>
        /// <returns>암호화 적용 완료 데이터</returns>
        public static String MD5HashCrypt(String val)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(val, "MD5");
        }
        #endregion

        #endregion
    }
}