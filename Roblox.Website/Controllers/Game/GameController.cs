// this handles the basic APIs required for joining/hosting
/*
    /game (GameController.cs)
        /join.ashx - connects to a server
        /gameserver.ashx - hosts a server
        /visit.ashx - loads a map, creates a local instance of a player, then runs the game.
        /studio.ashx - connects the required apis and loads the starterscript
    
    this folder also includes social services for the client. that's for later...
*/

using Microsoft.AspNetCore.Mvc;
using Roblox.Configuration;

namespace Roblox.Website.Controllers.Game
{
    [Route("Game")]
    public class GameController : Controller
    {
        // TODO: connect the db and this dependent on the db
        private IActionResult ProcessLuaFile(string file, Dictionary<string, string> ReplaceList)
        {
            if (!Settings.IsClientsSetup)
            {
                return NotFound();
            }

            Console.WriteLine($"Reading {file}.lua");
            string? SignFile = Utils.ReadFile($"{Utils.GetSolutionPath()}\\Roblox.Configuration\\{Settings.LuaFolder}\\{file}.lua");
            if (SignFile == null)
                return Problem($"The server had a problem finding the required file to sign. FILE: {SignFile}.lua", null, 500, null, null);

            Console.WriteLine($"Setting up {file}.lua");
            foreach (var KV in ReplaceList) {
                SignFile = SignFile.Replace(KV.Key, KV.Value);
            };

            Console.WriteLine($"Signing {file}.lua");
            SignFile = Utils.SignFile(SignFile, null) ?? "Failed to sign script!";

            return Content(SignFile, "text/plain");
        }

        public IActionResult Index()
        {
            return Forbid();
        }

        [HttpGet("Join.ashx")]
        public IActionResult Join()
        {
            var ReplaceList = new Dictionary<string, string> 
            {
                {"_burl", Settings.GetURL(Settings.MainEndpoint)},
                {"_aurl", Settings.GetURL(Settings.APIEndpoint)},
                {"_pid", "-1"},
                {"_cid", "1"},
                {"_port", "53640"},
                {"_host", "localhost"},
                {"_schat", "false"},
                {"_mem", "None"},
                {"_age", "365"},
            };

            return ProcessLuaFile("Join", ReplaceList);
        }

        [HttpGet("Gameserver.ashx")]
        public IActionResult Gameserver()
        {
            // TODO: add the assets (maybe connected to the db!)
            var ReplaceList = new Dictionary<string, string> 
            {
                {"...", $"1, 53640, 0, \"access=this is not required LOL\", \"{Settings.GetURL(Settings.MainEndpoint)}\", 1, 1, 60, nil, nil, nil, nil, nil, nil, nil, nil, nil, nil, 1, \"{Settings.GetURL(Settings.MainEndpoint)}\", nil, nil, 2"}
            };

            return ProcessLuaFile("Gameserver", ReplaceList);
        }

        [HttpGet("Visit.ashx")]
        public IActionResult Visit()
        {
            var ReplaceList = new Dictionary<string, string> 
            {
                {"_burl", Settings.GetURL(Settings.MainEndpoint)},
                {"_aurl", Settings.GetURL(Settings.APIEndpoint)},
                {"_name", "Guest 6921"},
            };

            return ProcessLuaFile("Visit", ReplaceList);
        }

        [HttpGet("Studio.ashx")]
        public IActionResult Studio()
        {
            var ReplaceList = new Dictionary<string, string> 
            {
                {"_burl", Settings.GetURL(Settings.MainEndpoint)},
                {"_aurl", Settings.GetURL(Settings.APIEndpoint)},
            };

            return ProcessLuaFile("Studio", ReplaceList);
        }
    }
}
