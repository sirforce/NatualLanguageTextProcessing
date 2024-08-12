namespace NaturalLanguageTextQueryProcessing;

public class QueryValidatorResult
{
    public bool IsValid { get; set; }
    public string ErrorMessage { get; set; }
    public string ProcessedText { get; set; } // New property for textual read-back
}