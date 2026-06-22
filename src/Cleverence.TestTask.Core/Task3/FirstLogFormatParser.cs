/*
  ФАЙЛ: FirstLogFormatParser.cs
  НАЗНАЧЕНИЕ: Задает каркас парсера первого формата входной строки лога для задачи 3.

  ЖУРНАЛ ИЗМЕНЕНИЙ:
  22.06.2026 - Первичное создание файла.
  22.06.2026 - Добавлен архитектурный каркас парсера первого формата лога для задачи 3.
  22.06.2026 - Реализован разбор первого формата входной строки лога для задачи 3.
*/

namespace Cleverence.TestTask.Core.Task3;

/// <summary>
/// Пытается разобрать строку первого входного формата лога.
/// </summary>
public sealed class FirstLogFormatParser : ILogLineParser
{
    /// <summary>
    /// Пытается разобрать строку первого формата в структурированную запись.
    /// </summary>
    /// <param name="line">Исходная строка лога.</param>
    /// <param name="record">Результирующая запись при успешном разборе.</param>
    /// <returns>Истина, если строка соответствует первому формату.</returns>
    /// <remarks>Первый формат не содержит имени метода, поэтому это поле остается пустым до этапа нормализации.</remarks>
    public bool TryParse(string line, out LogRecord? record)
    {
        record = null;

        if (string.IsNullOrWhiteSpace(line))
        {
            return false;
        }

        if (line.Length < 26)
        {
            // Минимальная длина нужна, чтобы строка гарантированно содержала дату, время, уровень и хотя бы один символ сообщения.
            return false;
        }

        string datePart = line.Substring(0, 10);
        string timePart = line.Substring(11, 12);

        if (!DateOnly.TryParseExact(datePart, "dd.MM.yyyy", out DateOnly date))
        {
            return false;
        }

        if (line[10] != ' ' || line[23] != ' ')
        {
            // Явно проверяем ожидаемые разделители первого формата, чтобы не принять случайно похожую строку.
            return false;
        }

        int levelSeparatorIndex = line.IndexOf(' ', 24);

        if (levelSeparatorIndex < 0 || levelSeparatorIndex >= line.Length - 1)
        {
            return false;
        }

        string level = line.Substring(24, levelSeparatorIndex - 24).Trim();
        string message = line[(levelSeparatorIndex + 1)..];

        if (string.IsNullOrWhiteSpace(level) || string.IsNullOrWhiteSpace(message))
        {
            return false;
        }

        record = new LogRecord(date, timePart, level, string.Empty, message);
        return true;
    }
}
