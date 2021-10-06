using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Net;
using System;
using Microsoft.Extensions.Options;
using RainstormTech.Storm.ImageProxy.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http.Headers;
using RainstormTech.Storm.ImageProxy.Extensions;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace RainstormTech.Storm.ImageProxy
{
    public class ImageProxyFunction
    {
        private readonly IImageResizerService imageResizerService;
        private readonly IOptions<ClientCacheOptions> clientCacheOptions;
        private readonly IConfiguration config;

        public ImageProxyFunction(IImageResizerService imageProxyService, 
            IOptions<ClientCacheOptions> clientCacheOptions,
            IConfiguration configuration)
        {
            this.imageResizerService = imageProxyService;
            this.clientCacheOptions = clientCacheOptions;
            config = configuration;
        }


        /*
         * Main entry point...takes a wildcard.
         */
        [FunctionName("ResizeImage")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ResizeImage/{*restOfPath}")] HttpRequest req, string restOfPath)
        {
            // check to see if we have a cached version and just leave if we do
            if (req.HttpContext.Request.GetTypedHeaders().IfModifiedSince.HasValue)
            {
                return new StatusCodeResult((int)HttpStatusCode.NotModified);
            }

            // set cache 
            this.SetCacheHeaders(req.HttpContext.Response.GetTypedHeaders());

            try
            {
                // get the url
                var url = restOfPath.Replace(config.GetValue<string>("AzureContainer"), "");

                // we need at least the url
                if (string.IsNullOrEmpty(url))
                    return new BadRequestObjectResult("URL is required");

                // figure out the needed variables
                // var url = req.Query["url"].ToString();
                var size = req.Query.ContainsKey("size") ? req.Query["size"].ToString() : "";
                var width = req.Query.ContainsKey("w") ? req.Query["w"].ToString().ToInt() : 0;
                var height = req.Query.ContainsKey("h") ? req.Query["h"].ToString().ToInt() : 0;
                var output = req.Query.ContainsKey("output") ? req.Query["output"].ToString().Replace(".", "") : url.ToSuffix();
                var mode = req.Query.ContainsKey("mode") ? req.Query["mode"].ToString() : "";
                var validOutputs = new List<string>() { "jpg", "gif", "png", "webp" };

                // validate the output
                if (!validOutputs.Contains(output))
                    output = url.ToSuffix();

                // figure out the actual size
                if (string.IsNullOrEmpty(size))
                    size = $"{width}x{height}";

                // try to resize the image
                var imageStream = await this.imageResizerService.ResizeAsync(url, size, output, mode);

                if (imageStream == null)
                    return new NotFoundResult();

                // choose the correct mime type
                var mimeType = output switch
                {
                    "jpg" => "image/jpeg",
                    "gif" => "image/gif",
                    "png" => "image/png",
                    "webp" => "image/webp",
                    _ => "image/jpeg"
                };

                // return the stream
                return new FileStreamResult(imageStream, mimeType);
            }
            catch(Exception ex)
            {
                return new BadRequestResult();
            }            
        }

        private void SetCacheHeaders(ResponseHeaders responseHeaders)
        {
            responseHeaders.CacheControl = new CacheControlHeaderValue { Public = true };
            responseHeaders.LastModified = new DateTimeOffset(new DateTime(1900, 1, 1));
            responseHeaders.Expires = new DateTimeOffset((DateTime.Now + this.clientCacheOptions.Value.MaxAge).ToUniversalTime());
        }
    }
}
