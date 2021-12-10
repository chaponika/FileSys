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
        private static readonly string[] imageFileTypesArray = new string[]{".bmp",".jpg",".jpeg",".tif", ".tiff"};
        private static readonly HashSet<string> imageTypes = new HashSet<string>(imageFileTypesArray);

        [HttpGet("{*path}")]
        public IActionResult displayFilesView(string path)
        {
            if (imageTypes.Contains(path.Substring(path.LastIndexOf("."))))
            {
                return displayImageView(path);
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
                    filesmodels[i].Icon =  (Image)System.Drawing.Icon.ExtractAssociatedIcon(@"Properties\Images\folder-icon.png").ToBitmap();
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
                    filesmodels[dirInfos.Length+i].Icon = (Image)iconForFile.ToBitmap();
                    Image image = iconForFile.ToBitmap();
                    
                    if (filesmodels[dirInfos.Length+i].path.Substring(filesmodels[dirInfos.Length+i].path.LastIndexOf(".")).Equals(".txt"))
                    {
                        filesmodels[dirInfos.Length+i].Icon = (Image)System.Drawing.Icon.ExtractAssociatedIcon(@"Properties\Images\txt-file-icon.png").ToBitmap();
                    }

                    if (imageTypes.Contains(filesmodels[dirInfos.Length+i].path.Substring(filesmodels[dirInfos.Length+i].path.LastIndexOf("."))))
                    {
                        filesmodels[dirInfos.Length+i].Icon = (Image)System.Drawing.Icon.ExtractAssociatedIcon(@"Properties\Images\txt-file-icon.png").ToBitmap();
                    }
                    //Console.WriteLine(filesmodels[dirInfos.Length+i].name);
                }
                
                Console.WriteLine("-----------");
                return View(filesmodels);

            }
            //UnauthorizedAccessException
            catch (Exception e)
            {
                Console.WriteLine("you shall not pass here");
            }
            
            return displayFilesView(path.Substring(0,path.LastIndexOf(@"/")));
        }

        public IActionResult displayImageView(string path)
        {
            return View(path);
        }
    }
}