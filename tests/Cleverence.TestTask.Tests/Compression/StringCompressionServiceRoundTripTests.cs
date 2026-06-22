/*
  ФАЙЛ: StringCompressionServiceRoundTripTests.cs
  НАЗНАЧЕНИЕ: Содержит тесты симметричности операций сжатия и декомпрессии для задачи 1.

  ЖУРНАЛ ИЗМЕНЕНИЙ:
  22.06.2026 - Первичное создание файла.
  22.06.2026 - Добавлены round-trip тесты для рабочей реализации задачи 1.
*/

using Cleverence.TestTask.Core.Compression;

namespace Cleverence.TestTask.Tests.Compression;

/// <summary>
/// Проверяет согласованность операций сжатия и декомпрессии как единого контракта.
/// </summary>
public sealed class StringCompressionServiceRoundTripTests
{
    /// <summary>
    /// Проверяет, что декомпрессия результата сжатия возвращает исходную строку.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("a")]
    [InlineData("abc")]
    [InlineData("aaabbcccdde")]
    [InlineData("zzzzzyyyyyx")]
    public void CompressAndDecompress_ShouldReturnOriginalValue(string source)
    {
        StringCompressionService service = new();

        string compressed = service.Compress(source);
        string restored = service.Decompress(compressed);

        Assert.Equal(source, restored);
    }
}
