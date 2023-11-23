using System;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using ClassLibraryLab4;

class Program
{
    static int Main(string[] args)
    {
        var app = new CommandLineApplication();

        app.HelpOption();

        app.Command("version", command => {
            command.Description = "Display the information about the program";

            command.OnExecute(() => {
                Console.WriteLine("LAB4");
                Console.WriteLine("Author: Stanislav Hanchev (Group 41/2)");
                Console.WriteLine("Version: 1.0.0");
                Console.WriteLine("Static variant of tasks: 69");
            });
        });

        app.Command("set-path", command => {
            command.Description = "Set the path to the folder with input and output files";
            var pathOption = command.Option("-p | --path <path>", "The path to the folder with input and output files", CommandOptionType.SingleValue);
            pathOption.IsRequired();

            command.OnExecute(() => {
                var pathValue = pathOption.Value();
                Console.WriteLine($"{pathValue}");
                System.Environment.SetEnvironmentVariable("LAB_PATH", pathValue, EnvironmentVariableTarget.User);
                Console.WriteLine($"LAB_PATH is set to {pathValue}");
            });
        });

        app.Command("run", command =>
        {
            command.Description = "Perform lab 1-3";
            var labArgument = command.Argument("lab", "The lab to run (lab1, lab2, lab3)");
            var inputOptionPath = command.Option("-i | --input <path>", "The input file path", CommandOptionType.SingleValue);
            var outputOptionPath = command.Option("-o | --output <path>", "The output file path", CommandOptionType.SingleValue);

            command.OnExecute(() => {
                var inputOption = inputOptionPath.Value();
                var outputOption = outputOptionPath.Value();

                if (string.IsNullOrEmpty(inputOption))
                {
                    inputOption = Environment.GetEnvironmentVariable("LAB_PATH", EnvironmentVariableTarget.User);
                    if (string.IsNullOrEmpty(inputOption))
                    {
                        inputOption = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                        if (!string.IsNullOrEmpty(inputOption))
                        {
                            inputOption = Path.Combine(inputOption, "INPUT.txt");
                        }
                    }
                    else
                    {
                        inputOption = Path.Combine(inputOption, "INPUT.txt");
                    }
                }

                if (string.IsNullOrEmpty(outputOption))
                {
                    outputOption = Environment.GetEnvironmentVariable("LAB_PATH", EnvironmentVariableTarget.User);

                    if (string.IsNullOrEmpty(outputOption))
                    {
                        outputOption = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                        if (!string.IsNullOrEmpty(outputOption))
                        {
                            outputOption = Path.Combine(outputOption, "OUTPUT.txt");
                        }
                    }
                    else
                    {
                        outputOption = Path.Combine(outputOption, "OUTPUT.txt");
                    }
                }

                if (File.Exists(inputOption))
                {
                    string labValue = labArgument.Value;

                    if (string.IsNullOrEmpty(labValue) || (labValue != "lab1" && labValue != "lab2" && labValue != "lab3"))
                    {
                        Console.WriteLine("Invalid lab value. Use lab1, lab2, or lab3.");
                    }
                    else
                    {
                        switch (labValue) 
                        {
                            case "lab1":
                                ClassLab1.Execute(inputOption, outputOption);
                                break;

                            case "lab2":
                                ClassLab2.Execute(inputOption, outputOption);
                                break;

                            case "lab3":
                                ClassLab3.Execute(inputOption, outputOption);
                                break;

                            default:
                                Console.WriteLine("Unknown argument: " + labValue);
                                break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Can't find INPUT.txt file by path {inputOption}\n\n");
                    command.ShowHelp();
                }
            });
        });

        app.OnExecute(() => app.ShowHelp());

        return app.Execute(args);
    }
}
