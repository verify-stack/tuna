// stores settings to be used
// uhhh, that's really it

namespace Roblox.Configuration.Properties
{
    public class Settings
    {
        // URL Endpoints
        public static string BaseURL { get; } = "http://%s.roblox.com";
        public static string APIEndpoint { get; } = "api";
        public static string MainEndpoint { get; } = "www";
        public static string DeployEndpoint { get; } = "setup";

        public static string GetURL(string endpoint) {  return BaseURL.Replace("%s", endpoint); }

        // Important locations of files
        public static string KeysFolder = "Keys";
        public static string LuaFolder = "Lua";

        public static string PrimaryKey = "PrivateKey.pem";
        public static bool UseNewRBXSig = true; // i guess if someone wants 2012 clients we can use this
    }
}
