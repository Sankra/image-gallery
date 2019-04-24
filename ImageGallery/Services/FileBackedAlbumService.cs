﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageGallery.Models;
using ImageMagick;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ImageGallery.Services {
    public class FileBackedAlbumService : IAlbumService {
        readonly string albumsPath;
        readonly string preRendersPath;

        public FileBackedAlbumService() {
            var currentFolder = Directory.GetCurrentDirectory();
            albumsPath = Path.Combine(currentFolder, "albums");
            Directory.CreateDirectory(albumsPath);
            preRendersPath = Path.Combine(albumsPath, "pre-renders");
            Directory.CreateDirectory(preRendersPath);
        }

        public async Task<Album> GetAlbum(string id) {
            // TODO: cache and update if images added while running
            var albumPath = Path.Combine(albumsPath, id);
            var content = await File.ReadAllTextAsync(albumPath + ".json");
            var album = JsonConvert.DeserializeObject<Album>(content);
            return album;
        }

        public async Task<Album> GetAlbumMetadata(string id) {
            // TODO: Don't add images here
            var albumPath = Path.Combine(albumsPath, id);
            var content = await File.ReadAllTextAsync(albumPath + ".json");
            var album = JsonConvert.DeserializeObject<Album>(content);
            return album;
        }

        public async Task AddImagesToAlbumWithId(string albumId, List<IFormFile> files) {
            var album = await GetAlbum(albumId);
            foreach (var formFile in files) {
                if (formFile.Length > 0) {
                    await AddImageToAlbumWithId(album, formFile);
                }
            }

            album.OrderByDateTaken();
            await SaveAlbum(album);
        }

        public async Task<byte[]> GetThumbnail(string albumId, string imageId) {
            // TODO: Create utility for all these paths
            var filePath = Path.Combine(albumsPath, albumId, "thumbs", imageId);
            return await File.ReadAllBytesAsync(filePath);
        }

        // TODO: image metadata during save
        // TODO: images in album
        // TODO: Show full version with metadata
        // TODO: Able to delete  when viewing full version

        public async Task<byte[]> GetPreRenders(ushort width, ushort height) {
            var filePath = Path.Combine(preRendersPath, width + "x" + height + ".png");
            return await File.ReadAllBytesAsync(filePath);
        }

        public async Task<byte[]> GetImage(string albumId, string imageId) {
            var filePath = Path.Combine(albumsPath, albumId, imageId);
            return await File.ReadAllBytesAsync(filePath);
        }

        public async Task<Image> GetImageWithMetadata(string albumId, string imageId) {
            // TODO: Horrible, just horrible
            var album = await GetAlbum(albumId);
            return album.Images.Single(p => p.Id == imageId);
        }

        public async Task Delete(string albumId, string imageId) {
            // TODO: Add deleted image id to trash album
            // TODO: Create a trash album to view deleted images
            // TODO: Delete 4 reelz from trash
            var album = await GetAlbum(albumId);
            album.Delete(new Image(imageId));
            await SaveAlbum(album);
        }

        public async Task<Album[]> GetAlbumPreviews() {
            var albumPreviews = await GetAlbumsWithoutImages();
            foreach (var album in albumPreviews) {
                album.Trim(5);
            }

            return albumPreviews;
        }

        public async Task<Album[]> GetAlbumsWithoutImages() {
            var albumFiles = Directory.GetFiles(albumsPath, "*.json");
            var albums = new Album[albumFiles.Length];
            for (int i = 0; i < albumFiles.Length; i++) {
                albums[i] = await GetAlbum(Path.GetFileNameWithoutExtension(albumFiles[i]));
            }

            return albums;
        }

        public async Task<string> AddAlbum(string name) {
            var albumId = Guid.NewGuid().ToString();

            var newAlbum = new Album(albumId, name);
            await SaveAlbum(newAlbum);

            var albumPath = Path.Combine(albumsPath, albumId);
            Directory.CreateDirectory(albumPath);

            return albumId;
        }

        async Task AddImageToAlbumWithId(Album album, IFormFile file) {
            var kindParts = file.ContentType.Split('/');
            if (kindParts.Length != 2 || kindParts[0] != "image" || kindParts[1].Length < 3 || kindParts[1].Length > 4) {
                return;
            }

            // If I need byte[]
            //using (var memoryStream = new MemoryStream())
            //{
            //    await model.AvatarImage.CopyToAsync(memoryStream);
            //    user.AvatarImage = memoryStream.ToArray();
            //}

            var albumPath = Path.Combine(albumsPath, album.Id);
            Directory.CreateDirectory(albumPath);

            var imageId = Guid.NewGuid().ToString() + "." + kindParts[1];
            var filePath = Path.Combine(albumPath, imageId);
            using (var stream = new FileStream(filePath, FileMode.Create)) {
                await file.CopyToAsync(stream);
            }

            var optimizer = new ImageOptimizer();
            optimizer.LosslessCompress(filePath);

            var thumbsPath = Path.Combine(albumPath, "thumbs");
            Directory.CreateDirectory(thumbsPath);
            using (var image = new MagickImage(filePath)) {
                const int MaxThubSize = 400;
                if (image.Width > image.Height) {
                    if (image.Width > MaxThubSize) {
                        image.Resize(MaxThubSize, 0);
                    }
                } else if (image.Height > MaxThubSize) {
                    image.Resize(0, MaxThubSize);
                }

                var thumbPath = Path.Combine(thumbsPath, Path.GetFileName(filePath));
                image.Write(thumbPath);

                optimizer.OptimalCompression = true;
                optimizer.Compress(thumbPath);

                // TODO: path to  utility
                var preRenderPath = Path.Combine(preRendersPath, image.Width + "x" + image.Height + ".png");
                if (!File.Exists(preRenderPath)) {
                    // TODO: Find mean color from the image
                    var preRender = new MagickImage(MagickColor.FromRgba(0, 0, 0, 1), image.Width, image.Height);
                    preRender.Write(preRenderPath);
                    optimizer.Compress(preRenderPath);
                }

                var dateTaken = DateTime.UtcNow;
                var exif = image.GetExifProfile();
                var exifDateTime = exif?.GetValue(ExifTag.DateTime);
                if (exifDateTime != null) {
                    if (DateTime.TryParseExact((string)exifDateTime.Value, "yyyy:MM:dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime)) {
                        dateTaken = dateTime;
                    }
                }

                album.AddImage(new Image(imageId, dateTaken, (ushort)image.Width, (ushort)image.Height));
            }
        }

        async Task SaveAlbum(Album album) {
            var content = JsonConvert.SerializeObject(album);
            await File.WriteAllTextAsync(Path.Combine(albumsPath, album.Id) + ".json", content);
        }
    }
}
