using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystem.BLL.Services.AttachementService
{
    public interface IAttachmentService
    {
        Task<string?>UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default);

        (Stream stream, string contentType)? GetFile(string fileName, string folderName);


        bool Delete(string fileName, string folderName);

 
    }
}
