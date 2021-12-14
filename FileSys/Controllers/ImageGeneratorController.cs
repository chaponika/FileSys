using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FileSys.Controllers
{
    public class ImageGeneratorController : Controller
    {
        private static readonly string[] imageFileTypesArray = new string[]{".bmp",".jpg",".jpeg",".tif", ".tiff",".png"};
        private static readonly HashSet<string> imageTypes = new HashSet<string>(imageFileTypesArray);

        
        //TODO async?
        public async Task<IActionResult> ImageGeneratoActionResult(string filePath)
        {
            byte[] image = await System.IO.File.ReadAllBytesAsync(filePath);
            string format = filePath.Substring(filePath.LastIndexOf("."));
            string imreBase64Data = Convert.ToBase64String(image);  
            string imgDataURL = string.Format("data:image/"+format+";base64,{0}", imreBase64Data);  
            //Passing image data in viewbag to view
            ViewBag.ImageData = imgDataURL;  
            //File(image, "image/"+format);
            return View();
        }
    }
}