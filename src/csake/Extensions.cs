using System;

namespace CSake
{
    public static class Extensions
    {
        public static void ToConsole(this string data, params object[] args)
        {
            Console.WriteLine(data,args);
        }
    }
}