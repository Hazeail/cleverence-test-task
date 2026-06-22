# cleverence-test-task

Репозиторий содержит выполнение тестового задания для вакансии `C# developer (офис)` в компании «Клеверенс Софт».

## Структура репозитория

- `src/Cleverence.TestTask.Core/Task1` - реализация задачи 1.
- `src/Cleverence.TestTask.Core/Task2` - реализация задачи 2.
- `src/Cleverence.TestTask.Core/Task3` - реализация задачи 3.
- `src/Cleverence.TestTask.Console` - консольная точка входа для демонстрации и ручной проверки.
- `tests/Cleverence.TestTask.Tests/Task1` - автоматические тесты задачи 1.
- `tests/Cleverence.TestTask.Tests/Task2` - автоматические тесты задачи 2.
- `tests/Cleverence.TestTask.Tests/Task3` - автоматические тесты задачи 3.
- `docs/Task1.md` - описание решения и проверки задачи 1.
- `docs/Task2.md` - описание решения и проверки задачи 2.
- `docs/Task3.md` - описание решения, допущений и проверки задачи 3.

## Как проверять

1. Открыть решение `Cleverence.TestTask.sln`.
2. Запустить автоматические тесты:
   `dotnet test tests/Cleverence.TestTask.Tests/Cleverence.TestTask.Tests.csproj --no-restore`
3. При необходимости выполнить ручные сценарии через `Console`:
   - `task1-compress <строка>`
   - `task1-decompress <строка>`
   - `task2-sequence <value1> <value2> ...`
   - `task2-parallel <число_писателей> <число_повторов>`
   - `task3 <inputFilePath> <outputFilePath> <problemsFilePath>`

## Текущий статус

1. Задача 1 реализована и покрыта автоматическими тестами.
2. Задача 2 реализована и покрыта автоматическими тестами.
3. Задача 3 реализована и покрыта автоматическими тестами.
