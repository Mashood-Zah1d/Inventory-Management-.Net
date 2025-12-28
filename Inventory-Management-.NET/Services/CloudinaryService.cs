using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Inventory_Management_.NET.Utils;
using System.IO;

namespace Inventory_Management_.NET.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task<ResponseMessage<string>> UploadImageAsync(IFormFile image)
        {
            string ImageUrl = null;

            if (image == null || image.Length == 0)
            {
                return new ResponseMessage<string>
                {
                    Success = true,
                    Message = "No image uploaded",
                    Data = null
                };
            }


            using var Stream = image.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(image.FileName, Stream),
                Folder = "product"
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                return new ResponseMessage<string>
                {
                    Success = false,
                    Message = "Can't Upload The Image On Cloudinary"
                };
            }

            ImageUrl = uploadResult.SecureUrl.ToString();

            return new ResponseMessage<string>
            {
                Success = true,
                Message = "Image Uploaded Successfully",
                Data = ImageUrl
            };
        }
    }
}
