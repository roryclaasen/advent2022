namespace Advent2022.Shared
{
    using System.Reflection;
    using Microsoft.Extensions.FileProviders;

    public class InputReader
    {
        private readonly EmbeddedFileProvider fileProvider;

        public InputReader(Assembly assembly)
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
                throw new FileNotFoundException(fileName);
            }

            return fileInfo.CreateReadStream();
        }
    }
}
