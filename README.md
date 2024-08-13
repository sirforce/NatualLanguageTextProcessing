
# Natural Language Text Query Processing Proof of Concept

## Overview

This project is a proof of concept for processing natural language text queries. The system validates and processes queries that include Boolean operators (AND, OR, ANDNOT), parentheses, quoted strings, and field-value pairs in the format `field:value`. The primary goal is to validate the structure of the queries and generate a human-readable representation of the query's logical structure.

## Features

- **Validation**: Ensures that queries have balanced parentheses and quotes, that Boolean operators are used correctly, and that field names are valid.
- **Field-Value Pair Handling**: Supports field-value pairs in the format `field:value`, where the value can be either a simple word or a quoted string.
- **Textual Representation**: Generates a clean, formatted representation of the query, showing the logical structure with appropriate indentation.
- **Handling of Quoted Strings**: Treats content within quotes as literal, preserving spaces and special characters.

## Project Structure

- **Main Program**: The entry point of the project that runs the validation and generates the textual representation of the query.
- **QueryValidator Class**: Handles the validation and processing of the query.
  - **ValidateQuery**: Main method to validate the query.
  - **AreParenthesesBalanced**: Checks for balanced parentheses.
  - **AreQuotesBalanced**: Checks for balanced quotes.
  - **ContainsValidOperatorsOutsideQuotes**: Validates Boolean operators outside of quoted strings.
  - **ValidateFieldsAndValues**: Ensures field names are valid and values are properly handled.
  - **GenerateTextualRepresentation**: Generates a human-readable, indented representation of the query.
- **QueryValidatorResult Class**: Holds the result of the query validation, including whether the query is valid and the generated textual representation.

## Example

### Input Query

```plaintext
(COMPANY:BrandA or COMPANY:"Some Company") AND ("BUSINESS AND OPERATIONS" OR "STRATEGY CONSULTANT") AND ("BACHELORS DEGREE" OR "B.S." OR "BBA" OR "MBA")
```

### Validation Process

1. **Balanced Parentheses**: The system checks that all opening parentheses have matching closing parentheses.
2. **Balanced Quotes**: Ensures that all opening quotes have matching closing quotes.
3. **Field-Value Pair Validation**: Confirms that the field names are valid and that field-value pairs are correctly structured.
4. **Boolean Operator Validation**: Confirms that Boolean operators are correctly used and surrounded by valid expressions.

### Output

If the query is valid, the system produces a textual representation like this:

```plaintext
Start Group:
  COMPANY: BrandA
  OR
  COMPANY: "Some Company"
End Group
AND
Start Group:
  "BUSINESS AND OPERATIONS"
  OR
  "STRATEGY CONSULTANT"
End Group
AND
Start Group:
  "BACHELORS DEGREE"
  OR
  "B.S."
  OR
  "BBA"
  OR
  "MBA"
End Group
```

This output clearly shows the logical structure of the query, with proper indentation for nested groups and quoted strings.

## Usage

To use the system, simply input a natural language text query into the program. The program will then validate the query and generate the processed text. The output will indicate whether the query is valid and, if so, display the formatted representation.

### Running the Program

1. Ensure you have a C# development environment set up.
2. Compile and run the program.
3. Input your query as a string.
4. View the validation results and the generated textual representation.

### Example Code

Here’s a basic example of how to use the `QueryValidator` class:

```csharp
string query = "(COMPANY:BrandA or COMPANY:"Some Company") AND ("BUSINESS AND OPERATIONS" OR "STRATEGY CONSULTANT") AND ("BACHELORS DEGREE" OR "B.S." OR "BBA" OR "MBA")";
List<string> validFields = new List<string> { "COMPANY", "POSITION", "DEGREE", "LOCATION" };
var validationResult = QueryValidator.ValidateQuery(query, validFields);

Console.WriteLine($"Is Query Valid: {validationResult.IsValid}");
Console.WriteLine($"Error Message: {validationResult.ErrorMessage}");
if (validationResult.IsValid)
{
    Console.WriteLine("Processed Text:");
    Console.WriteLine(validationResult.ProcessedText);
}
```

## Contributing

If you wish to contribute to this project, please follow these steps:

1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch`).
3. Make your changes.
4. Commit your changes (`git commit -am 'Add new feature'`).
5. Push to the branch (`git push origin feature-branch`).
6. Create a new pull request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact

If you have any questions or issues, feel free to open an issue or contact the project maintainer.
