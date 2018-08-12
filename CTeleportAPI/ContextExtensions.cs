using Nancy;

namespace CTeleportAPI
{
    public static class ContextExtensions
    {
        public const string OUTPUT_CACHE_TIME_KEY = "OUTPUT_CACHE_TIME";

        public static void EnableOutputCache(this NancyContext context, int seconds)
        {
            context.Items[OUTPUT_CACHE_TIME_KEY] = seconds;
        }

        public static void DisableOutputCache(this NancyContext context)
        {
            context.Items.Remove(OUTPUT_CACHE_TIME_KEY);
        }
    }
}
