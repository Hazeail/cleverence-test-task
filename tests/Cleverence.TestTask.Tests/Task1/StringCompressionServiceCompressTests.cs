/*
  ФАЙЛ: StringCompressionServiceCompressTests.cs
  НАЗНАЧЕНИЕ: Содержит тесты сценариев сжатия строк для задачи 1.

  ЖУРНАЛ ИЗМЕНЕНИЙ:
  22.06.2026 - Первичное создание файла.
  22.06.2026 - Добавлены тесты сжатия строк для рабочей реализации задачи 1.
  22.06.2026 - Перенесен в структуру Task1 для явного разделения тестов по задачам.
*/

using Cleverence.TestTask.Core.Task1;

namespace Cleverence.TestTask.Tests.Task1;

/// <summary>
/// Проверяет корректность операций сжатия строк.
/// </summary>
public sealed class StringCompressionServiceCompressTests
{
    /// <summary>
    /// Проверяет, что сервис сжимает строку из примера задания в ожидаемый формат.
    /// </summary>
    [Fact]
    public void Compress_ShouldReturnExpectedValue_ForTaskExample()
    {
        StringCompressionService service = new();

        string result = service.Compress("aaabbcccdde");

        Assert.Equal("a3b2c3d2e", result);
    }

    /// <summary>
    /// Проверяет, что одиночные символы сохраняются без числового счетчика.
    /// </summary>
    [Fact]
    public void Compress_ShouldKeepSingleCharacters_WithoutCounter()
    {
        StringCompressionService service = new();

        string result = service.Compress("abc");

        Assert.Equal("abc", result);
    }

    /// <summary>
    /// Проверяет, что сервис отклоняет исходную строку с недопустимыми символами.
    /// </summary>
    [Fact]
    public void Compress_ShouldThrowArgumentException_ForInvalidCharacters()
    {
        StringCompressionService service = new();

        Assert.Throws<ArgumentException>(() => service.Compress("abC1"));
    }
}
