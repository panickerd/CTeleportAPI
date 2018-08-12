using CTeleportAPI.Repository;
using CTeleportAPI.Service;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;

namespace CTeleportAPI
{
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        private readonly Dictionary<string, Tuple<DateTime, Response, int, string>> cachedResponses = new Dictionary<string, Tuple<DateTime, Response, int, string>>();

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            pipelines.BeforeRequest += CheckCache;

            pipelines.AfterRequest += SetCache;

            pipelines.OnError += (ctx, ex) =>
            {
                return "No records found";
            };
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
            container.Register<ICalculateDistance, AirportDistance>().AsSingleton();
        }

        public Response CheckCache(NancyContext context)
        {
            Tuple<DateTime, Response, int, string> cacheEntry;

            if (this.cachedResponses.TryGetValue(string.Concat(context.Request.Query["source"], context.Request.Query["destination"]), out cacheEntry))
            {
                if (cacheEntry.Item1.AddSeconds(cacheEntry.Item3) > DateTime.Now)
                {
                    return cacheEntry.Item2;
                }
            }

            return null;
        }

        public void SetCache(NancyContext context)
        {
            if (context.Response.StatusCode != HttpStatusCode.OK)
            {
                return;
            }

            if (!context.Items.TryGetValue(ContextExtensions.OUTPUT_CACHE_TIME_KEY, out object cacheSecondsObject))
            {
                return;
            }

            if (!int.TryParse(cacheSecondsObject.ToString(), out int cacheSeconds))
            {
                return;
            }

            var cachedResponse = new CachedResponse(context.Response);

            this.cachedResponses[string.Concat(context.Request.Query["source"], context.Request.Query["destination"])]
                = new Tuple<DateTime, Response, int, string>(DateTime.Now, cachedResponse, cacheSeconds, string.Concat(context.Request.Query["source"], context.Request.Query["destination"]));
        }
    }
}
