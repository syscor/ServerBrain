namespace BrilliantIdea.WebServer
{
    static class Settings
    {
        /// <summary>
        /// "" for complete access, otherwise @"[drive]:\[folder]\[folder]\[...]\"
        /// Watch the '\' at the end of the path
        /// </summary>
        public const string RootPath = "\\SD\\";

        /// <summary>
        /// Maximum byte size for a HTTP request sent to the server
        /// </summary>
        public const int MaxRequestsize = 1024;

        /// <summary>
        /// Buffersize for response file sending 
        /// </summary>
        public const int FileBuffersize = 1024;
    }
}
