using System;

namespace FCS.Common
{
    public static class GlobalConstants
    {
        private static DateTime? reindexStart = null;

        public static DateTime ReindexStart
        {
            get
            {
                if (reindexStart == null)
                {
                    reindexStart = DateTime.Now;
                }

                return (DateTime)reindexStart;
            }
        }

        public const int OneWeekInSeconds = 604800;

        public const string JsonFileExtension = ".json";

        public const string ExceptionMessageShouldNotBeNull = " should not be null.";

        public const string FileCacheDirectoryName = "FileCache_";


    }
}
