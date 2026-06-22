/*
  ФАЙЛ: CountServerConcurrencyTests.cs
  НАЗНАЧЕНИЕ: Содержит заготовку конкурентных тестов для статического сервера счетчика из задачи 2.

  ЖУРНАЛ ИЗМЕНЕНИЙ:
  22.06.2026 - Первичное создание файла.
  22.06.2026 - Добавлена заготовка конкурентных тестов для архитектурного каркаса задачи 2.
*/

using Cleverence.TestTask.Core.Task2;

namespace Cleverence.TestTask.Tests.Task2;

/// <summary>
/// Содержит проверки параллельного чтения и эксклюзивной записи для сервера счетчика.
/// </summary>
public sealed class CountServerConcurrencyTests
{
    /// <summary>
    /// Подготавливает сервер к следующему тесту через сброс общего состояния.
    /// </summary>
    public CountServerConcurrencyTests()
    {
        // Для конкурентных тестов особенно важно начинать с предсказуемого состояния общего сервера.
        CountServer.Reset();
    }

    /// <summary>
    /// Проверяет, что несколько писателей не теряют обновления счетчика при параллельной работе.
    /// </summary>
    [Fact(Skip = "Тест будет включен после реализации сервера задачи 2.")]
    public void AddToCount_ShouldPreserveAllUpdates_WhenWritersRunInParallel()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Проверяет, что чтение ожидает завершения записи, если сервер удерживает эксклюзивную блокировку.
    /// </summary>
    [Fact(Skip = "Тест будет включен после реализации сервера задачи 2.")]
    public void GetCount_ShouldWaitWhileWriterHoldsLock()
    {
        throw new NotImplementedException();
    }
}
