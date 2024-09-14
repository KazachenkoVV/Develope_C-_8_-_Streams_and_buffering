// Объедините две предыдущих работы (практические работы 2 и 3):
// поиск файла и поиск текста в файле,  написав  утилиту,
// которая  ищет  файлы  определенного  расширения  с  указанным текстом.
// Рекурсивно. Пример вызова утилиты: utility.exe txt текст

using CopyFile;
using static System.Net.Mime.MediaTypeNames;
using System.IO;

namespace SearchFileWithText
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Программа осуществляет поиск заданной фразы в файлах, с указанным расширением.");
            Console.WriteLine("В случае нахождения, она выводит на консоль строки из этих файлов,");
            Console.WriteLine("которые содержат искомую фразу, перед этим фораза преобразуется в верхний регистр.\n");
            Console.WriteLine("Программа принимает на вход два аргумента типа string:");
            Console.WriteLine(" - расширение искомых файлов");
            Console.WriteLine(" - искомый текст\n");
            Console.WriteLine("Пример испльзования:");
            Console.WriteLine("SearchFileWithText txt \"Пример вызова утилиты\"\n");

            if (args.Length != 2)
                throw new ArgumentException($"Программа требует 2 аргумента! Вы указали - {args.Length}");

            string? path = null;
            while (path == null)
                path = InputPath("Укажите путь к директории в которой осуществлять поиск - ");

            string ext = FileExtensionValidator.ValidateFileExtension(args[0]);
            string searchingText = args[1];
            /*
            // Красивое и короткое решение, которое не может обрабатывать права доступа к директрориям... :(
            // Поэтому не проходит!

            var files = Directory.EnumerateFiles(path, ext, SearchOption.AllDirectories);
            foreach (var file in files)
            {
                Console.WriteLine(file);
                // Функция обработки файла.
            }
            */
            SearchForMaskedFilesByExtension(path, ext, searchingText);
        }
        private static string InputPath(string str)
        {
            Console.Write(str);
            string? path = Console.ReadLine();
            if (Directory.Exists(path))
                return path;
            else if (File.Exists(path))
                Console.WriteLine($"Введённый путь {path} - указывает на файл, а не да директорию\nПовторите ввод.");
            else
                Console.WriteLine($"Указанный путь {path} - не существует!\nПовторите ввод.");
            return null;
        }
        static void SearchForMaskedFilesByExtension(string inputPath, string extension, string mask)
        {
            if (AccessControl.HasAccess(inputPath))
            {
                var files = Directory.EnumerateFiles(inputPath, extension);
                foreach (var file in files)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(file);
                    Console.ResetColor();
                    SearchForAWordInALineOfFile(file, mask);
                }
                var directories = Directory.EnumerateDirectories(inputPath);
                foreach (var dir in directories)
                    SearchForMaskedFilesByExtension(dir, extension, mask);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(inputPath);
                Console.ResetColor();
                Console.WriteLine(" <- Отсутствуют права доступа!");
            }
        }
        static void SearchForAWordInALineOfFile(string file, string template)
        {
            foreach (var str in File.ReadLines(file))
                if (str.Contains(template))
                    Console.WriteLine(str.Replace(template, template.ToUpper()));
        }
    }
}
