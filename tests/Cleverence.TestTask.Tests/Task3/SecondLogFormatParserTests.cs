/*
  ФАЙЛ: SecondLogFormatParserTests.cs
  НАЗНАЧЕНИЕ: Содержит заготовку тестов парсера второго формата лога для задачи 3.

  ЖУРНАЛ ИЗМЕНЕНИЙ:
  22.06.2026 - Первичное создание файла.
  22.06.2026 - Добавлена заготовка тестов парсера второго формата для архитектурного каркаса задачи 3.
  22.06.2026 - Реализованы тесты парсера второго формата для задачи 3.
*/

using Cleverence.TestTask.Core.Task3;

namespace Cleverence.TestTask.Tests.Task3;

/// <summary>
/// Содержит проверки парсинга второго формата входной строки лога.
/// </summary>
public sealed class SecondLogFormatParserTests
{
    /// <summary>
    /// Проверяет, что парсер второго формата корректно извлекает поля из валидной строки.
    /// </summary>
    [Fact]
    public void TryParse_ShouldParseValidSecondFormatLine()
    {
        SecondLogFormatParser parser = new();

        bool parsed = parser.TryParse(
            "2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO-M40-D-410244015546'",
            out LogRecord? record);

        Assert.True(parsed);
        Assert.NotNull(record);
        Assert.Equal(new DateOnly(2025, 3, 10), record.Date);
        Assert.Equal("15:14:51.5882", record.Time);
        Assert.Equal("INFO", record.Level);
        Assert.Equal("MobileComputer.GetDeviceId", record.Method);
        Assert.Equal("Код устройства: '@MINDEO-M40-D-410244015546'", record.Message);
    }

    /// <summary>
    /// Проверяет, что строка с некорректным количеством полей не разбирается вторым парсером.
    /// </summary>
    [Fact]
    public void TryParse_ShouldReturnFalse_ForInvalidSecondFormatLine()
    {
        SecondLogFormatParser parser = new();

        bool parsed = parser.TryParse("2025-03-10 15:14:51.5882| INFO|broken", out LogRecord? record);

        Assert.False(parsed);
        Assert.Null(record);
    }
}
