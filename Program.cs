using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace My_Encrypted_Data_Vault
{
    class Program
    {
        static void Main(string[] args)
        {
            if(!Directory.Exists("./Keys"))
            {
                Directory.CreateDirectory("./Keys");
            }
            if (!Directory.Exists("./SavedData"))
            {
                Directory.CreateDirectory("./SavedData");
            }
            if (!Directory.Exists("./Keys/Nonlocal"))
            {
                Directory.CreateDirectory("./Keys/Nonlocal");
            }
            if (!Directory.Exists("./Keys/Nonlocalstored"))
            {
                Directory.CreateDirectory("./Keys/Nonlocalstored");
            }
            RSAEncryption Encryptor = new RSAEncryption();
            IDictionary<int, string> Vault = new Dictionary<int, string>();
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to the encryptor! (useing rsa 4096 bit encryption)");
                Console.WriteLine("Type /help for command list");
                var text = Console.ReadLine();
                var texts = text.ToLower().Split("-");
                switch (texts[0])
                {
                    case "/encrypt":
                        {
                            string Responce = string.Empty;
                            Console.Clear();
                            Console.WriteLine("Choose a local or nonlocal key");
                            Console.WriteLine("type n and press enter for nonlocal or just press enter for local");
                            Console.WriteLine();
                            Responce = Console.ReadLine();
                            if (Responce != string.Empty)
                            {
                                if (Responce.ToLower() =="n")
                                {
                                    Console.Clear();
                                    Encryptor.EncryptTextwithnonnativekey();
                                }
                            }
                            if(Responce == string.Empty)
                            {
                                Console.Clear();
                                Encryptor.EncryptText();
                            }
                        }
                        break;
                    case "/decrypt":
                        {
                            Console.Clear();
                            Encryptor.DecryptText();
                        }
                        break;
                    case "/demo":
                        {
                            Console.Clear();
                            Encryptor.Demo();
                        }
                        break;
                    case "/example":
                        {
                            Console.Clear();
                            Console.WriteLine("/store");
                            Console.WriteLine("/get");
                            Console.WriteLine("/save");
                            Console.WriteLine("/load");
                            Console.WriteLine("/Demo");
                            Console.WriteLine("/Encrypt");
                            Console.WriteLine("/Decrypt");
                            Console.WriteLine("/Help-K");
                            Console.WriteLine("/Switchkey");
                            Console.WriteLine();
                            Console.WriteLine("Press any key to return to menu");
                            Console.ReadKey();
                        }
                        break;
                    case "/help":
                        {
                            if (texts.Length == 2)
                            {
                                if(texts[1] == "k")
                                {
                                    Console.Clear();
                                    Console.WriteLine("All keys for this program are stored in {0}", Directory.GetCurrentDirectory()+@"\Keys");
                                    Console.WriteLine("To regenerate the keys simply delete the two .key files directly in the folder.");
                                    Console.WriteLine("To import a place it in {0}", Directory.GetCurrentDirectory() + @"\Keys\Nonlocalstored");
                                    Console.WriteLine("and run type /switchkey and choose which key wou wish to be the nonlocal key");
                                    Console.WriteLine();
                                    Console.WriteLine("Press any key to return to menu");
                                    Console.ReadKey();
                                }
                            }
                            if (texts.Length == 1)
                            {
                                Console.Clear();
                                Console.WriteLine("For further info on command structures type /example for a example on how to use each commmand");
                                Console.WriteLine("To store a set of stored values and overite currently stored sets ones use /save");
                                Console.WriteLine("To load a set of stored values and overite currently stored ones use /load");
                                Console.WriteLine("For a demonstration of the encryption type /Demo");
                                Console.WriteLine("To enter text to encrypt type /Encrypt");
                                Console.WriteLine("To enter chiper to decrypt type /Decrypt");
                                Console.WriteLine("To store a value type /Store");
                                Console.WriteLine("To open a stored value type /Get");
                                Console.WriteLine("For info on keys type /Help-K");
                                Console.WriteLine();
                                Console.WriteLine("Press any key to return to menu");
                                Console.ReadKey();
                            }
                        }
                        break;
                    case "/store":
                        {
                            string Responce = string.Empty;
                            Console.Clear();
                            Console.WriteLine("Enter e or n or s");
                            Console.WriteLine("to specify storage method");
                            Console.WriteLine();
                            Responce = Console.ReadLine();
                            Console.WriteLine();
                            Console.WriteLine("Enter a slot number");
                            Console.WriteLine();
                            Responce = Responce + "-" + Console.ReadLine();
                            Console.WriteLine();
                            Console.WriteLine("Your text");
                            Console.WriteLine();
                            Responce = Responce + "-" + Console.ReadLine();
                            var Responces = Responce.ToLower().Split("-");
                            try
                            {
                                if (Responces.Length == 3)
                                {
                                    if (Responces[0] == "e")
                                    {
                                        if (Vault.ContainsKey(Int32.Parse(Responces[1])))
                                        {
                                            Vault.Remove(Int32.Parse(Responces[1]));
                                        }
                                        Vault.Add(Int32.Parse(Responces[1]), Encryptor.Encrypt(Responces[2].ToString()));
                                    }
                                    if (Responces[0] == "n")
                                    {
                                        if (Vault.ContainsKey(Int32.Parse(Responces[1])))
                                        {
                                            Vault.Remove(Int32.Parse(Responces[1]));
                                        }
                                        Vault.Add(Int32.Parse(Responces[1]), Encryptor.EncryptNonnativekey(Responces[2].ToString()));
                                    }
                                    if (Responces[0] == "s")
                                    {
                                        if (Vault.ContainsKey(Int32.Parse(Responces[1])))
                                        {
                                            Vault.Remove(Int32.Parse(Responces[1]));
                                        }
                                        Vault.Add(Int32.Parse(Responces[1]), Responces[2].ToString());
                                    }
                                }
                            }
                            catch(Exception)
                            {

                            }
                        }
                        break;
                    case "/get":
                        {
                            string Responce = string.Empty;
                            Console.Clear();
                            Console.WriteLine("Enter e or n or s");
                            Console.WriteLine("to specify retrieval method");
                            Console.WriteLine();
                            Responce = Console.ReadLine();
                            Console.WriteLine();
                            Console.WriteLine("Enter slot number");
                            Console.WriteLine();
                            Responce = Responce + "-" + Console.ReadLine();
                            var Responces = Responce.ToLower().Split("-");
                            try
                            {
                                Console.Clear();
                                Console.WriteLine("Stored Item:");
                                Console.WriteLine("");
                                if (Responces.Length == 2)
                                {
                                    if (Responces[0] == "e")
                                    {
                                        Console.WriteLine("{0}", Encryptor.Decrypt(Vault[Int32.Parse(Responces[1])]));
                                    }
                                    if (Responces[0] == "s")
                                    {
                                        Console.WriteLine("{0}", Vault[Int32.Parse(Responces[1])]);
                                    }
                                }
                                Console.WriteLine("");
                                Console.WriteLine("Press any key to return to menu");
                                Console.ReadKey();
                            }
                            catch(Exception)
                            {

                            }
                        }
                        break;
                    case "/save":
                        {
                            var serializer = new DataContractSerializer(typeof(Dictionary<int, string>));
                            using (var writer = XmlWriter.Create("./SavedData/Data.SV"))
                            {
                                serializer.WriteObject(writer, Vault);
                            }

                            //var xml = new XmlSerializer(typeof(IDictionary<int, string>));
                            //using (StreamWriter sw = new StreamWriter("./SavedData/Data.SV", false, System.Text.Encoding.GetEncoding("ISO-8859-1")))
                            //{
                            //    xml.Serialize(sw, Vault);
                            //}
                        }
                        break;
                    case "/load":
                        {
                            var serializer = new DataContractSerializer(typeof(Dictionary<int, string>));
                            using (Stream fs = new FileStream("./SavedData/Data.SV", FileMode.Open, FileAccess.Read))
                            using (XmlDictionaryReader xdw = XmlDictionaryReader.CreateTextReader(fs, Encoding.UTF8, XmlDictionaryReaderQuotas.Max, null))
                            {
                                //xdw.WriteStartDocument();
                                Dictionary<int, string> myDictionary = (Dictionary<int, string>)serializer.ReadObject(xdw);
                                Vault = myDictionary;
                            }
      
                            //using (StreamReader stream = new StreamReader("./SavedData/Data.SV"))
                            //{
                            //    var xs = new XmlSerializer(typeof(Dictionary<int, string>));
                            //    Vault = (Dictionary<int, string>)xs.Deserialize(stream);
                            //}
                        }
                        break;
                    case "/switchkey":
                        {
                            Console.Clear();
                            Console.WriteLine("Select a Key to switch to:");
                            Console.WriteLine("(only type the file name)");
                            Console.WriteLine();
                            var files = Directory.GetFiles("./Keys/Nonlocalstored");
                            var fileinkey = Directory.GetFiles("./Keys/Nonlocal");
                            foreach (var f in files)
                            {
                                Console.WriteLine("{0}",f);
                            }
                            Console.WriteLine();
                            var input = Console.ReadLine();
                            if(File.Exists("./Keys/Nonlocalstored/"+ input))
                            {
                                var path = ("./Keys/Nonlocalstored/" + input);
                                try
                                {
                                    File.Move(fileinkey[0], "./Keys/Nonlocalstored/" + fileinkey[0].Split("\\")[1]);
                                }
                                catch(Exception)
                                {

                                }
                                File.Move(path, "./Keys/Nonlocal/" + input);
                            }
                        }
                        break;
                    default: break;
                }
            }
        }

        //--------------------------------SOME OLD CODE-------------------------
        //static void OLDER_IDEA()
        //{
        //    Dictionary<int, string> Vault = new Dictionary<int, string>();
        //    static void draw() { };
        //    {
        //        int counter = 1;
        //        while (counter < 400)
        //        { Vault.Add(counter, null); ++counter; }
        //        counter = 0;
        //        int counter2 = 0;
        //        while (counter < 100)
        //        {
        //            ++counter;
        //            counter2 = 0;
        //            while (counter2 < 40)
        //            {
        //                ++counter2;
        //                if (Vault[counter2] != null) { Console.Write("X"); }
        //                else { Console.Write("O"); }
        //            }
        //            Console.WriteLine("");
        //        }
        //        draw();
        //        Console.ReadKey();
        //    }
        //}
    }
}
