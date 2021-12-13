using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using FileSys.Models;
using Microsoft.AspNetCore.Mvc;


namespace FileSys.Controllers
{
    public class PathController : Controller
    {
        private static readonly string[] imageFileTypesArray = new string[]{".bmp",".jpg",".jpeg",".tif", ".tiff",".png"};
        private static readonly HashSet<string> imageTypes = new HashSet<string>(imageFileTypesArray);

        [HttpGet("{*path}")]
        public IActionResult displayFilesView(string path)
        {
            if (path.LastIndexOf(".") != -1 && imageTypes.Contains(path.Substring(path.LastIndexOf("."))))
            {
                return RedirectToAction("DisplayImageView",new{path = path});
            }

            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                FileInfo[] files = dirInfo.GetFiles("*.*");
                DirectoryInfo[] dirInfos = dirInfo.GetDirectories("*.*");

                FilesModel[] filesmodels = new FilesModel[dirInfos.Length + files.Length];
                for (int i = 0; i < dirInfos.Length; i++)
                {
                    /*FilesModel temp = new FilesModel();
                    temp.isFolder = true;
                    temp.name = dirInfos[i].Name;
                    temp.path = dirInfos[i].FullName;
                    filesmodels[i] = temp;*/
                    filesmodels[i] = new FilesModel(dirInfos, i);
                    filesmodels[i].Icon =  @"wwwroot\Images\folder-icon.png";
                }

                string[] filePaths = Directory.GetFiles(path);
                for (int i=0; i <files.Length; i++)
                {
                    var file = files[i];
                    filesmodels[dirInfos.Length + i] = new FilesModel();
                    System.Drawing.Icon iconForFile = System.Drawing.Icon.ExtractAssociatedIcon(file.FullName);

                    filesmodels[dirInfos.Length+i].name = file.Name;
                    filesmodels[dirInfos.Length+i].path = file.FullName;
                    filesmodels[dirInfos.Length+i].isFolder = false;
                    Image image = iconForFile.ToBitmap();
                    
                    if (filesmodels[dirInfos.Length+i].path.Substring(filesmodels[dirInfos.Length+i].path.LastIndexOf(".")).Equals(".txt"))
                    {
                        filesmodels[dirInfos.Length+i].Icon = @"wwwroot\Images\txt-file-icon.png";
                    }else if (imageTypes.Contains(filesmodels[dirInfos.Length+i].path.Substring(filesmodels[dirInfos.Length+i].path.LastIndexOf("."))))
                    {
                        filesmodels[dirInfos.Length+i].Icon = @"wwwroot\Images\txt-file-icon.png";
                    }
                    else
                    {
                        filesmodels[dirInfos.Length+i].Icon = "favicon.ico";
                    }

                    //Console.WriteLine(filesmodels[dirInfos.Length+i].name);
                }
                Console.WriteLine("-----------");
                return View(filesmodels);

            }
            //UnauthorizedAccessException
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("you shall not pass here");
            }
            return displayFilesView(path.Substring(0,path.LastIndexOf(@"/")));
        }

        [HttpGet]
        public FileContentResult imageGenerate(string filePath)
        {
            System.Drawing.Icon iconForFile = System.Drawing.Icon.ExtractAssociatedIcon(filePath);
            Bitmap b = iconForFile.ToBitmap();
            ImageConverter converter = new ImageConverter();
            return new FileContentResult((byte[])converter.ConvertTo(b, typeof(byte[])), "image/png");
        }

        [HttpGet]
        public IActionResult DisplayImageView(string path)
        {
            ViewBag.Imagepath = path;
            return View();
        }
    }
}