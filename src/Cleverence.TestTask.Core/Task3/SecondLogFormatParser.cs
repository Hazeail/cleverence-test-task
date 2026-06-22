/*
  ФАЙЛ: SecondLogFormatParser.cs
  НАЗНАЧЕНИЕ: Задает каркас парсера второго формата входной строки лога для задачи 3.

  ЖУРНАЛ ИЗМЕНЕНИЙ:
  22.06.2026 - Первичное создание файла.
  22.06.2026 - Добавлен архитектурный каркас парсера второго формата лога для задачи 3.
  22.06.2026 - Реализован разбор второго формата входной строки лога для задачи 3.
  22.06.2026 - Уточнено извлечение сообщения второго формата без агрессивного удаления ведущих пробелов.
*/

namespace Cleverence.TestTask.Core.Task3;

/// <summary>
/// Пытается разобрать строку второго входного формата лога.
/// </summary>
public sealed class SecondLogFormatParser : ILogLineParser
{
    /// <summary>
    /// Пытается разобрать строку второго формата в структурированную запись.
    /// </summary>
    /// <param name="line">Исходная строка лога.</param>
    /// <param name="record">Результирующая запись при успешном разборе.</param>
    /// <returns>Истина, если строка соответствует второму формату.</returns>
    /// <remarks>Второй формат содержит служебное числовое поле, которое не требуется в выходной модели и поэтому пропускается.</remarks>
    public bool TryParse(string line, out LogRecord? record)
    {
        record = null;

        if (string.IsNullOrWhiteSpace(line))
        {
            return false;
        }

        string[] parts = line.Split('|');

        if (parts.Length != 5)
        {
            return false;
        }

        string dateTimePart = parts[0].Trim();
        string levelPart = parts[1].Trim();
        string methodPart = parts[3].Trim();
        string messagePart = ExtractMessagePart(parts[4]);

        if (dateTimePart.Length < 12)
        {
            return false;
        }

        int firstSpaceIndex = dateTimePart.IndexOf(' ');

        if (firstSpaceIndex <= 0 || firstSpaceIndex >= dateTimePart.Length - 1)
        {
            return false;
        }

        string datePart = dateTimePart[..firstSpaceIndex];
        string timePart = dateTimePart[(firstSpaceIndex + 1)..];

        if (!DateOnly.TryParseExact(datePart, "yyyy-MM-dd", out DateOnly date))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(levelPart) || string.IsNullOrWhiteSpace(messagePart))
        {
            return false;
        }

        record = new LogRecord(date, timePart, levelPart, methodPart, messagePart);
        return true;
    }

    /// <summary>
    /// Извлекает текст сообщения из последнего поля второго формата.
    /// </summary>
    /// <param name="rawMessagePart">Сырое поле сообщения после разбиения строки по разделителю.</param>
    /// <returns>Сообщение без служебного разделительного пробела после символа `|`.</returns>
    /// <remarks>Удаляется только один разделительный пробел, если он стоит сразу после `|`, а не произвольное количество пробелов из начала сообщения.</remarks>
    private static string ExtractMessagePart(string rawMessagePart)
    {
        if (rawMessagePart.Length == 0)
        {
            return string.Empty;
        }

        // Во втором формате после разделителя обычно стоит один технический пробел, который не относится к самому сообщению.
        return rawMessagePart[0] == ' ' ? rawMessagePart[1..] : rawMessagePart;
    }
}
