using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace AdminManager.Controllers
{
    [Route("api/download")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private readonly IHostEnvironment _env;
        public DownloadController(IHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return Content("Hello");
        }

        [HttpGet("csv/{filename}")]
        public IActionResult DownloadCSV(string filename)
        {
            var filePath = Path.Combine(_env.ContentRootPath, "download", $"{filename}.csv");

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                memory.Write(new byte[] { 0xEF, 0xBB, 0xBF }, 0, 3);
                stream.CopyTo(memory);
            }
            memory.Position = 0;
            return File(memory, "application/csv; charset=UTF-8", $"{filename}.csv");
        }
    }
}
