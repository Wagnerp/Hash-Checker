using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHA_1_Hash_Checker
{
    class Program
    {
        private enum Modes
        {
            FromFile,
            BrowseForIt
        }

        [STAThreadAttribute]
        static void Main(string[] args)
        {
            Modes ourMode = 0;
            byte[] fileHash_SHA1 = { };
            byte[] fileHash_SHA512 = { };
            byte[] fileHash_MD5 = { };
            string expectedHash = "";
            System.Security.Cryptography.SHA1 SHA1_Hasher = System.Security.Cryptography.SHA1.Create();
            System.Security.Cryptography.SHA512 SHA512_Hasher = System.Security.Cryptography.SHA512.Create();
            System.Security.Cryptography.MD5 MD5_Hasher = System.Security.Cryptography.MD5.Create();

            if (args.Length == 0)
            {
                ourMode = Modes.BrowseForIt;
                System.Windows.Forms.OpenFileDialog fileBrowser = new System.Windows.Forms.OpenFileDialog();
                fileBrowser.ShowDialog();
                fileHash_SHA1 = SHA1_Hasher.ComputeHash(System.IO.File.ReadAllBytes(fileBrowser.FileName));
                fileHash_MD5 = MD5_Hasher.ComputeHash(System.IO.File.ReadAllBytes(fileBrowser.FileName));

                Console.WriteLine("What's the Hash we're comparing to?");
                expectedHash = Console.ReadLine();
            }
            else
            {
                ourMode = Modes.FromFile;
                foreach (string arg in args)
                {
                    if (arg == "/o")
                    {
                        ourMode = Modes.FromFile;
                    }
                    else if (System.IO.File.Exists(arg))
                    {
                        fileHash_SHA512 = SHA512_Hasher.ComputeHash(System.IO.File.ReadAllBytes(arg));
                        fileHash_SHA1 = SHA1_Hasher.ComputeHash(System.IO.File.ReadAllBytes(arg));
                        fileHash_MD5 = MD5_Hasher.ComputeHash(System.IO.File.ReadAllBytes(arg));
                    }
                }
                int i = 0;
                do
                {
                    int textLength = System.Windows.Forms.Clipboard.GetText().Length;
                    if (System.Windows.Forms.Clipboard.ContainsText() && (textLength == 40 || textLength == 32 || textLength == 128))
                        break;
                    Console.WriteLine("Please select and copy the hash to compare the file to.");
                    Console.ReadLine();
                    i++;
                } while (i < 5);
                if (i == 5)
                    return;
                expectedHash = System.Windows.Forms.Clipboard.GetText().ToLower();
            }

            StringBuilder strBuild = new StringBuilder();
            string HashUsed = "";
            if (expectedHash.Length == 32)
            {
                HashUsed = "MD5";
                foreach (byte chunk in fileHash_MD5)
                    strBuild.Append(chunk.ToString("x2"));
            }
            else if (expectedHash.Length == 40)
            {
                HashUsed = "SHA1";
                foreach (byte chunk in fileHash_SHA1)
                    strBuild.Append(chunk.ToString("x2"));
            }
            else if (expectedHash.Length == 128)
            {
                HashUsed = "SHA512";
                foreach (byte chunk in fileHash_SHA512)
                    strBuild.Append(chunk.ToString("x2"));
            }
            else
            {
                Console.WriteLine("Input hash is not a supported hash.");
                Console.ReadLine();
                return;
            }
            if (strBuild.ToString().ToLower() == expectedHash.ToLower())
                Console.WriteLine("They match!");
            else
                Console.WriteLine("They don't match!");
            Console.WriteLine("Hash Method: " + HashUsed);
            Console.WriteLine();
            Console.WriteLine("Expected Hash: " + expectedHash.ToLower());
            Console.WriteLine("Result Hash:   " + strBuild.ToString().ToLower());
            Console.ReadLine();
        }
    }
}
