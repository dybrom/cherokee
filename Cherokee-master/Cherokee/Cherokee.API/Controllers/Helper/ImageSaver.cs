using Cherokee.DAL.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Cherokee.API.Controllers.Helper;
using System.Drawing;

namespace Cherokee.API.Controllers.Helper
{
    public static class ImageSaver
    {
        public static string ConvertAndSave(this BaseClass<int> entity)
        {
            var type = entity.GetType().Name;
            string path = HttpContext.Current.Server.MapPath("~/App_Data/" + type + "s");


            byte[] ImageBytes = null;
            string FileName = "";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            switch (type)
            {
                case "Employee":
                    var emp = entity as Employee;

                    //create image from first letters if image is not uploaded
                    if (String.IsNullOrEmpty(emp.Image))
                    {

                        var monogram = emp.FirstName + " " + emp.LastName;
                        var sTextImage = "";
                        foreach (var part in monogram.Split(' '))
                        {
                            sTextImage += part.Substring(0, 1);
                            sTextImage.ToUpper();
                        }

                        //TextToImage image = new TextToImage();
                     //   var mx = image.CreateBitmapImage(sTextImage);
                        FileName = emp.Id.ToString() + "-" + emp.FirstName + "-" + emp.LastName + ".jpg";
                        //mx.Save(path + "\\" + FileName);

                        string location = path + "\\" + FileName;
                        return location;
                    }

                    ImageBytes = Convert.FromBase64String(emp.Image);

                    //bytes to image object

                    try
                    {

                        Image empImg = (Bitmap)((new ImageConverter()).ConvertFrom(ImageBytes));

                        //crop to 640px
                        //CropImage empImgCrop = new CropImage();
                       // empImg = empImgCrop.MakeSquarePhoto((Bitmap)empImg, 640);

                        //high quality resize to 640px
                       // ImageResize empImgResize = new ImageResize();
                       // empImg = empImgResize.ResizeImage(empImg, 640, 640);

                        //convert image back to byteArray
                        ImageConverter empImgConverter = new ImageConverter();
                        ImageBytes = (byte[])empImgConverter.ConvertTo(empImg, typeof(byte[]));
                    }
                    catch
                    {

                    }



                    FileName = emp.Id.ToString() + "-" + emp.FirstName + "-" + emp.LastName + ".jpg";
                    break;

                case "Customer":
                    var cust = entity as Customer;

                    //if (String.IsNullOrEmpty(cust.Image)) return "";
                    if (String.IsNullOrEmpty(cust.Image))
                    {

                        var monogram = cust.Name;
                        var sTextImage = "";
                        foreach (var part in monogram.Split(' '))
                        {
                            sTextImage += part.Substring(0, 1);
                            sTextImage.ToUpper();
                        }

                        //TextToImage image = new TextToImage();
                      //  var mx = image.CreateBitmapImage(sTextImage);
                        FileName = cust.Id.ToString() + "-" + cust.Name + ".jpg";
                        //mx.Save(path + "\\" + FileName);

                        string location = path + "\\" + FileName;
                        return location;
                    }

                    ImageBytes = Convert.FromBase64String(cust.Image);

                    try
                    {
                        //bytes to image object
                        Image custImg = (Bitmap)((new ImageConverter()).ConvertFrom(ImageBytes));

                        //crop to 640px
                        //CropImage custImgCrop = new CropImage();
                        //custImg = custImgCrop.MakeSquarePhoto((Bitmap)custImg, 720);

                        //high quality resize to 16:9
                        //ImageResize custImgResize = new ImageResize();
                       // custImg = custImgResize.ResizeImage(custImg, 640, 360);

                        //convert image back to byteArray
                        ImageConverter converter = new ImageConverter();
                        ImageBytes = (byte[])converter.ConvertTo(custImg, typeof(byte[]));
                    }
                    catch
                    {

                    }



                    FileName = cust.Id + "-" + cust.Name + ".jpg";
                    break;

                default: return "";
            }
            string FullPath = Path.Combine(path, FileName);
            File.WriteAllBytes(FullPath, ImageBytes);

            return FullPath;
        }

        public static string ConvertToBase64(this BaseClass<int> entity)
        {
            var type = entity.GetType().Name.ToString();
            if (type != "Employee" && type != "Customer")
                type = type.Substring(0, type.IndexOf("_"));

            string base64Photo = "";

            if (type == "Employee")
            {
                var emp = entity as Employee;
                base64Photo = (File.Exists(emp.Image)) ? Convert.ToBase64String(File.ReadAllBytes(emp.Image)) : "";

            }
            else if (type == "Customer")
            {
                var cust = entity as Customer;
                base64Photo = (File.Exists(cust.Image)) ? Convert.ToBase64String(File.ReadAllBytes(cust.Image)) : "";

            }

            return base64Photo;
        }

    }
}