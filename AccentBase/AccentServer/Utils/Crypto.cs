using System.IO;
using System.Security.Cryptography;

namespace AccentServer.Utils
{
    class Crypto
    {
        public byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes, byte[] saltBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            if (saltBytes == null)
            {
                saltBytes = new byte[] { 101, 111, 67, 85, 13, 1, 217, 99 };
            }

            if (passwordBytes == null)
            {
                SHA256 mySHA256 = SHA256Managed.Create();
                passwordBytes = mySHA256.ComputeHash(Utils.Converting.GetBytes("Сорок тысяч обезьян с жопой что-то там того"));
                //byte[] byteMsg = new byte[15] { 67, 111, 110, 110, 101, 99, 116, 32, 84, 101, 115, 116, 105, 110, 103 };
            }
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;
                    AES.Padding = PaddingMode.PKCS7;
                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        //cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes, byte[] saltBytes)
        {
            byte[] decryptedBytes = null;

            if (saltBytes == null)
            {
                saltBytes = new byte[] { 101, 111, 67, 85, 13, 1, 217, 99 };
            }

            if (passwordBytes == null)
            {
                SHA256 mySHA256 = SHA256Managed.Create();
                passwordBytes = mySHA256.ComputeHash(Utils.Converting.GetBytes("Сорок тысяч обезьян с жопой что-то там того"));
                //byte[] byteMsg = new byte[15] { 67, 111, 110, 110, 101, 99, 116, 32, 84, 101, 115, 116, 105, 110, 103 };
            }
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;
                    AES.Padding = PaddingMode.PKCS7;
                    try
                    {
                        using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                            //cs.Close();
                        }
                        decryptedBytes = ms.ToArray();
                        return decryptedBytes;
                    }
                    catch
                    {
                        return null;
                    }

                }
            }
        }
    }
}
