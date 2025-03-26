using Microsoft.AspNetCore.Mvc;

namespace AdminManager.Controllers
{
    [ApiController]
    [Route("api/upload")]
    public class UploadController : ControllerBase
    {
        private readonly IHostEnvironment _env;
        public UploadController(IHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return Content("Hello");
        }

        [HttpPost("file")]
        public async Task<IActionResult> File(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    var path = Path.Combine(_env.ContentRootPath, "upload");
                    
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    var filePath = Path.Combine(path, Path.GetFileName(file.FileName));
                    await using Stream fileStream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return StatusCode(200);
        }
    }
}
