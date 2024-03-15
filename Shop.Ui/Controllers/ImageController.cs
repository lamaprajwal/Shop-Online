using Microsoft.AspNetCore.Mvc;
using Shop.Ui.FileManager;

namespace Shop.Ui.Controllers
{
    public class ImageController : Controller
    {
        private IFileManager _fileM;

        public ImageController(IFileManager fileManager)
        {
            _fileM=fileManager;
            
        }


        [HttpGet("/Image/{image}")]
        public IActionResult Image(string image)
        {
            var mime = image.Substring(image.LastIndexOf('.') + 1);
            return new FileStreamResult(_fileM.ImageStream(image), $"image/{mime}");
        }
    }
}
