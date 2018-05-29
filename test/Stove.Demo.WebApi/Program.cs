using System;

// using HibernatingRhinos.Profiler.Appender.EntityFramework;

using Microsoft.Owin.Hosting;

namespace Stove.Demo.WebApi
{
    public class Program
    {
        public static void Main()
        {
            const string localhost = "http://localhost:9000";

            //EntityFrameworkProfiler.Initialize();

            using (WebApp.Start<Startup>(localhost))
            {
                Console.WriteLine("Started...");
                Console.Read();
            }
        }
    }
}
