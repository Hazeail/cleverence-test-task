/*
  ФАЙЛ: LogFileProcessor.cs
  НАЗНАЧЕНИЕ: Задает каркас построчной обработки входного лог-файла для задачи 3.

  ЖУРНАЛ ИЗМЕНЕНИЙ:
  22.06.2026 - Первичное создание файла.
  22.06.2026 - Добавлен архитектурный каркас файлового процессора лога для задачи 3.
  22.06.2026 - Реализована построчная обработка входного файла с раздельной записью валидных и проблемных строк.
*/

namespace Cleverence.TestTask.Core.Task3;

/// <summary>
/// Выполняет построчную обработку входного лог-файла через цепочку парсеров и нормализатор.
/// </summary>
public sealed class LogFileProcessor
{
    private readonly IReadOnlyCollection<ILogLineParser> _parsers;
    private readonly LogRecordNormalizer _normalizer;

    /// <summary>
    /// Инициализирует новый файловый процессор лога.
    /// </summary>
    /// <param name="parsers">Коллекция доступных парсеров входных форматов.</param>
    /// <param name="normalizer">Нормализатор и форматировщик записи.</param>
    /// <remarks>Композиция через конструктор позволяет изолированно тестировать обработку без жесткой привязки к конкретным реализациям.</remarks>
    public LogFileProcessor(IReadOnlyCollection<ILogLineParser> parsers, LogRecordNormalizer normalizer)
    {
        _parsers = parsers ?? throw new ArgumentNullException(nameof(parsers));
        _normalizer = normalizer ?? throw new ArgumentNullException(nameof(normalizer));
    }

    /// <summary>
    /// Обрабатывает входной лог-файл и записывает результаты в целевые файлы.
    /// </summary>
    /// <param name="inputFilePath">Путь к входному файлу.</param>
    /// <param name="outputFilePath">Путь к основному выходному файлу.</param>
    /// <param name="problemsFilePath">Путь к файлу проблемных строк.</param>
    /// <remarks>Каждая строка маршрутизируется либо в основной выход после нормализации, либо в файл проблем без изменения исходного содержимого.</remarks>
    public void Process(string inputFilePath, string outputFilePath, string problemsFilePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(inputFilePath);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputFilePath);
        ArgumentException.ThrowIfNullOrWhiteSpace(problemsFilePath);

        using StreamWriter outputWriter = new(outputFilePath, append: false);
        using StreamWriter problemsWriter = new(problemsFilePath, append: false);

        foreach (string line in File.ReadLines(inputFilePath))
        {
            if (TryParseLine(line, out LogRecord? record))
            {
                // Валидную строку сначала нормализуем, а затем пишем в согласованный выходной формат.
                outputWriter.WriteLine(_normalizer.Format(record!));
                continue;
            }

            // Невалидную запись сохраняем как есть, чтобы не потерять исходный материал для ручного разбора.
            problemsWriter.WriteLine(line);
        }
    }

    /// <summary>
    /// Пытается разобрать строку через все зарегистрированные парсеры.
    /// </summary>
    /// <param name="line">Исходная строка лога.</param>
    /// <param name="record">Результирующая запись при успешном разборе.</param>
    /// <returns>Истина, если хотя бы один парсер распознал строку.</returns>
    /// <remarks>Метод выделен отдельно, чтобы маршрут выбора формата не дублировался внутри основного цикла обработки файла.</remarks>
    private bool TryParseLine(string line, out LogRecord? record)
    {
        foreach (ILogLineParser parser in _parsers)
        {
            if (parser.TryParse(line, out record))
            {
                return true;
            }
        }

        record = null;
        return false;
    }
}
