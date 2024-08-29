using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbiSharp
{
    internal class Parser
    {
        Scanner scanner;

        Token token;
        Token prevToken;

        void ErrorAt(Location location, string message)
        {
            throw new CompilerException(location, message);
        }

        public Parser(Scanner scanner)
        {
            this.scanner = scanner;
            token = prevToken = scanner.Scan();
        }

        void Advance()
        {
            prevToken = token;
            token = scanner.Scan();
        }

        public bool Accept(TokenType type)
        {
            if (token.Type == type)
            {
                Advance();
                return true;
            }
            return false;
        }

        public void Require(TokenType type, string message)
        {
            if (!Accept(type))
                ErrorAt(token.Location, message);
        }

        AstTop ParseTop()
        {
            Require(TokenType.KeywordStruct, "Expected a struct");
            return null;
        }

        public AstTop Parse()
        {
            return ParseTop();
        }
    }
}
