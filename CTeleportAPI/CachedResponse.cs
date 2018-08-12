using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Nancy;

namespace CTeleportAPI
{
    public class CachedResponse : Response
    {
        private readonly Response response;

        public CachedResponse(Response response)
        {
            this.response = response;

            this.ContentType = response.ContentType;
            this.Headers = response.Headers;
            this.StatusCode = response.StatusCode;
            this.Contents = this.GetContents();
        }

        public override Task PreExecute(NancyContext context)
        {
            return this.response.PreExecute(context);
        }

        private Action<Stream> GetContents()
        {
            return stream =>
            {
                using (var memoryStream = new MemoryStream())
                {
                    this.response.Contents.Invoke(memoryStream);

                    var contents =
                        Encoding.ASCII.GetString(memoryStream.GetBuffer());

                    var writer =
                        new StreamWriter(stream) { AutoFlush = true };

                    writer.Write(contents);
                }
            };
        }
    }
}