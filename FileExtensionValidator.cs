namespace CopyFile
{
    internal class FileExtensionValidator
    {
        private static readonly char[] InvalidFileNameChars = Path.GetInvalidFileNameChars();

        private static int CountOccurrences(string input, char targetChar) =>
    input.Count(c => c == targetChar);

        public static string ValidateFileExtension(string extension)
        {
            foreach (char c in extension)
            {
                if (Array.Exists(InvalidFileNameChars, invalidChar => invalidChar == c))
                    throw new ArgumentException("Расширение файла содержит неразрешённые символы.", nameof(extension));
            }
            int dotCount = CountOccurrences(extension, '.');
            if (dotCount == 0)
                return "*." + extension;
            else if (dotCount != 1)
                throw new ArgumentException("Расширение файла содержит несколько символов '.'", nameof(extension));
            return "*" + extension;

        }
    }
}
