using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.Services.AttachementService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly long _maxFileSize = 5 * 1024 * 1024;
        private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png"];
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _env;

        public AttachmentService(ILogger<IAttachmentService> logger, IWebHostEnvironment env )
        {
            _logger = logger;
            _env = env;
        }

        #region Upload
        public async Task<string?> UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default)
        {
            if (fileStream is null || !fileStream.CanRead || fileStream.Length == 0) return null!;
            if (fileStream.Length > _maxFileSize)
            {
                _logger.LogWarning($"Rejected Upload : File Too Large {fileStream.Length} Bytes");
                return null!;
            }
            var extension = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(extension) || !_allowedExtensions.Contains(extension))
            {
                _logger.LogWarning($"Rejected Upload : Extension '{extension}' Is Not Allowed");
                return null!;

            }

            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName)) return null!;

            var folderPath = Path.Combine(_env.ContentRootPath, folderName);
            Directory.CreateDirectory(folderPath);

            var storedFileName = $"{Guid.NewGuid()}{extension}";
            var filepath = Path.Combine(folderPath, storedFileName);

            try
            {
                await using var fs = new FileStream(filepath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                await fileStream.CopyToAsync(fs, ct);
                return storedFileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed To Upload File {fileName}");
                return null!;

            }
        }

        #endregion

        #region GetFile
        public (Stream stream, string contentType)? GetFile(string fileName, string folderName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName))
                return null;

            var filePath = Path.Combine(_env.ContentRootPath, folderName, fileName);
            if (!File.Exists(filePath)) return null;

            var contentType = Path.GetExtension(filePath).ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };

            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return (stream, contentType);

        }

        #endregion

        #region Delete
        public bool Delete(string fileName, string folderName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName))
                return false;

            try
            {
                var filePath = Path.Combine(_env.ContentRootPath, folderName, fileName);
                if (!File.Exists(filePath)) return false;

                File.Delete(filePath);
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed To Delete Attachment {fileName}");
                return false;
            }
        }

        #endregion






    }
}
