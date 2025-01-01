// creates keys for the client to use
// TODO: make an assert function

using System.Text;
using System.Security.Cryptography;
using Roblox.Website;
using System.Drawing;
using System.IO;

namespace Roblox.KeyHelper
{
    internal class Program
    {
        static string[] VaildArgs = {"-s", "-g", "-l", "-h"};

        private static Dictionary<string, string> ReadOptions(string[] args, string[] vaildargs) {
            var OptionsList = new Dictionary<string, string>();

            for (int i = 0; i < args.Length; i++) {
                if (vaildargs.Contains(args[i]) && i + 1 < args.Length) {
                    OptionsList[args[i]] = args[i + 1];
                    i++;
                }
            }

            return OptionsList;
        }

        private static void HelpMenu() {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Roblox.KeyHelper - A Tuna application.");
            Console.ResetColor();

            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Welcome to the Roblox.KeyHelper application.");
            Console.WriteLine("This small application allows you to generate keys and sign scripts.");
            Console.WriteLine("--------------------------------------");

            Console.WriteLine("List of commands:");
            Console.WriteLine("-> -h : This help menu incase you forget!");
            Console.WriteLine("-> -s [PrivateKey.pem] [MyScript.lua] : Goes into signing mode. Just signs the script with the key.");
            Console.WriteLine("-> -g [options] : Generates the required keys.");
            Console.WriteLine("---> -l [Length] : The length of the keys. By default it's 1024.");
        }

        static int Main(string[] args) {
            if (args.Length == 0) {
                HelpMenu();
                return 0;
            }

            Dictionary<string, string> CurrentOptions = ReadOptions(args, VaildArgs);

            if (CurrentOptions.ContainsKey("-h")) {
                HelpMenu();
                return 0;
            }

            if (CurrentOptions.ContainsKey("-s")) {
                string? FileName = args[CurrentOptions.Count + 1];
                if (FileName == null) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("FATAL ERROR");
                    Console.ResetColor();
                    Console.WriteLine("There is no given file name! Please put it in the format [PrivateKey.pem] [MyFile.lua]");

                    return 1;
                }

                string? SignFile = Utils.ReadFile(FileName);
                if (SignFile == null) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("FATAL ERROR");
                    Console.ResetColor();
                    Console.WriteLine("The given file doesn't exist! Please make sure it exists.");

                    return 1;
                }

                string? PrivateKey = Utils.ReadFile(CurrentOptions["-s"]);
                if (PrivateKey == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("FATAL ERROR");
                    Console.ResetColor();
                    Console.WriteLine("The given private key doesn't exist! Please make sure it exists.");

                    return 1;
                }

                string SignedFile = Utils.SignFile(SignFile, PrivateKey) ?? "For some reason Utils failed to sign the file...";
                if (PrivateKey == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("FATAL ERROR");
                    Console.ResetColor();
                    Console.WriteLine("The given private key doesn't exist! Please make sure it exists.");

                    return 1;
                }

                string? WritenFile = Utils.WriteFile(FileName + ".signed", SignedFile);
                if (WritenFile != null) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("FATAL ERROR");
                    Console.ResetColor();
                    Console.WriteLine($"Cannot write signed file because of {WritenFile}");

                    return 1;
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Signed the file! Be sure to the check given file or to check if it's proper");
                Console.ResetColor();

                return 0;
            }

            if (CurrentOptions.ContainsKey("-g"))
            {
                int RSASize = 1024;
                if (CurrentOptions.ContainsKey("-l"))
                    Int32.TryParse(CurrentOptions["-l"], out RSASize);
                using RSACryptoServiceProvider GenRSA = new(RSASize);
                Console.WriteLine($"Size set to {RSASize}");

                string PrivateKey = GenRSA.ExportRSAPrivateKeyPem();
                string PublicKey = GenRSA.ExportRSAPublicKeyPem();
                byte[] PrivateKB = GenRSA.ExportCspBlob(true);
                byte[] PublicKB = GenRSA.ExportCspBlob(false);
                
                Console.WriteLine("Exported all keys and key blobs.");

                Directory.CreateDirectory("out");
                File.WriteAllBytes("out/PrivateKeyBlob.txt", Encoding.UTF8.GetBytes(Convert.ToBase64String(PrivateKB)));
                File.WriteAllBytes("out/PublicKeyBlob.txt", Encoding.UTF8.GetBytes(Convert.ToBase64String(PublicKB)));
                File.WriteAllBytes("out/PrivateKey.pem", Encoding.UTF8.GetBytes(PrivateKey));
                File.WriteAllBytes("out/PublicKey.pem", Encoding.UTF8.GetBytes(PublicKey));

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Written all of the keys, be sure to check for any errors in the files.");
                Console.ResetColor();

                return 0;
            }

            return 0;
        }
    }
}