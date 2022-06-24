using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlantShop.Services
{
    public interface IFileService
    {
        string Upload(IFormFile photo);
        void DeleteFile(string fileName);


    }
}
