using classADO_6;
using SampleWebApi_6.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;
using System.Net.Http.Headers;
using Image = SampleWebApi_6.Models.Image;

namespace SampleWebApi_6.Controllers
{
    public class imageUploadController : ApiController
    {
        webapiEntities db = new webapiEntities();
        List<string> validExtensions = new List<string>() {".jpg", ".jpeg", ".png" };
        string upFileName, ext, filepath;

        [Route("api/imagetUpload/uploadImage")]
        [HttpPost]
        public string uploadImage()            //public async Task<string> uploadImage()
        {
            if(!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            try
            {
                HttpContext context = HttpContext.Current;

                string root = context.Server.MapPath("~/App_Data");

                //MultipartFormDataStreamProvider provider = new MultipartFormDataStreamProvider(root);

                for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[i];

                    upFileName = Path.GetFileName(file.FileName).Trim('"');

                    ext = Path.GetExtension(upFileName);

                    System.Drawing.Image myImage = System.Drawing.Image.FromStream(file.InputStream);

                    if ((file.ContentLength < 900000)
                        && (validExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase))
                        && (myImage.Height <= 1000 && myImage.Width <= 1000))
                    {
                        continue;
                    }
                    else
                    {
                        return $"{upFileName} : Image size is too large or Image type is not supported or Image dimensions too large...!.";
                    }
                }

                for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                {
                    HttpPostedFile file = HttpContext.Current.Request.Files[i];

                    upFileName = Path.GetFileName(file.FileName).Trim('"');

                    ext = Path.GetExtension(upFileName);

                    filepath = Path.Combine(root, upFileName);

                    HttpContext.Current.Request.Files[i].SaveAs(filepath);

                    InsertImageData(filepath);
                }

                //await Request.Content.ReadAsMultipartAsync(provider);

                //foreach (var fi in provider.FileData)
                //{
                //    string name = fi.Headers.ContentDisposition.FileName.Trim('"');

                //    string localFileName = fi.LocalFileName;

                //    string filepath = Path.Combine(root, name);

                //    File.Move(localFileName, filepath);

                //    InsertImageData(filepath);
                //}

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "Image uploaded.";
        }

        public void InsertImageData(string path)
        {
            test obj = new test();
            obj.connection();
            obj.InsertImage(path);
            db.SaveChanges();
        }
    }
}
