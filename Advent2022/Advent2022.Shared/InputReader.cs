namespace Advent2022.Shared
{
    using System.Reflection;
    using Kurukuru;
    using Microsoft.Extensions.FileProviders;

    public class InputReader
    {
        public static Task<T> ReadAndParse<T>(Assembly assembly, Func<string, T> parse, string file = "input.txt")
            => ReadAndAsyncParse(assembly, input => Task.FromResult(parse(input)), file);

        public static Task<T> ReadAndAsyncParse<T>(Assembly assembly, Func<string, Task<T>> parse, string file = "input.txt")
            => Spinner.StartAsync("Input: Reading", async spinner =>
            {
                try
                {
                    var reader = new InputReader(assembly);
                    var input = reader.ReadFile(file);
                    spinner.Text = "Input: Parsing";
                    var result = await parse(input);
                    spinner.Succeed("Input: Read and parsed");
                    return result;
                }
                catch (Exception ex)
                {
                    spinner.Fail($"Input: {ex.Message}");
                    throw;
                }
            });

        private readonly EmbeddedFileProvider fileProvider;

        private InputReader(Assembly assembly)
        {
            this.fileProvider = new EmbeddedFileProvider(assembly);
        }

        public string ReadFile(string fileName)
        {
            using (var stream = this.GetStream(fileName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private Stream GetStream(string fileName)
        {
            var fileInfo = this.fileProvider.GetFileInfo(fileName);
            if (fileInfo == null || !fileInfo.Exists)
            {
                throw new FileNotFoundException($"Unable to read input file {fileName} from assembly");
            }

            return fileInfo.CreateReadStream();
        }
    }
}
