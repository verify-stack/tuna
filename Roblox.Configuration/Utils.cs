// pretty cool utils

using Roblox.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Roblox.Website
{
    public class Utils
    {
        public static string? GetSolutionPath() {
            string CurrentDir = Directory.GetCurrentDirectory();
            string? SolutionDir = Directory.GetParent(CurrentDir)?.FullName;
            if (SolutionDir == null)
                throw new Exception("Unable to get the solution directory.");

            return SolutionDir;
        }

        public static string? ReadFile(string path) {
            if (!File.Exists(path))
                return null;

            try {
                return File.ReadAllText(path);
            } catch {
                return null;
            }
        }

        public static string? WriteFile(string path, string content) {
            try
            {
                File.WriteAllText(path, content);
                return null;
            }
            catch
            {
                return "Failed to write file!";
            }
        }

        public static bool FileExists(string path) {
            return File.Exists(path);
        }

        public static string? SignFile(string data, string? pem) {
            string? PrivateKey = ReadFile($"{GetSolutionPath()}\\Roblox.Configuration\\{Settings.KeysFolder}\\{Settings.PrimaryKey}");
            if (pem != null)
                PrivateKey = ReadFile(pem);
            
            if (PrivateKey == null)
                return null;

            data = "\r\n" + data;
            using RSA SignRSA = RSA.Create();
            SignRSA.ImportFromPem(PrivateKey.ToCharArray());

            byte[] SigBytes = SignRSA.SignData(Encoding.UTF8.GetBytes(data), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
            string SigStart = Settings.UseNewRBXSig ? "--rbxsig%" : "%";

            return $"{SigStart}{Convert.ToBase64String(SigBytes)}%{data}";
        }
    }
}