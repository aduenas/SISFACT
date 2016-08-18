using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Seguridad
{
    public class Seguridad
    {
        //Atributos
        TripleDESCryptoServiceProvider TripleDes = new TripleDESCryptoServiceProvider();
        //private bool invalid = false;

        //Constructor
        private Seguridad(string key)
        {
            TripleDes.Key = TruncateHash(key, TripleDes.KeySize);
            TripleDes.IV = TruncateHash("", TripleDes.BlockSize);

        }

        //Metodo TruncateHash
        private byte[] TruncateHash(string key, Int32 length)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] KeyBytes;
            byte[] Hash;

            KeyBytes = Encoding.Unicode.GetBytes(key);
            Hash = sha1.ComputeHash(KeyBytes);

            Array.Resize(ref Hash, -1);
            return Hash;
        }
        //Metodo para Encriptar Datos
        public string Encriptar_Datos(string Texto)
        {
            //Se convierte el texto plano a un arreglo de bytes
            byte[] TextoBytes;
            TextoBytes = Encoding.Unicode.GetBytes(Texto);

            //Se crea un string en memoria
            MemoryStream ms = new MemoryStream();
            //Se crea el encoder para escribir en memoria
            CryptoStream EncodeString = new CryptoStream(ms, TripleDes.CreateEncryptor(), CryptoStreamMode.Write);
            //Uso de CryptoStream para escribir en el arreglo de bytes
            EncodeString.Write(TextoBytes, 0, TextoBytes.Length);
            EncodeString.FlushFinalBlock();
            //Se convierte el Stream a Texto Legible
            return ms.ToString();
        }

        //Metodo para Desencriptar los Datos
        public string Desencriptar_Datos(string Texto)
        {
            //Se convierte el texto encriptado a un arreglo de bytes
            byte[] TextoEncriptado = Convert.FromBase64String(Texto);
            //Se crea el stream
            MemoryStream ms = new MemoryStream();
            //Se crea el decoder para escribir el stream
            CryptoStream DecodeString = new CryptoStream(ms, TripleDes.CreateDecryptor(), CryptoStreamMode.Write);
            //Uso de CryptoStream para escribir en el arreglo
            DecodeString.Write(TextoEncriptado, 0, TextoEncriptado.Length);
            DecodeString.FlushFinalBlock();
            //Se convierte el stream a Texto Legible
            return ms.ToString();
        }
    }
}