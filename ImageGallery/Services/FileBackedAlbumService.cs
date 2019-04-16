using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageGallery.Models;
using ImageMagick;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ImageGallery.Services
{
    public class FileBackedAlbumService : IAlbumService
    {
        readonly string albumsPath;

        public FileBackedAlbumService()
        {
            var currentFolder = Directory.GetCurrentDirectory();
            albumsPath = Path.Combine(currentFolder, "albums");
            Directory.CreateDirectory(albumsPath);
        }

        public async Task<Album> GetAlbum(string id)
        {
            var albumPath = Path.Combine(albumsPath, id);
            var content = await File.ReadAllTextAsync(albumPath + ".json");
            var album = JsonConvert.DeserializeObject<Album>(content);
            return album;
        }

        public async Task AddImagesToAlbumWithId(string albumId, List<IFormFile> files)
        {

            var album = await GetAlbum(albumId);
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    await AddImageToAlbumWithId(album, formFile);
                }
            }
            await SaveAlbum(album);
            // TODO: create pre render
            // TODO: create thumbnail in thumbs folder in album
            // TODO: save

        }

        public async Task<byte[]> GetThumbnail(string albumId, string imageId)
        {
            // TODO: Create utility for all these paths
            var filePath = Path.Combine(albumsPath, albumId, "thumbs", imageId);
            return await File.ReadAllBytesAsync(filePath);
        }

        // TODO: Order by time taken in exif, if no exist, write todays date
        // TODO: image metadata during save
        // TODO: images in album
        // TODO: Show full version with metadata
        // TODO: Able to delete  when viewing full version

        public async Task<byte[]> GetPreRenders(string albumId, string imageId)
        {
            var filePath = Path.Combine(albumsPath, albumId, "pre-renders", imageId);
            return await File.ReadAllBytesAsync(filePath);
        }

        async Task AddImageToAlbumWithId(Album album, IFormFile file)
        {
            var kindParts = file.ContentType.Split('/');
            if (kindParts.Length != 2 || kindParts[0] != "image" || kindParts[1].Length < 3 || kindParts[1].Length > 4)
            {
                return;
            }

            // TODO: If I need byte[]
            //using (var memoryStream = new MemoryStream())
            //{
            //    await model.AvatarImage.CopyToAsync(memoryStream);
            //    user.AvatarImage = memoryStream.ToArray();
            //}

            var albumPath = Path.Combine(albumsPath, album.Id);
            Directory.CreateDirectory(albumPath);

            var imageId = Guid.NewGuid().ToString() + "." + kindParts[1];
            var filePath = Path.Combine(albumPath, imageId);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var optimizer = new ImageOptimizer();
            optimizer.LosslessCompress(filePath);

            var thumbsPath = Path.Combine(albumPath, "thumbs");
            Directory.CreateDirectory(thumbsPath);
            const int MaxThubSize = 400;
            using (var image = new MagickImage(filePath))
            {
                if (image.Width > image.Height)
                {
                    if (image.Width > MaxThubSize)
                    {
                        image.Resize(MaxThubSize, 0);
                    }
                } 
                else if (image.Height > MaxThubSize)
                {
                    image.Resize(0, MaxThubSize);
                }

                var thumbPath = Path.Combine(thumbsPath, Path.GetFileName(filePath));
                image.Write(thumbPath);
                optimizer.OptimalCompression = true;
                optimizer.Compress(thumbPath);

                var preRendersPath = Path.Combine(albumPath, "pre-renders");
                Directory.CreateDirectory(preRendersPath);
                var preRenderPath = Path.Combine(preRendersPath, Path.GetFileName(filePath));
                // TODO: Find mean color from the image
                var preRender = new MagickImage(MagickColor.FromRgb(120, 60, 30), image.Width, image.Height);
                preRender.Write(preRenderPath);
                optimizer.Compress(preRenderPath);
            }

            album.Images.Add(new Image(imageId));
        }

        async Task SaveAlbum(Album album)
        {
            var content = JsonConvert.SerializeObject(album);
            await File.WriteAllTextAsync(Path.Combine(albumsPath, album.Id) + ".json", content);
        }


    }
}
