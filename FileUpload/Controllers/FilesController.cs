using FileUpload.Entities;
using FileUpload.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace FileUpload.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase {
        private readonly IFileService _uploadService;

        public FilesController(IFileService uploadService) {
            _uploadService = uploadService;
        }

        [HttpPost("PostSingleFile")]
        public async Task<ActionResult> PostSingleFile([FromForm] FileUploadModel fileDetails) {
            if (fileDetails == null) {
                return BadRequest();
            }

            await _uploadService.PostFileAsync(fileDetails.FileDetails, fileDetails.FileType);
            return Ok();
        }

        [HttpPost("PostMultipleFile")]
        public async Task<ActionResult> PostMultipleFile([FromForm] List<FileUploadModel> fileDetails) {
            if (fileDetails == null) {
                return BadRequest();
            }

            await _uploadService.PostMultiFileAsync(fileDetails);
            return Ok();
        }

        [HttpGet("DownloadFile")]
        public async Task<IActionResult> DownloadFile(int id) {
            if (id < 1) {
                return NotFound();
            }
            var file = await _uploadService.GetFileById(id);
            var fileStream = new MemoryStream(file.FileData);
            new FileExtensionContentTypeProvider().TryGetContentType(file.FileName, out string contentType);
            if (string.IsNullOrEmpty(contentType)) {
                return BadRequest();
            }
            return File(fileStream, contentType, file.FileName);
        }
    }
}