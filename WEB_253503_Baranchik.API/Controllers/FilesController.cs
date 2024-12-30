using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WEB_253503_Baranchik.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly string _imagePath;

        public FilesController(IWebHostEnvironment webHost)
        {
            _imagePath = Path.Combine(webHost.WebRootPath, "Images");
        }

        [HttpPost]
        public async Task<IActionResult> SaveFile(IFormFile file)
        {
            if (file is null)
            {
                return BadRequest();
            }
            var filePath = Path.Combine(_imagePath, file.FileName);
            var fileInfo = new FileInfo(filePath);

            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }

            using var fileStream = fileInfo.Create();
            await file.CopyToAsync(fileStream);

            var host = HttpContext.Request.Host;
            var fileUrl = $"Https://{host}/Images/{file.FileName}";

            return Ok(fileUrl);

        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFile()
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var url = await reader.ReadToEndAsync();
                string fileName = Path.GetFileName(url);
                if (string.IsNullOrEmpty(fileName))
                {
                    return BadRequest("File name cannot be empty.");
                }

                string path = Path.Combine(_imagePath, fileName);
                var fileInfo = new FileInfo(path);

                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                    return Ok();
                }

                return NotFound("File not found.");
            }
        }
    }
}
