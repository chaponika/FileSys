using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FileSys.Models;

namespace FileSys.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            DriveInfo di = new DriveInfo(@"C:\");
            DirectoryInfo dirInfo = di.RootDirectory;
            FileInfo[] files = dirInfo.GetFiles("*.*");
            DirectoryInfo[] dirInfos = dirInfo.GetDirectories("*.*");
            FilesModel[] filesModel = new FilesModel[dirInfos.Length];
            for (int i = 0; i < filesModel.Length; i++)
            {
               filesModel[i] = new FilesModel(dirInfos, i);
            }
            
            return View(filesModel);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}