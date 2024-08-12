using NaturalLanguageTextQueryProcessing;

Console.WriteLine("Hello, World Natural Language Text Query Processing Proof of Concept!");

//string query = "\"A AND (B)\" ANDNOT f or X (B OR \"C AND D\")";
string query ="\"microsoft AND (sql)\" AND (software OR \"hardware\")";
//string query = "\"A ANDNOT (B)\" AND (B OR \"C AND D\")";
Console.WriteLine($"Query as entered: {query}");

var validationResult = QueryValidator.ValidateQuery(query);
Console.WriteLine($"Is Query Valid: {validationResult.IsValid}");
Console.WriteLine($"Error Message: {validationResult.ErrorMessage}");
if (validationResult.IsValid)
{
    Console.WriteLine("Processed Text:");
    Console.WriteLine(validationResult.ProcessedText);
}


