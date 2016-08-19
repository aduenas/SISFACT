using System;
using System.Text;
using System.Security.Cryptography;

namespace Seguridad
{
    public class Seguridad
    {
        //Atributos
        TripleDESCryptoServiceProvider TripleDes = new TripleDESCryptoServiceProvider();
        private const string Semilla = "./-[3cl3$iast3$|]!./";
        private const string TipoHash = "SHA1";
        private const int Iteraciones = 2;
        private const string Vector = "3$P@rt@Cu$[;'.+2635./";
        private const int KeySize = 192;

        public static string Encriptar(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Vector));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(Vector);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }


    }
}