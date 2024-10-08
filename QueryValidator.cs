using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using NaturalLanguageTextQueryProcessing;


public class QueryValidator
{
    public static QueryValidatorResult ValidateQuery(string query, List<string> validFields,
        bool ConsoleLogOutputOn = false)
    {
        var result = new QueryValidatorResult
            { IsValid = true, ErrorMessage = string.Empty, ProcessedText = string.Empty };

        if (ConsoleLogOutputOn) Console.WriteLine("Starting validation...");

        // Check for balanced parentheses
        if (!AreParenthesesBalanced(query))
        {
            result.IsValid = false;
            result.ErrorMessage = "Unbalanced parentheses.";
            return result;
        }

        // Check for balanced quotes
        if (!AreQuotesBalanced(query))
        {
            result.IsValid = false;
            result.ErrorMessage = "Unbalanced quotes.";
            return result;
        }

        // Check for valid fields and values
        if (!ValidateFieldsAndValues(query, validFields))
        {
            result.IsValid = false;
            result.ErrorMessage = "Invalid field or value syntax.";
            return result;
        }

        // Check for valid Boolean operators outside quotes
        if (!ContainsValidOperatorsOutsideQuotes(query))
        {
            result.IsValid = false;
            result.ErrorMessage = "Invalid use of Boolean operators.";
            return result;
        }

        // If everything is valid, generate the processed text
        result.ProcessedText = GenerateTextualRepresentation(query);

        return result;
    }

    private static bool AreParenthesesBalanced(string query, bool ConsoleLogOutputOn = false)
    {
        int balance = 0;
        bool insideQuotes = false;

        foreach (char ch in query)
        {
            if (ch == '"')
            {
                insideQuotes = !insideQuotes;
            }
            else if (!insideQuotes && ch == '(')
            {
                balance++;
            }
            else if (!insideQuotes && ch == ')')
            {
                balance--;
            }

            // If balance goes negative, parentheses are not balanced
            if (balance < 0)
            {
                if (ConsoleLogOutputOn) Console.WriteLine("Unbalanced parentheses detected.");
                return false;
            }
        }

        // Balance should be zero for properly balanced parentheses
        bool balanced = balance == 0;
        if (ConsoleLogOutputOn) Console.WriteLine($"Parentheses balanced: {balanced}");
        return balanced;
    }

    private static bool AreQuotesBalanced(string query, bool ConsoleLogOutputOn = false)
    {
        bool isInsideQuote = false;

        foreach (char ch in query)
        {
            if (ch == '"')
            {
                isInsideQuote = !isInsideQuote;
            }
        }

        // If isInsideQuote is true, it means there's an unmatched quote
        bool balanced = !isInsideQuote;
        if (ConsoleLogOutputOn) Console.WriteLine($"Quotes balanced: {balanced}");
        return balanced;
    }

    private static bool ValidateFieldsAndValues(string query, List<string> validFields, bool ConsoleLogOutputOn = true)
    {
        // Regular expression to match field:value pairs with possible quoted values
        var regex = new Regex(@"\b(\w+):(""[^""]*""|\b\w+\b)", RegexOptions.IgnoreCase);
        var matches = regex.Matches(query);

        foreach (Match match in matches)
        {
            string field = match.Groups[1].Value;
            string value = match.Groups[2].Value;

            Console.WriteLine($"Validating field: {field} with value: {value}");

            // Validate the field name
            if (!validFields.Contains(field))
            {
                Console.WriteLine($"Invalid field: {field}");
                return false;
            }

            // Validate the value if it's quoted
            if (value.StartsWith("\"") && !value.EndsWith("\""))
            {
                Console.WriteLine($"Unterminated quoted value: {value}");
                return false;
            }
        }

        return true;
    }

    private static bool ContainsValidOperatorsOutsideQuotes(string query, bool ConsoleLogOutputOn = false)
    {
        bool insideQuotes = false;
        string[] operators = { "AND", "OR", "ANDNOT" };

        for (int i = 0; i < query.Length; i++)
        {
            if (query[i] == '"')
            {
                insideQuotes = !insideQuotes;
            }

            if (!insideQuotes)
            {
                foreach (var op in operators)
                {
                    if (i + op.Length <= query.Length &&
                        query.Substring(i, op.Length).Equals(op, StringComparison.OrdinalIgnoreCase))
                    {
                        if (ConsoleLogOutputOn) Console.WriteLine($"Operator '{op}' found at position {i}");

                        if (!IsOperatorValid(query, i, op.Length))
                        {
                            if (ConsoleLogOutputOn) Console.WriteLine($"Operator '{op}' at position {i} is invalid.");
                            return false;
                        }

                        if (ConsoleLogOutputOn) Console.WriteLine($"Operator '{op}' at position {i} is valid.");
                        i += op.Length - 1;
                    }
                }
            }
        }

        return true;
    }

    private static bool IsOperatorValid(string query, int operatorIndex, int operatorLength,
        bool ConsoleLogOutputOn = false)
    {
        int beforeIndex = operatorIndex - 1;
        int afterIndex = operatorIndex + operatorLength;

        if (ConsoleLogOutputOn) Console.WriteLine($"Checking operator validity at index {operatorIndex}");

        // Skip spaces before the operator
        while (beforeIndex >= 0 && char.IsWhiteSpace(query[beforeIndex]))
        {
            beforeIndex--;
        }

        // Skip spaces after the operator
        while (afterIndex < query.Length && char.IsWhiteSpace(query[afterIndex]))
        {
            afterIndex++;
        }

        // Check the validity of the character before the operator
        bool isBeforeValid = beforeIndex >= 0 && IsValidBeforeChar(query[beforeIndex]);

        // Check the validity of the character after the operator
        bool isAfterValid = afterIndex < query.Length && IsValidAfterChar(query[afterIndex]);

        if (ConsoleLogOutputOn) Console.WriteLine($"Is content before operator valid: {isBeforeValid}");
        if (ConsoleLogOutputOn) Console.WriteLine($"Is content after operator valid: {isAfterValid}");

        return isBeforeValid && isAfterValid;
    }

    private static bool IsValidBeforeChar(char ch, bool ConsoleLogOutputOn = false)
    {
        bool isValid = char.IsLetterOrDigit(ch) || ch == ')' || ch == '"';
        if (ConsoleLogOutputOn) Console.WriteLine($"Character before operator '{ch}' is valid: {isValid}");
        return isValid;
    }

    private static bool IsValidAfterChar(char ch, bool ConsoleLogOutputOn = false)
    {
        bool isValid = char.IsLetterOrDigit(ch) || ch == '(' || ch == '"';
        if (ConsoleLogOutputOn) Console.WriteLine($"Character after operator '{ch}' is valid: {isValid}");
        return isValid;
    }

    private static string GenerateTextualRepresentation(string query)
{
    StringBuilder textualRepresentation = new StringBuilder();
    StringBuilder temp = new StringBuilder(); // Temp storage for accumulating characters outside quotes
    int level = 0;
    bool insideQuotes = false;
    bool insideFieldValue = false;

    for (int i = 0; i < query.Length; i++)
    {
        char ch = query[i];

        if (ch == '"')
        {
            insideQuotes = !insideQuotes;

            if (insideQuotes)
            {
                textualRepresentation.Append(" "); // Add space before opening quote
                textualRepresentation.Append(new string(' ', level * 2) + "\""); // Opening quote
            }
            else
            {
                textualRepresentation.Append(temp.ToString()); // Append accumulated characters
                temp.Clear(); // Clear temp storage
                textualRepresentation.AppendLine("\""); // Closing quote
                insideFieldValue = false; // Reset field-value flag
            }
        }
        else if (ch == '(' && !insideQuotes)
        {
            if (temp.Length > 0)
            {
                textualRepresentation.AppendLine(temp.ToString().Trim()); // Append content before Start Group
                temp.Clear();
            }
            textualRepresentation.AppendLine(new string(' ', level * 2) + "Start Group:"); // Append Start Group before incrementing the level
            level++;
        }
        else if (ch == ')' && !insideQuotes)
        {
            level--;
            if (temp.Length > 0)
            {
                textualRepresentation.AppendLine(temp.ToString().Trim()); // Append content before End Group
                temp.Clear();
            }
            textualRepresentation.AppendLine(new string(' ', level * 2) + "End Group");
        }
        else if (ch == ':' && !insideQuotes)
        {
            // Handle field:value pairs
            insideFieldValue = true;
            textualRepresentation.Append(new string(' ', level * 2) + temp.ToString().Trim() + ":"); // Append the field name with colon
            temp.Clear();
        }
        else if (!char.IsWhiteSpace(ch) || insideQuotes) // Preserve spaces inside quotes
        {
            temp.Append(ch); // Accumulate characters
        }
        else if (temp.Length > 0)
        {
            textualRepresentation.AppendLine(new string(' ', level * 2) + temp.ToString().Trim());
            temp.Clear();
        }
    }

    if (temp.Length > 0) // Append any remaining characters outside quotes
    {
        textualRepresentation.Append(temp.ToString());
    }

        return textualRepresentation.ToString();
    }
    
}