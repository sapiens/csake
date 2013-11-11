//using System.Text.RegularExpressions;

//namespace CSake.Internals
//{
//    public abstract class BaseLineInterpretor : IInterpretLine
//    {
//        private readonly string _word;

//        protected BaseLineInterpretor(LineType type,string word)
//        {
//            _word = word;
//            Type = type;
//        }

//        public LineType Type
//        {
//            get; private set;

//        }
//        public virtual bool CanInterpret(string line)
//        {
//            var regex=new Regex(@"(using|)")
//            return line.StartsWith(_word);
            
//        }
//    }



//   public class UsingInterpretor : BaseLineInterpretor
//    {
//        public UsingInterpretor() : base(LineType.Using, "using")
//        {
//        }
//    }

//public class CommentInterpretor : BaseLineInterpretor
//    {
//        public CommentInterpretor() : base(LineType.Comment, "//")
//        {
//        }
//    }

//public class StartBraceInterpretor : BaseLineInterpretor
//    {
//        public StartBraceInterpretor() : base(LineType.StartBrace, "{")
//        {
//        }
//    }

//public class NamespaceInterpretor : BaseLineInterpretor
//    {
//        public NamespaceInterpretor() : base(LineType.Namespace, "namespace")
//        {
//        }
//    }

//public class ClassDefinitionInterpretor : BaseLineInterpretor
//    {
//        public ClassDefinitionInterpretor() : base(LineType.ClassDefinition, "class")
//        {
//        }

//    public override bool CanInterpret(string line)
//    {
//        if (!base.CanInterpret(line))
//        {
//            return line.st
//        }
//    }
//    }

//class UsingInterpretor : BaseLineInterpretor
//    {
//        public UsingInterpretor() : base(LineType.Using, "using")
//        {
//        }
//    }

//class UsingInterpretor : BaseLineInterpretor
//    {
//        public UsingInterpretor() : base(LineType.Using, "using")
//        {
//        }
//    }


//}