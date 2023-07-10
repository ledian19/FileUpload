using FileUpload.Data;
using FileUpload.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileUpload.Services {
    public class FileService : IFileService {
        private readonly DbContextClass _dbContextClass;

        public FileService(DbContextClass dbContextClass) {
            _dbContextClass = dbContextClass;
        }

        public async Task PostFileAsync(IFormFile fileData, FileType fileType) {
            var fileDetails = new FileDetails {
                Id = 0,
                FileName = fileData.FileName,
                FileType = fileType
            };

            using (var stream = new MemoryStream()) {
                await fileData.CopyToAsync(stream);
                fileDetails.FileData = stream.ToArray();
            }

            _dbContextClass.FileDetails.Add(fileDetails);
            await _dbContextClass.SaveChangesAsync();
        }

        public async Task PostMultiFileAsync(List<FileUploadModel> fileData) {
            foreach (FileUploadModel file in fileData) {
                var fileDetails = new FileDetails {
                    Id = 0,
                    FileName = file.FileDetails.FileName,
                    FileType = file.FileType,
                };

                using (var stream = new MemoryStream()) {
                    await file.FileDetails.CopyToAsync(stream);
                    fileDetails.FileData = stream.ToArray();
                }

                _dbContextClass.FileDetails.Add(fileDetails);
            }

            await _dbContextClass.SaveChangesAsync();
        }

        public async Task<FileDetails> GetFileById(int id) {
            var file = await _dbContextClass.FileDetails.Where(x => x.Id == id).FirstOrDefaultAsync();
            return file;
        }
    }
}