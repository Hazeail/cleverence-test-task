/*
  ФАЙЛ: LogRecordNormalizerTests.cs
  НАЗНАЧЕНИЕ: Содержит заготовку тестов нормализации записей лога для задачи 3.

  ЖУРНАЛ ИЗМЕНЕНИЙ:
  22.06.2026 - Первичное создание файла.
  22.06.2026 - Добавлена заготовка тестов нормализатора для архитектурного каркаса задачи 3.
  22.06.2026 - Реализованы тесты нормализации уровней, метода и форматирования для задачи 3.
*/

using Cleverence.TestTask.Core.Task3;

namespace Cleverence.TestTask.Tests.Task3;

/// <summary>
/// Содержит проверки правил нормализации уровней, метода и итогового формата записи.
/// </summary>
public sealed class LogRecordNormalizerTests
{
    /// <summary>
    /// Проверяет, что нормализатор преобразует значения записи к целевому выходному формату.
    /// </summary>
    [Fact]
    public void Normalize_ShouldApplyTargetOutputRules()
    {
        LogRecordNormalizer normalizer = new();
        LogRecord source = new(new DateOnly(2025, 3, 10), "15:14:49.523", "INFORMATION", string.Empty, "Версия программы");

        LogRecord normalized = normalizer.Normalize(source);

        Assert.Equal("INFO", normalized.Level);
        Assert.Equal("DEFAULT", normalized.Method);
        Assert.Equal("Версия программы", normalized.Message);
    }

    /// <summary>
    /// Проверяет, что форматирование использует табуляцию и согласованный формат даты из примеров ТЗ.
    /// </summary>
    [Fact]
    public void Format_ShouldBuildExpectedOutputLine()
    {
        LogRecordNormalizer normalizer = new();
        LogRecord source = new(new DateOnly(2025, 3, 10), "15:14:49.523", "INFORMATION", string.Empty, "Версия программы");

        string line = normalizer.Format(source);

        Assert.Equal("2025-03-10\t15:14:49.523\tINFO\tDEFAULT\tВерсия программы", line);
    }
}
