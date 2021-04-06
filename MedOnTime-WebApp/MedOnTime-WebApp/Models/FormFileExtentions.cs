using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MedOnTime_WebApp.Models
{
    // Extention implementation:
    //var bytes = await formFile.GetBytes();
    //var hexString = Convert.ToBase64String(bytes);

    // Extention Stack Overflow Source: https://stackoverflow.com/questions/36432028/how-to-convert-a-file-into-byte-array-in-memory

    public static class FormFileExtensions
    {
        public static async Task<byte[]> GetBytes(this IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
