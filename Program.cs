using Labb2_Bildtjanster_I_Azure_AI.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Labb2_Bildtjanster_I_Azure_AI
{
    internal class Program
    {
        // Define the Computer Vision client and the exit variable
        private static ComputerVisionClient cvClient;
        static private bool exitProgram;

        // Local image
        // C:\Users\crill\Pictures\272814710_10166276808540472_9054784407626144230_n.jpg

        // Url image
        // https://wholesgame.com/wp-content/uploads/ludzie_0260.jpg

        static async Task Main(string[] args)
        {
            // Get configuration settings from appsettings
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfigurationRoot configuration = builder.Build();
            string cogSvcEndpoint = configuration["CognitiveServicesEndpoint"];
            string cogSvcKey = configuration["CognitiveServiceKey"];

            // Authenticate the Computer Vision client
            ApiKeyServiceClientCredentials credentials = new ApiKeyServiceClientCredentials(cogSvcKey);
            cvClient = new ComputerVisionClient(credentials)
            {
                Endpoint = cogSvcEndpoint
            };

            string input = "";

            do
            {
                // Display the input menu and wait for user's choice
                InputMenu(input);
            }
            // Exits the program when true
            while (exitProgram == false);
        }

        // Display the main menu
        static void InputMenu(string input)
        {
            string prompt = "Enter a local path or URL to an image.";
            string[] options =
            {
                "╟── Via local path",
                "╟── Via URL",
                "╟── Exit"
            };

            Menu mainMenu = new Menu();
            mainMenu.SetIndex(prompt, options);
            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    LocalFileInput(input); // Input a local file path
                    break;
                case 1:
                    UrlInput(input); // Input a URL
                    break;
                case 2:
                    exitProgram = true; // Exit the program
                    break;
            }
        }

        // Handle input for a local file path
        static void LocalFileInput(string input)
        {
            Console.Clear();
            string prompt = $"Enter the file path to the image to analyze";
            Console.WriteLine("╔═════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                          Image Analyzer                         ║");
            Console.WriteLine("╠═════════════════════════════════════════════════════════════════╣");
            Console.WriteLine($"║ {prompt,-63} ║");
            Console.WriteLine("║                                                                 ║");
            Console.WriteLine("║ Input:                                                          ║");
            Console.WriteLine("╚═════════════════════════════════════════════════════════════════╝");
            Console.SetCursorPosition(9, 5);
            input = Console.ReadLine()?.Trim();

            // Sends the user's input to image analyze
            AnalyzeImageAsync(input).Wait();

            Console.ReadKey();
        }

        // Handle input for a URL
        static void UrlInput(string input)
        {
            Console.Clear();
            string prompt = $"Enter the URL to the image to analyze";
            Console.WriteLine("╔═════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                          Image Analyzer                         ║");
            Console.WriteLine("╠═════════════════════════════════════════════════════════════════╣");
            Console.WriteLine($"║ {prompt,-63} ║");
            Console.WriteLine("║                                                                 ║");
            Console.WriteLine("║ Input:                                                          ║");
            Console.WriteLine("╚═════════════════════════════════════════════════════════════════╝");
            Console.SetCursorPosition(9, 5);
            input = Console.ReadLine()?.Trim();

            // Sends the user's input to image analyze
            AnalyzeImageAsync(input).Wait();

            Console.ReadKey();
        }

        // Analyze the image using the Computer Vision Service
        static async Task AnalyzeImageAsync(string input)
        {
            Console.Clear();
            string prompt = $"Please wait while the image is analyzed";
            Console.WriteLine("╔═════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                          Image Analyzer                         ║");
            Console.WriteLine("╠═════════════════════════════════════════════════════════════════╣");
            Console.WriteLine($"║ {prompt,-63} ║");
            Console.WriteLine("║                                                                 ║");

            try
            {
                // Checks if the input can be parsed as an absolute URI
                if (Uri.TryCreate(input, UriKind.Absolute, out var uri))
                {
                    // Declaring a variable to store the description result
                    ImageDescription? imageDescription;

                    Console.WriteLine("\nPlease Wait, Analyzing image..");

                    // Checks if the URI is a local file
                    if (uri.IsFile)
                        // Analyzing the image from a local file
                        imageDescription = await cvClient.DescribeImageInStreamAsync(File.OpenRead(uri.LocalPath));

                    // If the URI is a URL
                    else
                        // Analyzing the image from a URL
                        imageDescription = await cvClient.DescribeImageAsync(uri.AbsoluteUri);

                    // If the analysis result is not null
                    if (imageDescription is not null)
                    {
                        // Gets the confidence score
                        var confidence = $"{Math.Round(imageDescription.Captions[0].Confidence * 100)}%";
                        // Gets the description of the image
                        var description = imageDescription.Captions[0].Text;

                        Console.WriteLine($"I'm {confidence} certain that this is an image of {description}!\n");
                    }
                }
                else
                {
                    throw new Exception("Bad input");
                }
            }
            catch
            {
                Console.WriteLine("\nSomething went wrong!");
            }
            Console.WriteLine("╚═════════════════════════════════════════════════════════════════╝");
        }
    }
}
