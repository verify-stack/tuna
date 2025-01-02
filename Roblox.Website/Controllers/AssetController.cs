// get assets via cache or the roblox website
using Microsoft.AspNetCore.Mvc;
using Roblox.Configuration;

namespace Roblox.Website.Controllers
{
    [Route("Asset")]
    public class AssetController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private HttpClient _httpClient;

        public AssetController(IWebHostEnvironment env) 
        {
            _env = env;
            _httpClient = new HttpClient();
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync([FromQuery] string id) 
        {
            if (!Settings.IsClientsSetup)
            {
                return NotFound();
            }

            // again, ward off bad requests TO error pages
            if (!int.TryParse(id, out int AssetId))
                return BadRequest("AssetId must be a valid number.");

            Console.WriteLine($"Finding AssetId {AssetId}.");
            string AssetCachePath = $"{_env.ContentRootPath}\\Controllers\\Asset\\{AssetId}";
            string? AssetFile = Utils.ReadFile(AssetCachePath);

            if (AssetFile == null) {
                Console.WriteLine($"Checking if AssetId {AssetId} is a corescript.");

                AssetFile = Utils.ReadFile(AssetCachePath + ".core"); // core script check
                if (AssetFile != null) {
                    Console.WriteLine($"AssetId {AssetId} is a corescript.");
                    AssetFile = $"--rbxassetid%{AssetId}%\r\n" + AssetFile;
                    AssetFile = Utils.SignFile(AssetFile, null) ?? string.Empty;
                } else if (AssetFile == null) {
                    // the asset is not in the cache, grab it from roblox's api
                    Console.WriteLine($"Failed to find AssetId {AssetId}, getting from the ROBLOX API.");

                    HttpResponseMessage HttpRep = await _httpClient.GetAsync($"https://assetdelivery.roblox.com/v1/asset/?ID={AssetId}");
                    if (!HttpRep.IsSuccessStatusCode)
                    {
                        return Problem($"Failed to grab AssetId {AssetId} from ROBLOX's API", null, 500, null, null);
                    }
                    AssetFile = await HttpRep.Content.ReadAsStringAsync() ?? string.Empty;
                }
            }

            AssetFile = AssetFile.Replace("_burl", Settings.GetURL(Settings.MainEndpoint)); // make sure to replace _burl with OUR domain
            if (AssetFile == null) {
                Console.WriteLine($"Cannot find AssetId {AssetId}.");
                return NotFound();
            }

            Console.WriteLine($"Done finding AssetId {AssetId}.");
            return Content(AssetFile, "text/plain");
        }

        [HttpGet("CharacterFetch.ashx")]
        public IActionResult CharacterFetch() 
        {
            if (!Settings.IsClientsSetup)
            {
                return NotFound();
            }

            // TODO: make this dependent on the DB
            return Content($"{Settings.GetURL(Settings.MainEndpoint)}/asset?id=585442799;", "text/plain");
        }

        [HttpGet("GetScriptState.ashx")]
        public IActionResult GetScriptState() 
        {
            if (!Settings.IsClientsSetup)
            {
                return NotFound();
            }

            return Content("0 0 0 00 0 0 0", "text/plain");
        }
    }
 }
