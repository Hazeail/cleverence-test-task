/*
  ФАЙЛ: StringCompressionValidator.cs
  НАЗНАЧЕНИЕ: Содержит проверки входных данных для сервиса сжатия и декомпрессии строк.

  ЖУРНАЛ ИЗМЕНЕНИЙ:
  22.06.2026 - Первичное создание файла.
  22.06.2026 - Добавлены проверки предусловий для архитектурного каркаса задачи 1.
  22.06.2026 - Добавлена проверка допустимых символов исходной строки для рабочей реализации задачи 1.
  22.06.2026 - Перенесен в структуру Task1 для явного разделения задач в репозитории.
*/

namespace Cleverence.TestTask.Core.Task1;

/// <summary>
/// Выполняет базовую валидацию строк для операций сжатия и декомпрессии.
/// </summary>
internal static class StringCompressionValidator
{
    /// <summary>
    /// Проверяет корректность входной строки перед сжатием.
    /// </summary>
    /// <param name="input">Исходная строка.</param>
    /// <remarks>На текущем этапе валидатор проверяет только обязательные предусловия контракта.</remarks>
    internal static void ValidateSource(string input)
    {
        // Защитная проверка нужна до запуска алгоритма, чтобы сервис работал по явному контракту.
        ArgumentNullException.ThrowIfNull(input);

        for (int index = 0; index < input.Length; index++)
        {
            // Для исходной строки разрешаем только маленькие латинские буквы, иначе формат сжатия теряет однозначность.
            if (!IsLowerLatinLetter(input[index]))
            {
                throw new ArgumentException(
                    "Исходная строка должна содержать только маленькие буквы латинского алфавита.",
                    nameof(input));
            }
        }
    }

    /// <summary>
    /// Проверяет корректность входной строки перед декомпрессией.
    /// </summary>
    /// <param name="input">Сжатая строка.</param>
    /// <remarks>Детальная проверка формата будет расширена вместе с реализацией алгоритма декомпрессии.</remarks>
    internal static void ValidateCompressed(string input)
    {
        // Держим отдельную точку проверки, чтобы правила формата не размазывались по сервису.
        ArgumentNullException.ThrowIfNull(input);
    }

    /// <summary>
    /// Проверяет, что символ входит в допустимый диапазон маленьких латинских букв.
    /// </summary>
    /// <param name="symbol">Проверяемый символ.</param>
    /// <returns>Истина, если символ допустим для формата задачи.</returns>
    /// <remarks>Метод используется и при сжатии, и при разборе сжатого представления.</remarks>
    internal static bool IsLowerLatinLetter(char symbol)
    {
        return symbol >= 'a' && symbol <= 'z';
    }
}
