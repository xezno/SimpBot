using Disco;
using System;

namespace SimpBot
{
    public static class Program
    {
        public static DiscoApplication discoApplication;
        public static void Main(string[] args)
        {
            discoApplication = new DiscoApplication();
            discoApplication.Run().Wait();
            Console.WriteLine("Bot quit");
        }
    }
}
