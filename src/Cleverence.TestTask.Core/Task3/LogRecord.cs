/*
  ФАЙЛ: LogRecord.cs
  НАЗНАЧЕНИЕ: Описывает нормализованную модель записи лога для задачи 3.

  ЖУРНАЛ ИЗМЕНЕНИЙ:
  22.06.2026 - Первичное создание файла.
  22.06.2026 - Добавлена доменная модель записи лога для архитектурного каркаса задачи 3.
*/

namespace Cleverence.TestTask.Core.Task3;

/// <summary>
/// Представляет нормализованную запись лога, с которой работают парсеры, нормализатор и файловый процессор.
/// </summary>
public sealed class LogRecord
{
    /// <summary>
    /// Инициализирует новую запись лога.
    /// </summary>
    /// <param name="date">Дата события.</param>
    /// <param name="time">Время события в исходном строковом формате.</param>
    /// <param name="level">Уровень логирования.</param>
    /// <param name="method">Имя вызвавшего метода.</param>
    /// <param name="message">Текст сообщения.</param>
    /// <remarks>Модель хранит данные в уже выделенных полях, чтобы следующие слои не зависели от исходного формата строки.</remarks>
    public LogRecord(DateOnly date, string time, string level, string method, string message)
    {
        Date = date;
        Time = time ?? throw new ArgumentNullException(nameof(time));
        Level = level ?? throw new ArgumentNullException(nameof(level));
        Method = method ?? throw new ArgumentNullException(nameof(method));
        Message = message ?? throw new ArgumentNullException(nameof(message));
    }

    /// <summary>
    /// Получает дату события.
    /// </summary>
    public DateOnly Date { get; }

    /// <summary>
    /// Получает время события в исходном строковом формате.
    /// </summary>
    public string Time { get; }

    /// <summary>
    /// Получает уровень логирования.
    /// </summary>
    public string Level { get; }

    /// <summary>
    /// Получает имя вызвавшего метода.
    /// </summary>
    public string Method { get; }

    /// <summary>
    /// Получает текст сообщения.
    /// </summary>
    public string Message { get; }
}
