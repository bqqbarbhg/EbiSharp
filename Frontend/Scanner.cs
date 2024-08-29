using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EbiSharp
{
    class Scanner
    {
        public SourceFile File;

        Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType> {
            { "class", TokenType.KeywordClass },
            { "struct", TokenType.KeywordStruct },
            { "new", TokenType.KeywordNew },
            { "ref", TokenType.KeywordRef },
            { "own", TokenType.KeywordOwn },
        };

        int position = 0;
        int maxPosition;

        public Scanner(SourceFile file)
        {
            File = file;
            maxPosition = file.Source.Length;
        }

        char Peek(int delta = 0)
        {
            int index = position + delta;
            return index < File.Source.Length ? File.Source[index] : '\0';
        }

        char Eat(int count=1)
        {
            char ch = Peek();
            position += count;
            position = Math.Min(position, maxPosition);
            return ch;
        }

        static bool IsSpace(char ch)
        {
            return ch == ' ' || ch == '\t' || ch == '\n' || ch == '\r';
        }

        static bool IsIdentStart(char ch)
        {
            return (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z') || ch == '_';
        }

        static bool IsIdent(char ch)
        {
            return IsIdentStart(ch) || (ch >= '0' && ch <= '9');
        }

        static bool IsNumberStart(char ch)
        {
            return ch >= '0' && ch <= '9';
        }

        static bool IsDigit(char ch)
        {
            return ch >= '0' && ch <= '9';
        }

        static bool IsHexDigit(char ch)
        {
            return (ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'F') || (ch >= 'a' && ch <= 'f');
        }

        void SkipWhitespace()
        {
            while (true)
            {
                while (IsSpace(Peek()))
                    Eat();

                if (Peek(0) == '/' && Peek(1) == '/')
                {
                    Eat(2);

                    while (Peek() != '\n' && Peek() != '\0')
                        Eat();
                }
                else
                    break;
            }
        }

        Location GetLocation()
        {
            return new Location()
            {
                File = File,
                Offset = position,
                Length = 0,
            };
        }

        void ErrorAt(Location location, string message)
        {
            throw new CompilerException(location, message);
        }

        void ErrorHere(string message)
        {
            throw new CompilerException(GetLocation(), message);
        }

        public Token Scan()
        {
            SkipWhitespace();

            Token token = new Token();
            token.Location = GetLocation();
            token.Location.Length = 1;

            char ch = Eat();

            switch ((ch, Peek()))
            {
                case (char c, _) when IsIdentStart(c):
                    token.Type = TokenType.Identifier;
                    while (IsIdent(Peek()))
                        Eat();
                    TokenType keywordToken;
                    if (keywords.TryGetValue(token.Text, out keywordToken))
                        token.Type = keywordToken;
                    break;
                case ('0', 'x'):
                case ('0', 'X'):
                    token.Type = TokenType.HexLiteral;
                    Eat();
                    while (IsHexDigit(Peek()))
                        Eat();
                    break;
                case (char c, _) when IsDigit(c):
                    token.Type = TokenType.NumberLiteral;
                    while (IsDigit(Peek()))
                        Eat();
                    break;
                case ('"', _):
                    Eat();
                    while (Peek() != '"')
                    {
                        if (Peek() == '\0')
                            ErrorAt(token.Location, "Unclosed string literal");
                        if (Eat() == '\\')
                            Eat();
                    }
                    Eat();
                    break;
                case ('=','='): token.Type = TokenType.Equal; Eat(); break;
                case ('=',_): token.Type = TokenType.Assign; break;
                case ('!','='): token.Type = TokenType.NotEqual; Eat(); break;
                case ('!',_): token.Type = TokenType.Not; break;
                case ('+','='): token.Type = TokenType.AddAssign; Eat(); break;
                case ('+',_): token.Type = TokenType.Add; break;
                case ('-','='): token.Type = TokenType.SubAssign; Eat(); break;
                case ('-',_): token.Type = TokenType.Sub; break;
                case ('*','='): token.Type = TokenType.MulAssign; Eat(); break;
                case ('*',_): token.Type = TokenType.Mul; break;
                case ('/','='): token.Type = TokenType.DivAssign; Eat(); break;
                case ('/',_): token.Type = TokenType.Div; break;
                case ('%','='): token.Type = TokenType.ModAssign; Eat(); break;
                case ('%',_): token.Type = TokenType.Mod; break;
                case ('<','='): token.Type = TokenType.LessEqual; Eat(); break;
                case ('<',_): token.Type = TokenType.Less; break;
                case ('>','='): token.Type = TokenType.GreaterEqual; Eat(); break;
                case ('>',_): token.Type = TokenType.Greater; break;
                case ('(',_): token.Type = TokenType.ParenLeft; break;
                case (')',_): token.Type = TokenType.ParenRight; break;
                case ('{',_): token.Type = TokenType.BlockLeft; break;
                case ('}',_): token.Type = TokenType.BlockRight; break;
                case ('[',_): token.Type = TokenType.BracketLeft; break;
                case (']',_): token.Type = TokenType.BracketRight; break;
                case ('&','&'): token.Type = TokenType.And; Eat(); break;
                case ('|','|'): token.Type = TokenType.Or; Eat(); break;
                case ('?',_): token.Type = TokenType.Question; break;
                case (':',_): token.Type = TokenType.Colon; break;
                case (';',_): token.Type = TokenType.Semicolon; break;
                case ('.',_): token.Type = TokenType.Dot; break;
                case (',',_): token.Type = TokenType.Comma; break;
                case ('\0',_): token.Type = TokenType.End; break;
                default:
                    ErrorAt(token.Location, $"Bad character: '{ch}'");
                    break;
            }

            token.Location.Length = position - token.Location.Offset;
            return token;
        }

    }
}
