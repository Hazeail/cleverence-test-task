/*
  ФАЙЛ: CountServerTestCollection.cs
  НАЗНАЧЕНИЕ: Отключает параллельный запуск тестов задачи 2 из-за общего статического состояния сервера счетчика.

  ЖУРНАЛ ИЗМЕНЕНИЙ:
  22.06.2026 - Первичное создание файла.
  22.06.2026 - Добавлена непараллельная коллекция тестов для корректной изоляции статического CountServer.
*/

namespace Cleverence.TestTask.Tests.Task2;

/// <summary>
/// Объединяет тесты задачи 2 в непараллельную коллекцию, потому что они работают с общим статическим сервером.
/// </summary>
[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class CountServerTestCollection
{
    /// <summary>
    /// Имя коллекции тестов статического сервера счетчика.
    /// </summary>
    public const string Name = "CountServer collection";
}
