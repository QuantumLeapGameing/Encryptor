using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace My_Encrypted_Data_Vault
{
    public class RSAEncryption
    {
        private static RSACryptoServiceProvider csp = new RSACryptoServiceProvider(4096);
        private RSAParameters _privateKey;
        private RSAParameters _publicKey;
        private RSAParameters _nonlocalpublicKey;
        public RSAEncryption()
        {
            try
            {
                _privateKey = csp.ExportParameters(true);
                _publicKey = csp.ExportParameters(false);
            }
            catch
            {

            }
            if (!File.Exists("./Keys/parameter.pub.key") || !File.Exists("./Keys/parameter.priv.key")) 
            {
                // This is the new way to write the RSAParameters to a file.
                var xml = new XmlSerializer(typeof(RSAParameters));
                using (StreamWriter sw = new StreamWriter("./Keys/parameter.priv.key", false, Encoding.GetEncoding("ISO-8859-1")))
                {
                    xml.Serialize(sw, _privateKey);
                }


                // This is the new way to write the RSAParameters to a file.
                using (StreamWriter sw = new StreamWriter("./Keys/parameter.pub.key", false, Encoding.GetEncoding("ISO-8859-1")))
                {
                    xml.Serialize(sw, _publicKey);
                }
            }

            // Go from file to an object variable, aka deserialization
            using (StreamReader stream = new StreamReader("./Keys/parameter.pub.key"))
            {
                var xs = new XmlSerializer(typeof(RSAParameters));
                _publicKey = (RSAParameters)xs.Deserialize(stream);
            }

            // Go from file to an object variable, aka deserialization
            using (StreamReader stream = new StreamReader("./Keys/parameter.priv.key"))
            {
                var xs = new XmlSerializer(typeof(RSAParameters));
                _privateKey = (RSAParameters)xs.Deserialize(stream);
            }
            {

            }
            try
            {
                var files = Directory.GetFiles("./Keys/Nonlocal");
                if (files.Length != 0)
                {
                    using (StreamReader stream = new StreamReader(files[0]))
                    {
                        var xs = new XmlSerializer(typeof(RSAParameters));
                        _nonlocalpublicKey = (RSAParameters)xs.Deserialize(stream);
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error in nonlocal key: File Invalid");
            }
        }

        public string GetPublicKey()
        {
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, _publicKey);
            return sw.ToString();
        }
        public string GetPrivateKey()
        {
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, _privateKey);
            return sw.ToString();
        }
        public string Encrypt(string plainText)
        {
            try
            {
                csp = new RSACryptoServiceProvider();
                csp.ImportParameters(_publicKey);
                var data = Encoding.Unicode.GetBytes(plainText);
                var cypher = csp.Encrypt(data, false);
                return Convert.ToBase64String(cypher);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine();
                Console.WriteLine("This message most likely means that your keys are invalid. to fix try recovering the keys or makeing new ones but note makeing new keys will render all older stored encrypted texts useless.");
                return null;
            }
        }
        public string EncryptNonnativekey(string plainText)
        {
            try
            {
                csp = new RSACryptoServiceProvider();
                csp.ImportParameters(_nonlocalpublicKey);
                var data = Encoding.Unicode.GetBytes(plainText);
                var cypher = csp.Encrypt(data, false);
                return Convert.ToBase64String(cypher);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine();
                Console.WriteLine("This most likely means you dont have a non local key stored to get one ask a freind to give you his public key and put it in the nonlocal folder under keys");
                return null;
            }
        }
        public string Decrypt(string cypherText)
        {
            try
            {
                var dataBytes = Convert.FromBase64String(cypherText);
                csp.ImportParameters(_privateKey);
                var plainText = csp.Decrypt(dataBytes, false);
                return Encoding.Unicode.GetString(plainText);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine();
                Console.WriteLine("This message most likely means that your keys are invalid. to fix try recovering the keys or makeing new ones but note makeing new keys will render all older stored encrypted texts useless.");
                Console.WriteLine("or you imported non cypher text");
                Console.WriteLine();
                return null;
            }

        }
        public void Demo()
        {
            RSAEncryption rsa = new RSAEncryption();
            string cypher = string.Empty;
            //Console.WriteLine($"Public Key: {rsa.GetPublicKey()}");
            Console.WriteLine("Input Text");
            var text = Console.ReadLine();
            if (!string.IsNullOrEmpty(text))
            {
                cypher = rsa.Encrypt(text);
                Console.WriteLine($"Encypted text: {cypher} ");
            }
            Console.WriteLine("press any key to decrypt");
            Console.ReadLine();
            Console.WriteLine(rsa.Decrypt(cypher));
            Console.ReadKey();
        }
        public void EncryptText()
        {
            RSAEncryption rsa = new RSAEncryption();
            string cypher = string.Empty;
            Console.WriteLine("Input Text:");
            Console.WriteLine();
            var text = Console.ReadLine();
            if (!string.IsNullOrEmpty(text) & text.Length < 749)
            {
                cypher = rsa.Encrypt(text);
                Console.WriteLine();
                Console.WriteLine($"Encypted text: {cypher} ");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("try typeing something good next time lol");
            }
            Console.WriteLine();
            Console.WriteLine("press any key to continue");
            Console.ReadKey();
        }

        public void EncryptTextwithnonnativekey()
        {
            RSAEncryption rsa = new RSAEncryption();
            string cypher = string.Empty;
            Console.WriteLine("Input Text:");
            Console.WriteLine();
            var text = Console.ReadLine();
            if (!string.IsNullOrEmpty(text) & text.Length < 749)
            {
                cypher = rsa.EncryptNonnativekey(text);
                Console.WriteLine();
                Console.WriteLine($"Encypted text: {cypher} ");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("try typeing something good next time lol");
            }
            Console.WriteLine();
            Console.WriteLine("press any key to continue");
            Console.ReadKey();
        }
        public void DecryptText()
        {
            RSAEncryption rsa = new RSAEncryption();
            Console.WriteLine("Input Cypher Text");
            Console.WriteLine();
            var cypher = Console.ReadLine();
            if (!string.IsNullOrEmpty(cypher))
            {
                var text = (rsa.Decrypt(cypher));
                Console.WriteLine();
                Console.WriteLine($"Decypted text: {text} ");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("try typeing something good next time lol");
            }
            Console.WriteLine();
            Console.WriteLine("press any key to continue");
            Console.ReadKey();
        }
    }

}