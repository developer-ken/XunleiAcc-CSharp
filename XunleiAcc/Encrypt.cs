using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace XunleiAcc
{
    class Encrypt
    {
        /// <summary>
        /// 32位标准MD5
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <returns>32位 MD5字符串</returns>
        public static string MD5(string data)
        {
            byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(data);
            System.Security.Cryptography.MD5CryptoServiceProvider cryptHandler;
            cryptHandler = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hash = cryptHandler.ComputeHash(textBytes);
            string ret = "";
            foreach (byte a in hash)
            {
                if (a < 16)
                    ret += "0" + a.ToString("x");
                else
                    ret += a.ToString("x");
            }
            return ret;
        }

        /// <summary>
        /// HMAC1   标准SHA1
        /// </summary>
        /// <param name="publickey">公钥</param>
        /// <param name="payload">要加密的内容</param>
        /// <returns>SHA1字符串</returns>
        public static string SHA1(string publickey, string payload)
        {
            byte[] byte1 = System.Text.Encoding.UTF8.GetBytes(publickey);
            byte[] byte2 = System.Text.Encoding.UTF8.GetBytes(payload);
            HMACSHA1 hmac = new HMACSHA1(byte1);
            string hashValue = hmac.ComputeHash(byte2).Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);
            return hashValue;
        }

        /// <summary>
        /// 无公钥的SHA1
        /// </summary>
        /// <param name="payload">要加密的内容</param>
        /// <returns>SHA1字符串</returns>
        public static string SHA1(string payload)
        {
            var strRes = Encoding.UTF8.GetBytes(payload);
            HashAlgorithm iSha = new SHA1CryptoServiceProvider();
            strRes = iSha.ComputeHash(strRes);
            var enText = new StringBuilder();
            foreach (byte iByte in strRes)
            {
                enText.AppendFormat("{0:x2}", iByte);
            }
            return enText.ToString();
        }
    }
}
