using ByteStore.Domain.Abstractions;
using BytStore.Application.DTOs;
using BytStore.Application.Helpers;
using Imagekit.Sdk;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace BytStore.Application.Services
{
    public class ImageService
    {
        private readonly ImagekitClient _ik;
        private readonly ImageKitOptions _opt;

        public ImageService(IOptions<ImageKitOptions> opt)
        {
            _opt = opt.Value;
            _ik = new ImagekitClient(_opt.PublicKey, _opt.PrivateKey, _opt.UrlEndpoint);
        }

        public async Task<Result<ImageUploadDto>> UploadAsync(IFormFile file,string folder="/", CancellationToken ct = default)
        {
            if (file == null || file.Length == 0)
                return Result<ImageUploadDto>.Failure(new List<string> { "File is empty." });

            if (!file.ContentType.StartsWith("image/"))
                return Result<ImageUploadDto>.Failure(new List<string> { "Only image files allowed." });

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            var uploadRequest = new FileCreateRequest
            {
                file = fileBytes,                    
                fileName = file.FileName,
                folder = $"/{folder}"     
            };

            var response = await _ik.UploadAsync(uploadRequest);
            return Result<ImageUploadDto>.Success(new ImageUploadDto(response.url, response.fileId, response.name));
        }

        public async Task<ByteStore.Domain.Abstractions.Result> DeleteAsync(string fileId, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(fileId))
            return ByteStore.Domain.Abstractions.Result.Failure(new List<string> { "fileId is required." });
            await _ik.DeleteFileAsync(fileId);
            return ByteStore.Domain.Abstractions.Result.Success();
        }

        public async Task<Result<string>> UpdateImageAsync(string fileId, IFormFile newFile, string folder = "/")
        {

            var deleteResult = await DeleteAsync(fileId);
            if (!deleteResult.IsSuccess)
            {
                return Result<string>.Failure(deleteResult.Errors);
            }

            // Validate new file
            if (newFile == null || newFile.Length == 0)
                return Result<string>.Failure(new List<string> { "New file is empty." });

            if (!newFile.ContentType.StartsWith("image/"))
                return Result<string>.Failure(new List<string> { "Only image files allowed." });

            // Upload the new image
            using var memoryStream = new MemoryStream();
            await newFile.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            var uploadResult = await _ik.UploadAsync(new FileCreateRequest
            {
                file = fileBytes,
                fileName = newFile.FileName,
                folder = $"/{folder}"
            });

            return Result<string>.Success(uploadResult.url);
        }
    }
}
