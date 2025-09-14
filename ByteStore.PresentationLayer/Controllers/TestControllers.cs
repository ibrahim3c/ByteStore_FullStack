using ByteStore.PresentationLayer.Controllers;
using BytStore.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ByteStore.Presentation.Controllers
{
    public class TestControllers : BaseController
    {
        public TestControllers(IServiceManager serviceManager) : base(serviceManager)
        {
        }
        // POST: api/email/send
        [HttpPost("send")]
        public async Task<IActionResult> SendEmail(
            [FromForm] string to,
            [FromForm] string subject,
            [FromForm] string body,
            [FromForm] List<IFormFile> files)
        {
            if (string.IsNullOrWhiteSpace(to))
                return BadRequest("Recipient email is required");

            await serviceManager.EmailService.SendMailByBrevoAsync(to, subject, body, files);

            return Ok(new { message = $"Email sent successfully to {to}" });
        }

        // ✅ Upload Image
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromQuery] string folder = "/", CancellationToken ct = default)
        {
            var result = await serviceManager.ImageService.UploadAsync(file, folder, ct);

            if (!result.IsSuccess)
                return BadRequest(result.Error.Description);

            return Ok(result.Value);
        }

        // ✅ Delete Image
        [HttpDelete("{fileId}")]
        public async Task<IActionResult> Delete(string fileId, CancellationToken ct = default)
        {
            var result = await serviceManager.ImageService.DeleteAsync(fileId, ct);

            if (!result.IsSuccess)
                return BadRequest(result.Error.Description);

            return Ok(new { message = "Image deleted successfully" });
        }

        // ✅ Update Image
        [HttpPut("update/{fileId}")]
        public async Task<IActionResult> Update(string fileId, [FromForm] IFormFile newFile, [FromQuery] string folder = "/", CancellationToken ct = default)
        {
            var result = await serviceManager.ImageService.UpdateImageAsync(fileId, newFile, folder);

            if (!result.IsSuccess)
                return BadRequest(result.Error.Description);

            return Ok(new { url = result.Value });
        }


    }

}
