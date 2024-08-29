namespace EbiSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filename = "Test/Test.ebi";
            string text = File.ReadAllText(filename);
            SourceFile sourceFile = new SourceFile(filename, text);
            Scanner scanner = new Scanner(sourceFile);
            Parser parser = new Parser(scanner);

            try
            {
                AstTop top = parser.Parse();
            }
            catch (CompilerException ex)
            {
                SourceFile file = ex.Location.File;
                LineColumn lineCol = ex.Location.LineColumn;
                Console.WriteLine($"{file.Filename}:{lineCol.Line}:{lineCol.Column}: {ex.Message}\n\n{ex.Location.FormatContext()}\n");
            }
        }
    }
}
