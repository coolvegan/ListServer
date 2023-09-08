using System;
using System.Security.Cryptography;
using System.Text;

namespace EssensbestellServer
{
    public class Verstecker
    {
        private static Verstecker verstecker;
        private string privaterSchluessel;
        private string publicSchluessel;
        public String gibPublic()
        {
            return publicSchluessel;
        }
        private Verstecker(){        
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048)) // 2048 ist die Schlüssellänge in Bits
            {
                // Exportieren des öffentlichen Schlüssels im XML-Format
                string publicKey = rsa.ExportRSAPublicKeyPem();

                // Exportieren des privaten Schlüssels im XML-Format (nur wenn erforderlich)
                privaterSchluessel = rsa.ToXmlString(true); // true gibt den privaten Schlüssel zurück
                publicSchluessel = rsa.ExportRSAPublicKeyPem();
            }
        }
        public static Verstecker getInstance()
        {
            if(verstecker == null)
            {
                verstecker = new Verstecker();
                return verstecker;
            }
            return verstecker;
        }

        public String entschluessel(byte[] bytes)
        {
            String result = "";
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048)) // 2048 ist die Schlüssellänge in Bits
            {
                // Exportieren des öffentlichen Schlüssels im XML-Format
                      // Exportieren des privaten Schlüssels im XML-Format (nur wenn erforderlich)
                rsa.FromXmlString(privaterSchluessel);
 
                try
                {
                    byte[] decryptedBytes = rsa.Decrypt(bytes, false);
                    result = Encoding.UTF8.GetString(decryptedBytes);
                } catch(CryptographicException e)
                {
                    Console.WriteLine("Nachtricht war nicht entschlüsselbar.");
                }
                

                return result;
            }
        }

		public String verschluessel(String text)
		{
            using (RSA rsa = RSA.Create())
            {
                byte[] publicKeyBytes = rsa.ExportRSAPublicKey();
                byte[] encryptedBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(text), RSAEncryptionPadding.OaepSHA256);

                return Convert.ToBase64String(encryptedBytes);
            }
        }
	}
}

