/*
  ФАЙЛ: FirstLogFormatParserTests.cs
  НАЗНАЧЕНИЕ: Содержит заготовку тестов парсера первого формата лога для задачи 3.

  ЖУРНАЛ ИЗМЕНЕНИЙ:
  22.06.2026 - Первичное создание файла.
  22.06.2026 - Добавлена заготовка тестов парсера первого формата для архитектурного каркаса задачи 3.
  22.06.2026 - Реализованы тесты парсера первого формата для задачи 3.
*/

using Cleverence.TestTask.Core.Task3;

namespace Cleverence.TestTask.Tests.Task3;

/// <summary>
/// Содержит проверки парсинга первого формата входной строки лога.
/// </summary>
public sealed class FirstLogFormatParserTests
{
    /// <summary>
    /// Проверяет, что парсер первого формата корректно извлекает поля из валидной строки.
    /// </summary>
    [Fact]
    public void TryParse_ShouldParseValidFirstFormatLine()
    {
        FirstLogFormatParser parser = new();

        bool parsed = parser.TryParse(
            "10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'",
            out LogRecord? record);

        Assert.True(parsed);
        Assert.NotNull(record);
        Assert.Equal(new DateOnly(2025, 3, 10), record.Date);
        Assert.Equal("15:14:49.523", record.Time);
        Assert.Equal("INFORMATION", record.Level);
        Assert.Equal(string.Empty, record.Method);
        Assert.Equal("Версия программы: '3.4.0.48729'", record.Message);
    }

    /// <summary>
    /// Проверяет, что строка без обязательных полей не считается валидной для первого формата.
    /// </summary>
    [Fact]
    public void TryParse_ShouldReturnFalse_ForInvalidFirstFormatLine()
    {
        FirstLogFormatParser parser = new();

        bool parsed = parser.TryParse("10.03.2025 broken line", out LogRecord? record);

        Assert.False(parsed);
        Assert.Null(record);
    }
}
