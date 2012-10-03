using System;
using Crypto = System.Security.Cryptography;
using System.Text;
using Winforms = System.Windows.Forms;

namespace Hash_Checker
{
    class Program
    {
        public enum SupportedHashes
        {
            // Supported types are stored here by their character-length when encoded as a hex string
            MD5 = 32,
            SHA1 = 40,
            SHA512 = 128
        }

        [STAThreadAttribute]
        static void Main(string[] args)
        {
            string expectedHash = "";
            string fileToHash = "";

            // Don't run if we don't have any arguments
            if (args.Length == 0)
            {
                Console.WriteLine("Please copy the checksum to your clipboard, and then call Hash-Checker with the context menu for the file you're verifying.");
                Console.ReadKey();
                return;
            }
            else
            {
                foreach (string arg in args)
                {
                    if (System.IO.File.Exists(arg))
                        fileToHash = arg;
                }
                // Don't continue if we have no valid file to check against
                if (fileToHash == "")
                {
                    Console.WriteLine("No files were passed as arguments.");
                    Console.WriteLine("Please call Hash-Checker with the context menu for the file you're verifying.");
                    Console.ReadKey();
                    return;
                }
                // Check the clipboard for a checksum we can use
                if (!checkClipboard(ref expectedHash))
                    return;
            }
            string HashUsed = "";
            Crypto.HashAlgorithm hashGenerator;
            byte[] hashbytes;
            if (expectedHash.Length == (int)SupportedHashes.MD5)
            {
                hashGenerator = Crypto.MD5.Create();
                HashUsed = "MD5";
            }
            else if (expectedHash.Length == (int)SupportedHashes.SHA1)
            {
                hashGenerator = Crypto.SHA1.Create();
                HashUsed = "SHA1";
            }
            else if (expectedHash.Length == (int)SupportedHashes.SHA512)
            {
                hashGenerator = Crypto.SHA512.Create();
                HashUsed = "SHA512";
            }
            else
            {
                Console.WriteLine("Input hash type is not supported.");
                Console.ReadLine();
                return;
            }
            // Make the checksum and build it into the StringBuilder as hex
            StringBuilder checksumBuilder = new StringBuilder();
            hashbytes = hashGenerator.ComputeHash(System.IO.File.ReadAllBytes(fileToHash));
            foreach (byte chunk in hashbytes) checksumBuilder.Append(chunk.ToString("x2"));
            // Check the values
            if (checksumBuilder.ToString().ToLower() == expectedHash.ToLower())
                Console.WriteLine("Hash provided matches calculated hash.");
            else
                Console.WriteLine("Hash provided DOES NOT match calculated hash.");
            Console.WriteLine("Hash Method: " + HashUsed);
            Console.WriteLine();
            Console.WriteLine("Expected Hash: " + expectedHash.ToLower());
            Console.WriteLine("Result Hash:   " + checksumBuilder.ToString().ToLower());
            Console.ReadLine();
        }

        private static bool checkClipboard(ref string expectedHash)
        {
            do
            {
                if (Winforms.Clipboard.ContainsText())
                {
                    // Grab clipboard text
                    expectedHash = Winforms.Clipboard.GetText();
                    // Check for quit or just q
                    if (expectedHash == "quit" || expectedHash == "q")
                        return false;
                    // See if the length matches any supported hash types
                    foreach (SupportedHashes hashType in Enum.GetValues(typeof(SupportedHashes)))
                    {
                        if (expectedHash.Length == (int)hashType)
                            return true;
                    }
                    // If no matches, clear our value
                    expectedHash = "";
                }
                // Let the user know to copy the hash
                Console.WriteLine("Please select and copy the hash to compare the file to.");
                Console.ReadLine();
            } while (true);
        }
    }
}
