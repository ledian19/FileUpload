using FileUpload.Entities;
using FileUpload.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult> DownloadFile(int id) {
            if (id < 1) {
                return BadRequest();
            }

            await _uploadService.DownloadFileById(id);
            return Ok();
        }
    }
}