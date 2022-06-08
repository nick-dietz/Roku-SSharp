using System;
using Roku_SSharp;

namespace Roku_SSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Roku roku  = new Roku();
            roku.GetApps("192.168.0.103");
            Console.ReadLine();
        }
    }
}
