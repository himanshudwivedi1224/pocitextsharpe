using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Diagnostics;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        string pdfFilePath = "C:\\Temp\\bin\\Adityabirlafashionretail_AR_FY23.pdf";
        string outputPath = "C:\\Temp\\bin\\output.txt";

        Stopwatch stopwatch = Stopwatch.StartNew();

        // Extract text from PDF and save to file
        ExtractAndSaveText(pdfFilePath, outputPath);

        stopwatch.Stop();

        Console.WriteLine("Time taken: " + stopwatch.Elapsed);
        Console.WriteLine("Text extracted from PDF and saved to: " + outputPath);
    }
    static void ExtractAndSaveText(string pdfFilePath, string outputPath)
    {
        // Create a StringBuilder to store the extracted text
        StringBuilder extractedText = new StringBuilder();

        try
        {
            // Use a PdfReader object to read the PDF file
            using (PdfReader reader = new PdfReader(pdfFilePath))
            {
                // Process each page in parallel
                Parallel.For(1, reader.NumberOfPages + 1, page =>
                {
                    // Extract text from the page
                    string pageText = PdfTextExtractor.GetTextFromPage(reader, page);

                    // Check if the page should be skipped based on its content
                    if (!ShouldSkipPage(pageText))
                    {
                        lock (extractedText)
                        {
                            // Append the text to the StringBuilder if the page should not be skipped
                            extractedText.Append(pageText);
                        }
                    }
                });
            }

            // Save the extracted text to a text file
            File.WriteAllText(outputPath, extractedText.ToString());

            Console.WriteLine("Text extracted from PDF and saved to: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while extracting text: " + ex.Message);
        }
    }


    static bool ShouldSkipPage(string pageText)
    {
        // Implement your logic here to determine whether the page should be skipped based on its content
        // For example, you could use Regex for efficient pattern matching
        return pageText.Contains("Page to Skip");
    }
}
