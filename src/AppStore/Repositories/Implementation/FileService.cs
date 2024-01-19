using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppStore.Repositories.Abstract;

namespace AppStore.Repositories.Implementation
{ 
   
    public class FileService : IFileService
    {

     private readonly IWebHostEnvironment environment;
     
     public FileService(IWebHostEnvironment _environment)
     {
       environment = _environment;
     }

        public bool DeleteImage(string imageFileName)
        {
           try
           {
            var wwwPAth = environment.WebRootPath;
            var path = Path.Combine(wwwPAth, "Upload\\", imageFileName);
             if(System.IO.File.Exists(path))
             {
                 System.IO.File.Delete(path);
                 return true;
             }
             return false;

           }
           catch (Exception)
           {
            
           return false;
           }
        }

        public Tuple<int, string> SaveImage(IFormFile imageFile)
        {
          try
          {
            var wwwPAth = environment.WebRootPath;
            var path = Path.Combine(wwwPAth, "Upload");

            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            var ext = Path.GetExtension(imageFile.FileName);

            var allowedExtention = new String[] {".jpg", ".png", ".jpeg"};
            if(!allowedExtention.Contains(ext))
            {
                var message = $"Solo estan permitidas las extensiones {allowedExtention}";
                return new Tuple<int, string>(0, message);
            }

            var uniqueString = Guid.NewGuid().ToString();
            var newFileName = uniqueString+ext;

            var fileWithPath = Path.Combine(path, newFileName);

            var stream = new FileStream(fileWithPath, FileMode.Create);

            imageFile.CopyTo(stream);
            stream.Close();

            return new Tuple<int, string>(1, newFileName);


          }
          catch (Exception)
          {
            
           return new Tuple<int, string>(0, "Errores guardando la imagen");
          }
        }
    }
}