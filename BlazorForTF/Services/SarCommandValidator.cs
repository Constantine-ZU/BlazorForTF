using System;
using System.Text.RegularExpressions;

public class SarCommandValidator
{
    // Регулярное выражение для проверки наличия символа `-` и двух чисел
    private static readonly Regex FlagAndNumberPattern = new Regex(@"-[a-zA-Z]+\s+\d+(\s+\d+)?");

    // Метод для проверки общей длины и начала с `sar`
    private bool ValidatePrefixAndLength(string command)
    {
        return command.StartsWith("sar") && command.Length <= 13;
    }

    // Метод для проверки наличия символа `-` и двух чисел
    private bool ValidateFlagsAndNumbers(string command)
    {
        return FlagAndNumberPattern.IsMatch(command);
    }

    // Общий метод, который проверяет оба условия
    public bool Validate(string command)
    {
        bool isValidPrefixAndLength = ValidatePrefixAndLength(command);
        bool isValidFlagsAndNumbers = ValidateFlagsAndNumbers(command);

       // Console.WriteLine($@"command: {command} | Prefix and Length: {isValidPrefixAndLength}, Flags and Numbers: {isValidFlagsAndNumbers}");

        // Команда валидна только если оба условия выполнены
        return isValidPrefixAndLength && isValidFlagsAndNumbers;
    }
}
