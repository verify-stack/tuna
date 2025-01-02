// stores settings to be used
// uhhh, that's really it

namespace Roblox.Configuration
{
    public class Settings
    {
        // Database info
        public static string MainConnection { get; } = "Host=localhost; Database=Roblox; Username=postgres; Password=1234";

        // URL Endpoints
        public static string BaseURL { get; } = "http://%s.roblox.com";
        public static string APIEndpoint { get; } = "api";
        public static string MainEndpoint { get; } = "www";
        public static string DeployEndpoint { get; } = "setup";

        public static string GetURL(string endpoint) { return BaseURL.Replace("%s", endpoint); }

        // Clients
        public static bool IsClientsSetup { get; } = true;
        public static bool UseNewRBXSig { get; } = true; // i guess if someone wants 2012 clients we can use disable this

        public static string KeysFolder { get; } = "Keys";
        public static string LuaFolder { get; } = "Lua";
        public static string PrimaryKey { get; } = "PrivateKey.pem";
    }
}
