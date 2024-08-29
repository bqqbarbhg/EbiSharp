using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbiSharp
{
    internal abstract class Ast
    {
        public abstract Location? Location { get; }
    }

    abstract class AstType : Ast
    {

    }

    class AstField : Ast
    {
        public override Location? Location => Name.Location;

        public AstType Type;
        public Token Name;
    }

    class AstStruct : Ast
    {
        public override Location? Location => Name.Location;

        public Token Keyword;
        public Token Name;
        public AstField[] Fields;
    }

    class AstTop : Ast
    {
        public override Location? Location => null;
    }
}
