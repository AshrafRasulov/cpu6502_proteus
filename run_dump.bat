@echo off
:: Устанавливаем кодировку UTF-8 для консоли
chcp 65001 > nul

echo 🚀 Запуск сборщика проекта cpu6502_proteus...
echo.

:: Запускаем PowerShell и передаем ему команду запуска скрипта из папки run
powershell -NoProfile -ExecutionPolicy Bypass -Command "& '.\run\gather.ps1'"

:: Проверяем, успешно ли отработал скрипт
if %errorlevel% neq 0 (
    echo.
    echo ❌ Произошла ошибка при выполнении.
) else (
    echo.
    echo ✅ Готово! Файл создан: full_project_code.txt
)
pause