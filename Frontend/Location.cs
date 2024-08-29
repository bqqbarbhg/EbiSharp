using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbiSharp
{
    struct Location
    {
        public SourceFile File;
        public int Offset, Length;

        public string Text => File.Source.Substring(Offset, Length);
        public LineColumn LineColumn => File.ResolveLineColumn(Offset);

        public string FormatContext()
        {
            LineColumn lineCol = LineColumn;
            string line = File.GetLine(lineCol.Line).TrimEnd();

            StringBuilder builder = new();

            int offsetLeft = lineCol.Column;
            int countLeft = Math.Max(Length, 1);

            int position = 0;
            int caretLength = 0;
            bool startOffset = true;
            foreach (char ch in line)
            {
                char c = ch;
                if (c == '\t')
                    c = ' ';

                offsetLeft--;
                if (c == ' ' && startOffset)
                    continue;

                builder.Append(c);
                startOffset = false;
                if (offsetLeft > 0)
                    position++;
                else
                {
                    if (countLeft > 0)
                        caretLength++;
                    countLeft--;
                }
            }

            string caret;
            if (caretLength <= 1)
                caret = new string(' ', position) + '^';
            else
                caret = new string(' ', position) + '^' + new string('~', caretLength - 2) + '^';
            return builder.ToString() + '\n' + caret;
        }
    }
}
