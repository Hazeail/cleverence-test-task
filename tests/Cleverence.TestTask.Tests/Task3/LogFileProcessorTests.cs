/*
  ФАЙЛ: LogFileProcessorTests.cs
  НАЗНАЧЕНИЕ: Содержит заготовку тестов построчной обработки лог-файла для задачи 3.

  ЖУРНАЛ ИЗМЕНЕНИЙ:
  22.06.2026 - Первичное создание файла.
  22.06.2026 - Добавлена заготовка тестов файлового процессора для архитектурного каркаса задачи 3.
  22.06.2026 - Реализован тест маршрутизации валидных и невалидных строк для задачи 3.
*/

using Cleverence.TestTask.Core.Task3;

namespace Cleverence.TestTask.Tests.Task3;

/// <summary>
/// Содержит проверки маршрутизации валидных и невалидных строк в целевые выходные файлы.
/// </summary>
public sealed class LogFileProcessorTests
{
    /// <summary>
    /// Проверяет, что файловый процессор раскладывает валидные и проблемные строки по разным выходным файлам.
    /// </summary>
    [Fact]
    public void Process_ShouldWriteValidAndProblemLines_ToDifferentOutputs()
    {
        string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);

        try
        {
            string inputFilePath = Path.Combine(tempDirectory, "input.txt");
            string outputFilePath = Path.Combine(tempDirectory, "output.txt");
            string problemsFilePath = Path.Combine(tempDirectory, "problems.txt");

            File.WriteAllLines(
                inputFilePath,
                [
                    "10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'",
                    "2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO-M40-D-410244015546'",
                    "broken line"
                ]);

            LogFileProcessor processor = new(
                [
                    new FirstLogFormatParser(),
                    new SecondLogFormatParser()
                ],
                new LogRecordNormalizer());

            processor.Process(inputFilePath, outputFilePath, problemsFilePath);

            string[] outputLines = File.ReadAllLines(outputFilePath);
            string[] problemLines = File.ReadAllLines(problemsFilePath);

            Assert.Equal(2, outputLines.Length);
            Assert.Equal("2025-03-10\t15:14:49.523\tINFO\tDEFAULT\tВерсия программы: '3.4.0.48729'", outputLines[0]);
            Assert.Equal("2025-03-10\t15:14:51.5882\tINFO\tMobileComputer.GetDeviceId\tКод устройства: '@MINDEO-M40-D-410244015546'", outputLines[1]);

            Assert.Single(problemLines);
            Assert.Equal("broken line", problemLines[0]);
        }
        finally
        {
            if (Directory.Exists(tempDirectory))
            {
                // Тест создает временные файлы ввода и вывода, поэтому в конце удаляем их, чтобы не оставлять мусор в системе.
                Directory.Delete(tempDirectory, recursive: true);
            }
        }
    }
}
