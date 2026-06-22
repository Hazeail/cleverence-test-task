/*
  ФАЙЛ: StringCompressionService.cs
  НАЗНАЧЕНИЕ: Реализует сервис сжатия и декомпрессии строк для задачи 1.

  ЖУРНАЛ ИЗМЕНЕНИЙ:
  22.06.2026 - Первичное создание файла.
  22.06.2026 - Добавлен архитектурный каркас сервиса сжатия строк для задачи 1.
  22.06.2026 - Реализованы алгоритмы сжатия и декомпрессии строк для задачи 1.
  22.06.2026 - Перенесен в структуру Task1 для явного разделения задач в репозитории.
*/

using System.Text;

namespace Cleverence.TestTask.Core.Task1;

/// <summary>
/// Реализует операции сжатия и декомпрессии строк в согласованном прикладном формате.
/// </summary>
public sealed class StringCompressionService : IStringCompressionService
{
    /// <summary>
    /// Сжимает исходную строку по правилам задачи.
    /// </summary>
    /// <param name="input">Исходная строка.</param>
    /// <returns>Сжатое строковое представление.</returns>
    /// <remarks>Одиночный символ записывается без счетчика, а группа повторений сохраняется как символ и количество.</remarks>
    public string Compress(string input)
    {
        StringCompressionValidator.ValidateSource(input);

        if (input.Length == 0)
        {
            // Пустую строку возвращаем как есть, чтобы не вводить специальный служебный формат для отсутствия данных.
            return string.Empty;
        }

        StringBuilder builder = new(input.Length);
        char currentSymbol = input[0];
        int currentCount = 1;

        for (int index = 1; index < input.Length; index++)
        {
            char nextSymbol = input[index];

            if (nextSymbol == currentSymbol)
            {
                currentCount++;
                continue;
            }

            // На границе группы записываем накопленную серию до перехода к следующему символу.
            AppendGroup(builder, currentSymbol, currentCount);
            currentSymbol = nextSymbol;
            currentCount = 1;
        }

        // После завершения цикла обязательно дописываем последнюю накопленную группу.
        AppendGroup(builder, currentSymbol, currentCount);
        return builder.ToString();
    }

    /// <summary>
    /// Восстанавливает исходную строку из сжатого представления.
    /// </summary>
    /// <param name="input">Сжатая строка.</param>
    /// <returns>Восстановленная строка.</returns>
    /// <remarks>Строка считается корректной, если после буквы указан либо отсутствующий счетчик, либо целое число больше единицы.</remarks>
    public string Decompress(string input)
    {
        StringCompressionValidator.ValidateCompressed(input);

        if (input.Length == 0)
        {
            // Пустой ввод поддерживаем симметрично с методом сжатия.
            return string.Empty;
        }

        StringBuilder builder = new(input.Length);

        for (int index = 0; index < input.Length; index++)
        {
            char symbol = input[index];

            if (!StringCompressionValidator.IsLowerLatinLetter(symbol))
            {
                // Если сегмент не начинается с буквы, разбор становится неоднозначным, поэтому строку считаем невалидной.
                throw new FormatException("Сжатая строка содержит недопустимую последовательность символов.");
            }

            int groupCount = ReadGroupCount(input, ref index);
            builder.Append(symbol, groupCount);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Добавляет в буфер одну группу символов в формате текущего сжатия.
    /// </summary>
    /// <param name="builder">Буфер результата.</param>
    /// <param name="symbol">Символ группы.</param>
    /// <param name="count">Количество повторений.</param>
    /// <remarks>Метод используется как единая точка записи групп при реализации сжатия.</remarks>
    private static void AppendGroup(StringBuilder builder, char symbol, int count)
    {
        ArgumentNullException.ThrowIfNull(builder);

        if (count <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Количество повторений должно быть положительным.");
        }

        builder.Append(symbol);

        if (count > 1)
        {
            // Счетчик одиночного символа не записываем, чтобы соблюсти формат задачи и не порождать записи вида a1.
            builder.Append(count);
        }
    }

    /// <summary>
    /// Читает количество повторений группы из сжатой строки.
    /// </summary>
    /// <param name="input">Сжатая строка.</param>
    /// <param name="index">Текущая позиция чтения, которая будет продвинута по мере разбора.</param>
    /// <returns>Количество повторений для текущего символа.</returns>
    /// <remarks>Метод локализует разбор числовой части группы и отбрасывает неоднозначные формы записи.</remarks>
    private static int ReadGroupCount(string input, ref int index)
    {
        int countStartIndex = index + 1;

        if (countStartIndex >= input.Length || !char.IsDigit(input[countStartIndex]))
        {
            // Отсутствие счетчика означает одиночный символ, что является штатной формой хранения.
            return 1;
        }

        if (input[countStartIndex] == '0')
        {
            // Запрещаем ведущий ноль, чтобы не принимать двусмысленные формы вроде a0 и a02.
            throw new FormatException("Сжатая строка содержит недопустимый счетчик повторений.");
        }

        long parsedCount = 0;
        int scanIndex = countStartIndex;

        while (scanIndex < input.Length && char.IsDigit(input[scanIndex]))
        {
            parsedCount = (parsedCount * 10) + (input[scanIndex] - '0');

            if (parsedCount > int.MaxValue)
            {
                // Защищаем декомпрессию от переполнения счетчика еще на этапе разбора чисел.
                throw new FormatException("Сжатая строка содержит слишком большое количество повторений.");
            }

            scanIndex++;
        }

        if (parsedCount == 1)
        {
            // Не принимаем форму a1, потому что по контракту одиночный символ должен храниться без числа.
            throw new FormatException("Сжатая строка содержит недопустимую запись одиночного символа.");
        }

        index = scanIndex - 1;
        return (int)parsedCount;
    }
}
