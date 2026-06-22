/*
  ФАЙЛ: LogRecordNormalizer.cs
  НАЗНАЧЕНИЕ: Задает каркас нормализации и форматирования записей лога для задачи 3.

  ЖУРНАЛ ИЗМЕНЕНИЙ:
  22.06.2026 - Первичное создание файла.
  22.06.2026 - Добавлен архитектурный каркас нормализатора записей лога для задачи 3.
  22.06.2026 - Реализованы нормализация полей и форматирование выходной строки для задачи 3.
*/

namespace Cleverence.TestTask.Core.Task3;

/// <summary>
/// Нормализует значения полей записи лога и формирует итоговую строку выходного формата.
/// </summary>
public sealed class LogRecordNormalizer
{
    /// <summary>
    /// Возвращает нормализованную запись лога по правилам задачи.
    /// </summary>
    /// <param name="record">Исходная структурированная запись.</param>
    /// <returns>Нормализованная запись лога.</returns>
    /// <remarks>Нормализатор централизует маппинг уровня логирования, подстановку метода и политику представления даты.</remarks>
    public LogRecord Normalize(LogRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);

        string normalizedLevel = NormalizeLevel(record.Level);
        string normalizedMethod = string.IsNullOrWhiteSpace(record.Method) ? "DEFAULT" : record.Method;

        // Нормализацию выделяем отдельно от парсинга, чтобы оба входных формата сходились к единому выходному контракту.
        return new LogRecord(record.Date, record.Time, normalizedLevel, normalizedMethod, record.Message);
    }

    /// <summary>
    /// Формирует строку выходного формата из нормализованной записи.
    /// </summary>
    /// <param name="record">Нормализованная запись лога.</param>
    /// <returns>Строка итогового выходного формата.</returns>
    /// <remarks>Форматирование выполняется табуляцией и сохраняет время и сообщение без дополнительного преобразования.</remarks>
    public string Format(LogRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);

        LogRecord normalizedRecord = Normalize(record);
        return string.Join(
            "\t",
            FormatDate(normalizedRecord.Date),
            normalizedRecord.Time,
            normalizedRecord.Level,
            normalizedRecord.Method,
            normalizedRecord.Message);
    }

    /// <summary>
    /// Возвращает целевое строковое представление даты для выходного файла.
    /// </summary>
    /// <param name="date">Дата записи.</param>
    /// <returns>Строковое представление даты выходного формата.</returns>
    /// <remarks>В текущем решении принят формат из примеров ТЗ: `yyyy-MM-dd`.</remarks>
    private static string FormatDate(DateOnly date)
    {
        return date.ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// Преобразует уровень логирования к согласованному выходному набору значений.
    /// </summary>
    /// <param name="level">Исходное значение уровня логирования.</param>
    /// <returns>Нормализованное значение уровня логирования.</returns>
    /// <remarks>Неизвестные, но непустые значения переводятся в верхний регистр без дополнительного маппинга.</remarks>
    private static string NormalizeLevel(string level)
    {
        string normalizedLevel = (level ?? throw new ArgumentNullException(nameof(level))).Trim().ToUpperInvariant();

        return normalizedLevel switch
        {
            "INFORMATION" => "INFO",
            "WARNING" => "WARN",
            _ => normalizedLevel
        };
    }
}
