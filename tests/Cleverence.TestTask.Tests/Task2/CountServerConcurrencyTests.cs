/*
  ФАЙЛ: CountServerConcurrencyTests.cs
  НАЗНАЧЕНИЕ: Содержит заготовку конкурентных тестов для статического сервера счетчика из задачи 2.

  ЖУРНАЛ ИЗМЕНЕНИЙ:
  22.06.2026 - Первичное создание файла.
  22.06.2026 - Добавлена заготовка конкурентных тестов для архитектурного каркаса задачи 2.
  22.06.2026 - Реализованы конкурентные тесты записи и ожидания чтения для задачи 2.
  22.06.2026 - Переведены конкурентные тесты на async/await для устранения блокирующих ожиданий xUnit.
*/

using System.Diagnostics;
using Cleverence.TestTask.Core.Task2;

namespace Cleverence.TestTask.Tests.Task2;

/// <summary>
/// Содержит проверки параллельного чтения и эксклюзивной записи для сервера счетчика.
/// </summary>
[Collection(CountServerTestCollection.Name)]
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
    [Fact]
    public async Task AddToCount_ShouldPreserveAllUpdates_WhenWritersRunInParallel()
    {
        const int writerCount = 8;
        const int iterationsPerWriter = 500;

        Task[] tasks = new Task[writerCount];

        for (int writerIndex = 0; writerIndex < writerCount; writerIndex++)
        {
            tasks[writerIndex] = Task.Run(() =>
            {
                for (int iteration = 0; iteration < iterationsPerWriter; iteration++)
                {
                    // Каждый писатель обновляет счетчик многократно, чтобы проверить отсутствие потери записей под нагрузкой.
                    CountServer.AddToCount(1);
                }
            });
        }

        await Task.WhenAll(tasks);

        int result = CountServer.GetCount();

        Assert.Equal(writerCount * iterationsPerWriter, result);
    }

    /// <summary>
    /// Проверяет, что чтение ожидает завершения записи, если сервер удерживает эксклюзивную блокировку.
    /// </summary>
    [Fact]
    public async Task GetCount_ShouldWaitWhileWriterHoldsLock()
    {
        using ManualResetEventSlim writerEntered = new(false);
        using ManualResetEventSlim releaseWriter = new(false);
        using ManualResetEventSlim readerCompleted = new(false);

        Task writerTask = Task.Run(() =>
        {
            CountServer.ExecuteWithWriteLock(() =>
            {
                writerEntered.Set();

                // Удерживаем write-lock до явного сигнала, чтобы проверить, что читатель не сможет пройти раньше времени.
                releaseWriter.Wait();
            });
        });

        Assert.True(writerEntered.Wait(TimeSpan.FromSeconds(2)));

        Stopwatch stopwatch = Stopwatch.StartNew();

        Task readerTask = Task.Run(() =>
        {
            CountServer.GetCount();
            readerCompleted.Set();
        });

        // Пока писатель держит блокировку, чтение не должно завершиться.
        Assert.False(readerCompleted.Wait(TimeSpan.FromMilliseconds(150)));

        releaseWriter.Set();

        await Task.WhenAll(writerTask, readerTask);
        stopwatch.Stop();

        // Дополнительно убеждаемся, что чтение действительно подождало, а не проскочило мгновенно.
        Assert.True(stopwatch.ElapsedMilliseconds >= 150);
    }
}
