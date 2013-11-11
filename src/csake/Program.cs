using System;
using CSScriptLibrary;

namespace CSake
{
    class Program
    {
        static void Main(string[] args)
        {
            //var file = args[0];
            
            
            var asm=CSScript.LoadCode(@" 
using System.IO;
namespace Hihi{
public class Bla {
public static void Clean()
{
       Directory.CreateDirectory(""haha"");
}}}
");
            var m = new AsmHelper(asm);
            m.Invoke("Hihi.Bla.Clean");
            foreach (var tp in asm.GetTypes())
            {
                Console.WriteLine(tp.FullName);
            }
            Console.ReadLine();

        }
    }
}
