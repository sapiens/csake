using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CSake.Internals
{
    public class ScriptWrapper
    {
        public const string ClassName = "CSakeWrapper";
        private List<string> _refs=new List<string>();

        public IEnumerable<string> ReferencedAssemblies
        {
            get { return _refs; }
        }

        public string Wrap(IReadLine data)
        {
            data.MustNotBeNull();
            var sb = new StringBuilder(4096);
            var afterHeader = false;
            while (!data.HasFinished)
            {
                var line = data.ReadLine().Trim();
                if (line.IsNullOrEmpty())
                {
                    sb.AppendLine();
                    continue;
                }
               
                if (IsReference(line))
                {
                    ExtractReference(line);
                    continue;
                }

                if (IsComment(line))
                {
                    continue;
                }

                if (!afterHeader)
                {
                    if (!IsPartOfHeader(line))
                    {
                        afterHeader = true;
                        sb.AppendLine("public class " + ClassName+"{"); 
                    }                    
                }
                sb.AppendLine(line);
            }
            sb.Append("}");
            return sb.ToString();
        }

        static bool IsReference(string line)
        {
            return line.StartsWith("#r");
        }

        void ExtractReference(string line)
        {
            var namechars = line.Skip(4).TakeWhile(d => d != '"').ToArray();
            var name = new string(namechars);
            _refs.Add(name);
        }

        static Regex regex=new Regex(@"(using|#)+",RegexOptions.Compiled);
        static bool IsPartOfHeader(string line)
        {
            return regex.IsMatch(line);
        }

        static bool IsComment(string line)
        {
            return line.StartsWith("//");
        }
    }
}