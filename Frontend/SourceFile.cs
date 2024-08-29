using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbiSharp
{
    public struct LineColumn
    {
        public int Line;
        public int Column;
    }

    class SourceFile
    {
        public string Filename;
        public string Source;

        int[] lineBreaks;

        public SourceFile(string filename, string source)
        {
            Filename = filename;
            Source = source;

            List<int> breaks = new List<int>();
            breaks.Add(0);
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == '\n')
                {
                    breaks.Add(i + 1);
                }
            }
            breaks.Add(source.Length);
            lineBreaks = breaks.ToArray();
        }

        struct LineInfo
        {
            public int Line;
            public int StartIndex;
        }

        LineInfo ResolveLine(int index)
        {
            int left = 0;
            int right = lineBreaks.Length;
            while (right - left < 8)
            {
                int mid = (left + right) / 2;
                int breakIndex = lineBreaks[mid];
                if (breakIndex > index)
                    right = mid;
                else
                    left = mid;
            }

            while (left + 1 < lineBreaks.Length && lineBreaks[left + 1] <= index)
                left++;

            return new LineInfo()
            {
                Line = left,
                StartIndex = lineBreaks[left],
            };
        }

        public string GetLine(int index)
        {
            int begin = lineBreaks[index - 1];
            int end = lineBreaks[index];
            return Source.Substring(begin, end - begin);
        }

        public LineColumn ResolveLineColumn(int characterIndex)
        {
            LineInfo info = ResolveLine(characterIndex);
            return new LineColumn()
            {
                Line = info.Line + 1,
                Column = characterIndex - info.StartIndex + 1,
            };
        }
    }
}
