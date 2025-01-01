// grabs a json file and reads it, then collects the CSS files listed and spits out the combined css file

using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Roblox.Configuration;

namespace Roblox.Website.Controllers
{
    [Route("CSS/Base/CSS/[controller]")]
    public class FetchCSSController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public FetchCSSController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] string path)
        {
            // TODO: errors becomes 302 to RobloxDefaultErrorPage.aspx
            Console.WriteLine($"Fetching CSS {path}");

            if (string.IsNullOrEmpty(path))
            {
                return BadRequest("Path query parameter is required.");
            }

            int DotPoint = path.IndexOf('.');
            if (DotPoint == -2)
            {
                return BadRequest("Path must include \".css\".");
            }
            path = path.Substring(0, DotPoint);

            Console.WriteLine($"Spliting CSS {path}");
            string FolderName, HashName, Method = "";
            string[] SplicedSegments = path.Split("___");
            FolderName = SplicedSegments[0];

            SplicedSegments = SplicedSegments[1].Split('_');
            HashName = SplicedSegments[0];
            if (SplicedSegments.Length == 2) // prevent an error
                Method = SplicedSegments[1];

            Console.WriteLine($"Reading JSON {HashName}.json");
            string CSSResult = "";
            string CSSInfoPath = $"{_env.ContentRootPath}\\Controllers\\CSS\\{FolderName}\\{HashName}.json";
            string? JSONContent = Utils.ReadFile(CSSInfoPath);
            if (JSONContent == null)
            {
                return NotFound("JSON doesn't exist " + CSSInfoPath);
            }

            Console.WriteLine($"Reading JSON {HashName}.json and CSS files.");
            string[] CSSFiles = JsonSerializer.Deserialize<string[]>(JSONContent) ?? [];
            foreach (string CSSFile in CSSFiles)
            {
                string FilePath = _env.WebRootPath + CSSFile;
                CSSResult += $"\n/*{CSSFile}*/\n{Utils.ReadFile(FilePath) ?? "/* File doesn't exist... */"}"; // the fetchcss files includes ~ so we do it!
            }
            if (Method == "m")
                CSSResult = CSSResult.Trim();

            return Content(CSSResult, "text/css");
        }
    }
}