/*
  ФАЙЛ: CountServer.cs
  НАЗНАЧЕНИЕ: Задает каркас статического сервера счетчика для задачи 2 с параллельным чтением и эксклюзивной записью.

  ЖУРНАЛ ИЗМЕНЕНИЙ:
  22.06.2026 - Первичное создание файла.
  22.06.2026 - Добавлен архитектурный каркас статического сервера счетчика для задачи 2.
  22.06.2026 - Реализован безопасный сброс состояния для изоляции тестовых прогонов.
  22.06.2026 - Реализованы потокобезопасные операции чтения и записи для задачи 2.
  22.06.2026 - Добавлен внутренний helper выполнения под write-lock для воспроизводимых конкурентных тестов.
*/

using System.Threading;

namespace Cleverence.TestTask.Core.Task2;

/// <summary>
/// Представляет статический сервер счетчика с разделением доступа на параллельное чтение и эксклюзивную запись.
/// </summary>
public static class CountServer
{
    private static readonly ReaderWriterLockSlim CountLock = new(LockRecursionPolicy.NoRecursion);
    private static int _count;

    /// <summary>
    /// Возвращает текущее значение счетчика.
    /// </summary>
    /// <returns>Текущее значение общего счетчика.</returns>
    /// <remarks>Метод использует режим чтения, чтобы несколько клиентов могли читать значение параллельно.</remarks>
    public static int GetCount()
    {
        CountLock.EnterReadLock();

        try
        {
            // Чтение выполняем под read-lock, чтобы несколько читателей не блокировали друг друга без необходимости.
            return _count;
        }
        finally
        {
            CountLock.ExitReadLock();
        }
    }

    /// <summary>
    /// Добавляет указанное значение к общему счетчику.
    /// </summary>
    /// <param name="value">Значение, которое нужно прибавить к счетчику.</param>
    /// <remarks>Метод использует эксклюзивный режим записи, чтобы исключить одновременное изменение счетчика.</remarks>
    public static void AddToCount(int value)
    {
        CountLock.EnterWriteLock();

        try
        {
            // Изменение счетчика выполняем только внутри write-lock, чтобы читатели ждали завершения записи.
            _count += value;
        }
        finally
        {
            CountLock.ExitWriteLock();
        }
    }

    /// <summary>
    /// Сбрасывает внутреннее состояние счетчика для воспроизводимого тестового прогона.
    /// </summary>
    /// <remarks>Метод нужен только для изоляции автоматических тестов и не является бизнес-операцией сервера.</remarks>
    internal static void Reset()
    {
        ExecuteWithWriteLock(() =>
        {
            // Сброс состояния нужен для повторяемости тестов и должен быть тем же образом сериализован, что и обычная запись.
            _count = 0;
        });
    }

    /// <summary>
    /// Выполняет действие под эксклюзивной блокировкой записи.
    /// </summary>
    /// <param name="action">Действие, которое нужно выполнить внутри write-lock.</param>
    /// <remarks>Метод нужен для воспроизводимых инфраструктурных и тестовых сценариев, где важно явно удержать запись.</remarks>
    internal static void ExecuteWithWriteLock(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        CountLock.EnterWriteLock();

        try
        {
            // Централизуем выполнение под write-lock, чтобы тестовые сценарии и служебные операции использовали тот же механизм синхронизации.
            action();
        }
        finally
        {
            CountLock.ExitWriteLock();
        }
    }
}
