using NaturalLanguageTextQueryProcessing;

Console.WriteLine("Hello, World Natural Language Text Query Processing Proof of Concept!");

//string query = "\"A AND (B)\" ANDNOT f or X (B OR \"C AND D\")";
//string query ="\"microsoft AND (sql)\" AND (software OR \"hardware\")";
//string query = "\"A ANDNOT (B)\" AND (B OR \"C AND D\")";
string query =
    "(COMPANY:BrandA or COMPANY:\"Some Company\") AND (\"BUSINESS AND OPERATIONS\" OR (\"STRATEGY CONSULTANT\" AND \"Technology\")) AND (\"BACHELORS DEGREE\" OR \"B.S.\" OR \"BBA\" OR \"MBA\") OR TITLE:Developer";
Console.WriteLine($"Query as entered: {query}");

var fieldsToValidate = new List<string>()
{
    "COMPANY", "TITLE"
};

var validationResult = QueryValidator.ValidateQuery(query, fieldsToValidate);
Console.WriteLine($"Is Query Valid: {validationResult.IsValid}");
Console.WriteLine($"Error Message: {validationResult.ErrorMessage}");
if (validationResult.IsValid)
{
    Console.WriteLine("Processed Text:");
    Console.WriteLine(validationResult.ProcessedText);
}


