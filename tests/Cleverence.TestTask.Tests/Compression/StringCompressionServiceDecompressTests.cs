/*
  ФАЙЛ: StringCompressionServiceDecompressTests.cs
  НАЗНАЧЕНИЕ: Содержит тесты сценариев декомпрессии строк для задачи 1.

  ЖУРНАЛ ИЗМЕНЕНИЙ:
  22.06.2026 - Первичное создание файла.
  22.06.2026 - Добавлены тесты декомпрессии строк для рабочей реализации задачи 1.
*/

using Cleverence.TestTask.Core.Compression;

namespace Cleverence.TestTask.Tests.Compression;

/// <summary>
/// Проверяет корректность операций декомпрессии строк.
/// </summary>
public sealed class StringCompressionServiceDecompressTests
{
    /// <summary>
    /// Проверяет, что сервис восстанавливает строку из примера задания.
    /// </summary>
    [Fact]
    public void Decompress_ShouldReturnExpectedValue_ForTaskExample()
    {
        StringCompressionService service = new();

        string result = service.Decompress("a3b2c3d2e");

        Assert.Equal("aaabbcccdde", result);
    }

    /// <summary>
    /// Проверяет, что форма записи одиночного символа со счетчиком считается невалидной.
    /// </summary>
    [Fact]
    public void Decompress_ShouldThrowFormatException_ForSingleCharacterCounter()
    {
        StringCompressionService service = new();

        Assert.Throws<FormatException>(() => service.Decompress("a1"));
    }

    /// <summary>
    /// Проверяет, что строка не может начинаться с числа без ведущего символа.
    /// </summary>
    [Fact]
    public void Decompress_ShouldThrowFormatException_ForLeadingDigit()
    {
        StringCompressionService service = new();

        Assert.Throws<FormatException>(() => service.Decompress("3a"));
    }
}
