using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbiSharp
{
    enum TokenType
    {
        None,

        Identifier,
        NumberLiteral,
        HexLiteral,

        Assign,
        Equal,
        Not,
        NotEqual,

        Add,
        AddAssign,
        Sub,
        SubAssign,
        Mul,
        MulAssign,
        Div,
        DivAssign,
        Mod,
        ModAssign,

        Less,
        LessEqual,
        Greater,
        GreaterEqual,

        ParenLeft,
        ParenRight,
        BlockLeft,
        BlockRight,
        BracketLeft,
        BracketRight,

        And,
        Or,
        Question,
        Colon,
        Semicolon,
        Dot,
        Comma,

        End,

        KeywordClass,
        KeywordStruct,
        KeywordNew,
        KeywordRef,
        KeywordOwn,
    }

    struct Token
    {
        public TokenType Type;
        public Location Location;

        public string Text => Location.Text;
    }
}
