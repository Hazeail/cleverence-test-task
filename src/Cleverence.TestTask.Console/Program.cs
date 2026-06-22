/*
  ФАЙЛ: Program.cs
  НАЗНАЧЕНИЕ: Предоставляет консольную точку входа для ручной проверки задач 1, 2 и 3.

  ЖУРНАЛ ИЗМЕНЕНИЙ:
  22.06.2026 - Первичное создание файла.
  22.06.2026 - Добавлена консольная точка входа для запуска сценариев проверки задач 1, 2 и 3.
*/

using Cleverence.TestTask.Core.Task1;
using Cleverence.TestTask.Core.Task2;
using Cleverence.TestTask.Core.Task3;

namespace Cleverence.TestTask.Console;

/// <summary>
/// Содержит точку входа консольного приложения и маршрутизацию команд ручной проверки.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Запускает консольный сценарий в зависимости от переданной команды.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    /// <remarks>Команды распределены по задачам, чтобы проверяющему было удобно запускать каждый сценарий отдельно.</remarks>
    private static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            PrintHelp();
            return;
        }

        string command = args[0].ToLowerInvariant();

        switch (command)
        {
            case "task1-compress":
                RunTask1Compress(args);
                break;
            case "task1-decompress":
                RunTask1Decompress(args);
                break;
            case "task2-sequence":
                RunTask2Sequence(args);
                break;
            case "task2-parallel":
                RunTask2Parallel(args);
                break;
            case "task3":
                RunTask3(args);
                break;
            case "help":
            case "--help":
            case "-h":
                PrintHelp();
                break;
            default:
                // Для неизвестной команды сразу показываем справку, чтобы пользователь видел допустимый формат запуска.
                global::System.Console.WriteLine($"Неизвестная команда: {args[0]}");
                PrintHelp();
                break;
        }
    }

    /// <summary>
    /// Выполняет сжатие строки для задачи 1.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    /// <remarks>Команда ожидает строку после имени команды и печатает только результат сжатия.</remarks>
    private static void RunTask1Compress(string[] args)
    {
        if (args.Length < 2)
        {
            global::System.Console.WriteLine("Формат: task1-compress <строка>");
            return;
        }

        IStringCompressionService service = new StringCompressionService();
        string result = service.Compress(args[1]);
        global::System.Console.WriteLine(result);
    }

    /// <summary>
    /// Выполняет декомпрессию строки для задачи 1.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    /// <remarks>Команда ожидает сжатую строку после имени команды и печатает восстановленный результат.</remarks>
    private static void RunTask1Decompress(string[] args)
    {
        if (args.Length < 2)
        {
            global::System.Console.WriteLine("Формат: task1-decompress <строка>");
            return;
        }

        IStringCompressionService service = new StringCompressionService();
        string result = service.Decompress(args[1]);
        global::System.Console.WriteLine(result);
    }

    /// <summary>
    /// Выполняет последовательный сценарий добавления значений к серверу счетчика для задачи 2.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    /// <remarks>Команда принимает список целых чисел и показывает итоговое значение счетчика после последовательного добавления.</remarks>
    private static void RunTask2Sequence(string[] args)
    {
        if (args.Length < 2)
        {
            global::System.Console.WriteLine("Формат: task2-sequence <value1> <value2> ...");
            return;
        }

        foreach (string valueArgument in args[1..])
        {
            if (!int.TryParse(valueArgument, out int value))
            {
                // Прерываем сценарий на первой некорректной записи, чтобы не показывать частично обработанный результат.
                global::System.Console.WriteLine($"Некорректное целое значение: {valueArgument}");
                return;
            }

            CountServer.AddToCount(value);
        }

        global::System.Console.WriteLine($"Итоговое значение счетчика: {CountServer.GetCount()}");
    }

    /// <summary>
    /// Выполняет параллельный сценарий записи в сервер счетчика для задачи 2.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    /// <remarks>Команда принимает количество писателей и число повторов на каждого писателя.</remarks>
    private static void RunTask2Parallel(string[] args)
    {
        if (args.Length != 3)
        {
            global::System.Console.WriteLine("Формат: task2-parallel <число_писателей> <число_повторов>");
            return;
        }

        if (!int.TryParse(args[1], out int writerCount) || writerCount <= 0)
        {
            global::System.Console.WriteLine($"Некорректное число писателей: {args[1]}");
            return;
        }

        if (!int.TryParse(args[2], out int iterationCount) || iterationCount <= 0)
        {
            global::System.Console.WriteLine($"Некорректное число повторов: {args[2]}");
            return;
        }

        Task[] tasks = new Task[writerCount];

        for (int writerIndex = 0; writerIndex < writerCount; writerIndex++)
        {
            tasks[writerIndex] = Task.Run(() =>
            {
                for (int iteration = 0; iteration < iterationCount; iteration++)
                {
                    // Параллельный сценарий нужен для наглядной ручной проверки, что сервер не теряет обновления под нагрузкой.
                    CountServer.AddToCount(1);
                }
            });
        }

        Task.WaitAll(tasks);
        global::System.Console.WriteLine($"Итоговое значение счетчика: {CountServer.GetCount()}");
    }

    /// <summary>
    /// Запускает обработку лог-файла для задачи 3.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    /// <remarks>Команда принимает пути к входному файлу, основному выходному файлу и файлу проблемных строк.</remarks>
    private static void RunTask3(string[] args)
    {
        if (args.Length != 4)
        {
            global::System.Console.WriteLine("Формат: task3 <inputFilePath> <outputFilePath> <problemsFilePath>");
            return;
        }

        LogFileProcessor processor = new(
            [
                new FirstLogFormatParser(),
                new SecondLogFormatParser()
            ],
            new LogRecordNormalizer());

        processor.Process(args[1], args[2], args[3]);
        global::System.Console.WriteLine("Обработка лог-файла завершена.");
    }

    /// <summary>
    /// Печатает краткую справку по доступным командам.
    /// </summary>
    /// <remarks>Справка служит входной точкой для ручной проверки и должна оставаться максимально короткой и прикладной.</remarks>
    private static void PrintHelp()
    {
        global::System.Console.WriteLine("Доступные команды:");
        global::System.Console.WriteLine("  task1-compress <строка>");
        global::System.Console.WriteLine("  task1-decompress <строка>");
        global::System.Console.WriteLine("  task2-sequence <value1> <value2> ...");
        global::System.Console.WriteLine("  task2-parallel <число_писателей> <число_повторов>");
        global::System.Console.WriteLine("  task3 <inputFilePath> <outputFilePath> <problemsFilePath>");
    }
}
