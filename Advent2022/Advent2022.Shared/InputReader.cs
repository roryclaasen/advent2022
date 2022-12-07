namespace Advent2022.Shared
{
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Reflection;
    using Kurukuru;
    using Microsoft.Extensions.FileProviders;

    public class InputReader
    {

        #region Static Read Methods

        private static readonly ConcurrentDictionary<Assembly, InputReader> CachedInputReaders = new();

        public static Task<string> Read(Assembly assembly, string file = "input.txt")
            => Read(assembly, input => input, file);

        public static Task<T> Read<T>(Assembly assembly, Func<string, T> parse, string file = "input.txt")
            => Read(assembly, input => Task.FromResult(parse(input)), file);

        public static Task<T> Read<T>(Assembly assembly, Func<string, Task<T>> parse, string file = "input.txt")
        {
            var prefix = $"Input ({file})";
            return Spinner.StartAsync($"{prefix}: Reading", async spinner =>
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                try
                {
                    var input = CachedInputReaders
                        .GetOrAdd(assembly, a => new InputReader(a))
                        .ReadFile(file);

                    spinner.Text = $"{prefix}: ({stopwatch.ElapsedMilliseconds}ms): Parsing";
                    var result = await parse(input).ConfigureAwait(false);

                    stopwatch.Stop();
                    spinner.Succeed($"{prefix}: ({stopwatch.ElapsedMilliseconds}ms): Processed");
                    return result;
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    spinner.Fail($"{prefix}: ({stopwatch.ElapsedMilliseconds}ms): {ex.Message}");
                    throw;
                }
            });
        }

        #endregion

        #region Class Implementation

        private readonly EmbeddedFileProvider fileProvider;

        private InputReader(Assembly assembly)
        {
            this.fileProvider = new EmbeddedFileProvider(assembly);
        }

        public string ReadFile(string fileName)
        {
            using var stream = this.GetStream(fileName);
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
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

        #endregion
    }
}
