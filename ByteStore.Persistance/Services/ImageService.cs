using ByteStore.Domain.Abstractions.Result;
using BytStore.Application.DTOs.Product;
using BytStore.Application.Helpers;
using BytStore.Application.IServices;
using Imagekit.Sdk;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ByteStore.Persistance.Services
{
    internal class ImageService : IImageService
    {
        private readonly ImagekitClient _ik;
        private readonly ImageKitOptions _opt;

        public ImageService(IOptions<ImageKitOptions> opt)
        {
            _opt = opt.Value;
            _ik = new ImagekitClient(_opt.PublicKey, _opt.PrivateKey, _opt.UrlEndpoint);
        }

        public async Task<Result2<ImageUploadDto>> UploadAsync(IFormFile file, string folder = "/", CancellationToken ct = default)
        {
            if (file == null || file.Length == 0)
                return Result2<ImageUploadDto>.Failure(FileErrors.Empty);

            if (!file.ContentType.StartsWith("image/"))
                return Result2<ImageUploadDto>.Failure(FileErrors.OnlyImage);

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
            return Result2<ImageUploadDto>.Success(new ImageUploadDto(response.url, response.fileId, response.name));
        }

        public async Task<Result2> DeleteAsync(string fileId, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(fileId))
                return Result2.Failure(FileErrors.FileIdNotFound);
            await _ik.DeleteFileAsync(fileId);
            return Result2.Success();
        }

        public async Task<Result2<string>> UpdateImageAsync(string fileId, IFormFile newFile, string folder = "/")
        {

            var deleteResult = await DeleteAsync(fileId);
            if (!deleteResult.IsSuccess)
            {
                return Result2<string>.Failure(deleteResult.Error);
            }

            // Validate new file
            if (newFile == null || newFile.Length == 0)
                return Result2<string>.Failure(FileErrors.Empty);

            if (!newFile.ContentType.StartsWith("image/"))
                return Result2<string>.Failure(FileErrors.OnlyImage);

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

            return Result2<string>.Success(uploadResult.url);
        }
    }
}
