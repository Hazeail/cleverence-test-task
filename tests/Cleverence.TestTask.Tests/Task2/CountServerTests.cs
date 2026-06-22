/*
  ФАЙЛ: CountServerTests.cs
  НАЗНАЧЕНИЕ: Содержит заготовку базовых тестов для статического сервера счетчика из задачи 2.

  ЖУРНАЛ ИЗМЕНЕНИЙ:
  22.06.2026 - Первичное создание файла.
  22.06.2026 - Добавлена заготовка базовых тестов для архитектурного каркаса задачи 2.
  22.06.2026 - Реализованы базовые тесты чтения и записи для задачи 2.
*/

using Cleverence.TestTask.Core.Task2;

namespace Cleverence.TestTask.Tests.Task2;

/// <summary>
/// Содержит базовые проверки контракта сервера счетчика без сложных конкурентных сценариев.
/// </summary>
public sealed class CountServerTests
{
    /// <summary>
    /// Подготавливает сервер к следующему тесту через сброс общего состояния.
    /// </summary>
    public CountServerTests()
    {
        // Каждый тест должен начинаться с чистого состояния, иначе общий статический счетчик даст ложные результаты.
        CountServer.Reset();
    }

    /// <summary>
    /// Проверяет, что сервер умеет возвращать текущее значение счетчика.
    /// </summary>
    [Fact]
    public void GetCount_ShouldReturnCurrentValue()
    {
        CountServer.AddToCount(7);

        int result = CountServer.GetCount();

        Assert.Equal(7, result);
    }

    /// <summary>
    /// Проверяет, что сервер корректно добавляет значение к счетчику.
    /// </summary>
    [Fact]
    public void AddToCount_ShouldIncreaseCounter()
    {
        CountServer.AddToCount(3);
        CountServer.AddToCount(4);

        int result = CountServer.GetCount();

        Assert.Equal(7, result);
    }
}
