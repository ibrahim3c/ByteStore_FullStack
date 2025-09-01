using ByteStore.Domain.Abstractions;
using BytStore.Application.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace BytStore.Application.IServices
{
    public interface IImageService
    {
        Task<Result<ImageUploadDto>> UploadAsync(IFormFile file, string folder = "/", CancellationToken ct = default);
        Task<ByteStore.Domain.Abstractions.Result> DeleteAsync(string fileId, CancellationToken ct = default);
        Task<Result<string>> UpdateImageAsync(string fileId, IFormFile newFile, string folder = "/");
    }
}
