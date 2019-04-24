using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageGallery.Configuration;
using ImageGallery.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCaching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ImageGallery {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddResponseCaching();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            Enum.TryParse(Configuration["BackingStore"], out BackingStore backingStore);
            // TODO: Change Exception text when another store is added
            switch (backingStore) {
                case BackingStore.File:
                    services.AddSingleton<IAlbumService, FileBackedAlbumService>();
                    break;
                default:
                    throw new ArgumentException("BackingStore must be either file or TODO");
            }
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            var cssPath = Path.Combine(env.WebRootPath, "css", "styles.css");
            var cssLines = File.ReadAllLines(cssPath);
            for (int i = 0; i < cssLines.Length; i++) {
                if (cssLines[i].StartsWith("    background-color: ", StringComparison.InvariantCulture)) {
                    cssLines[i] = "    background-color: " + Configuration["MenuColor"] + ";";
                }
            }

            File.WriteAllLines(cssPath, cssLines);

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            var OneYearTimeSpan = TimeSpan.FromSeconds(31536000);
            app.UseResponseCaching();
            app.Use(async (context, next) => {
                context.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue {
                        Public = true,
                        MaxAge = OneYearTimeSpan
                    };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };

                await next();
            });

            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
