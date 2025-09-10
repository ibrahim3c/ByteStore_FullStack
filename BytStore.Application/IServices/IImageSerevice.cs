using ByteStore.Domain.Abstractions.Result;
using BytStore.Application.DTOs.Product;
using Microsoft.AspNetCore.Http;

namespace BytStore.Application.IServices
{
    public interface IImageService
    {
        Task<Result2<ImageUploadDto>> UploadAsync(IFormFile file, string folder = "/", CancellationToken ct = default);
        Task<Result2> DeleteAsync(string fileId, CancellationToken ct = default);
        Task<Result2<string>> UpdateImageAsync(string fileId, IFormFile newFile, string folder = "/");
    }
}
