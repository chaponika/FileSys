using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using FileSys.Models;
using Microsoft.AspNetCore.Mvc;


namespace FileSys.Controllers
{
    
    [Route("files")]
    public class PathController : Controller
    {
        private static readonly string[] imageFileTypesArray = new string[]{".bmp",".jpg",".jpeg",".tif", ".tiff",".png"};
        private static readonly HashSet<string> imageTypes = new HashSet<string>(imageFileTypesArray);

        [HttpGet("rezgs/{*path}")]
        public IActionResult displayFilesView(string path)
        {
            if (path.LastIndexOf(".") != -1 && imageTypes.Contains(path.Substring(path.LastIndexOf("."))))
            {
                return RedirectToAction("ImageGenerator","ImageGenerator",new{imagePath = path});
            }

            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                FileInfo[] files = dirInfo.GetFiles("*.*");
                DirectoryInfo[] dirInfos = dirInfo.GetDirectories("*.*");

                FilesModel[] filesmodels = new FilesModel[dirInfos.Length + files.Length];
                for (int i = 0; i < dirInfos.Length; i++)
                {
                    filesmodels[i] = new FilesModel(dirInfos, i);
                }

                string[] filePaths = Directory.GetFiles(path);
                for (int i=0; i <files.Length; i++)
                {
                    var file = files[i];
                    filesmodels[dirInfos.Length + i] = new FilesModel();
                    //
                    //System.Drawing.Icon iconForFile = System.Drawing.Icon.ExtractAssociatedIcon(file.FullName);
                    //
                    filesmodels[dirInfos.Length+i].name = file.Name;
                    filesmodels[dirInfos.Length+i].path = file.FullName;
                    filesmodels[dirInfos.Length+i].isFolder = false;
                    //
                    //Image image = iconForFile.ToBitmap();
                    //
                    //if (filesmodels[dirInfos.Length+i].path.Substring(filesmodels[dirInfos.Length+i].path.LastIndexOf(".")).Equals(".txt"))
                    //{
                      //  filesmodels[dirInfos.Length+i].Icon = @"~/Images/txt-file-icon.png";
                    //}else if (imageTypes.Contains(filesmodels[dirInfos.Length+i].path.Substring(filesmodels[dirInfos.Length+i].path.LastIndexOf("."))))
                    //{
                      //  filesmodels[dirInfos.Length+i].Icon = @"~/Images/image-icon.png";
                    //}
                    //Console.WriteLine(filesmodels[dirInfos.Length+i].name);
                }
                //Console.WriteLine("-----------");
                
                return View(filesmodels);
            }
            //UnauthorizedAccessException
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("you shall not pass here");
            }
            return displayFilesView(path.Substring(0,path.LastIndexOf(@"/")));
        }

        [HttpGet("[action]")]
        public FileContentResult imageGenerate(string filePath)
        {
            Bitmap bitmap;
            System.Drawing.Icon iconForFile = System.Drawing.Icon.ExtractAssociatedIcon(filePath);
            bitmap = iconForFile.ToBitmap();
            ImageConverter converter = new ImageConverter();
            return new FileContentResult((byte[]) converter.ConvertTo(bitmap, typeof(byte[])), "image/png");
        }

        [HttpGet("[action]")]
        public IActionResult DisplayImageView(string path)
        {
            ViewBag.Imagepath = path;
            return View();
        }

        public bool isFolder(string path)
        {
            if (path.StartsWith("~/Images"))
            {
                return false;
            }
            return true;
        }
           
        
    }
}