using System;
using Roku_SSharp;

namespace Roku_SSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Roku roku  = new Roku("192.168.0.103");
            roku.GetApps();
            Console.ReadLine();
        }
    }
}
