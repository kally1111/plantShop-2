using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace PlantShop.Services
{
    public class FileService:IFileService
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        public FileService( IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        public string Upload(IFormFile file)
        {
            var uploadDirecotroy = "img/";
            var uploadPath = Path.Combine(webHostEnvironment.WebRootPath, uploadDirecotroy);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var strem = File.Create(filePath))
            {
                file.CopyTo(strem);
            }
            return fileName;
        }
        public void DeleteFile(string fileName)
        {
            string imgToDelate = Path.Combine(webHostEnvironment.WebRootPath, "img\\", fileName);

            if (System.IO.File.Exists(imgToDelate))
            {
                System.IO.File.Delete(imgToDelate);
            }
        }
    }
}
