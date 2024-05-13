using Anyhandy.Models.DTOs;
using Anyhandy.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anyhandy.Interface
{
    public interface IFileStorage
    {
        //Task<byte[]> GetFileContentAsync(string key);
        Task UploadFileAsync(string key, byte[] content, string contentType);
    }
}
