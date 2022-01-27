namespace NewXXSY.Server
{
    internal class SocketsHttpHandler
    {
        public SocketsHttpHandler()
        {
        }

        public bool UseProxy { get; set; }
        public object Proxy { get; set; }
        public bool UseCookies { get; set; }
        public int AutomaticDecompression { get; set; }
    }
}