using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class CLIHelper
    {
        /// <summary>
        /// Gets a user input integer value for the given message prompt.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static int GetInteger(string message)
        {
            string userInput = String.Empty;
            int intValue = 0;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again.");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (!int.TryParse(userInput, out intValue));

            return intValue;
        }

        /// <summary>
        /// Gets a user input integer value in the specified range (inclusive).
        /// </summary>
        /// <param name="prompt">Prompt message for the user input.</param>
        /// <param name="lowerBound">The lower bound (inclusive) for the allowable range.</param>
        /// <param name="upperBound">The upper bound (inclusive) for the allowable range.</param>
        /// <param name="rangeErrorMessage">Message to display for out of range attempts.</param>
        /// <returns></returns>
        public static int GetIntegerInRange(string prompt, int lowerBound, int upperBound,
            string rangeErrorMessage = "Invalid option. Please choose a number from the list.")
        {
            int intValue = lowerBound - 1;
            int numberOfAttempts = 0;

            while (intValue < lowerBound || intValue > upperBound)
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine(rangeErrorMessage);
                }

                intValue = GetInteger(prompt);
                numberOfAttempts++;
            }

            return intValue;
        }

        /// <summary>
        /// Gets a user input integer value from the provided allowable list.
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="allowableNumbers"></param>
        /// <param name="rangeErrorMessage"></param>
        /// <returns></returns>
        public static int GetIntegerInRange(string prompt, List<int> allowableNumbers,
            string rangeErrorMessage = "Invalid option. Please choose a number from the list.")
        {
            int intValue = allowableNumbers.Min() - 1;
            int numberOfAttempts = 0;

            while (!allowableNumbers.Contains(intValue))
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine(rangeErrorMessage);
                }

                intValue = GetInteger(prompt);
                numberOfAttempts++;
            }

            return intValue;
        }

        /// <summary>
        /// Gets a user input double value for the given message prompt.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static double GetDouble(string message)
        {
            string userInput = String.Empty;
            double doubleValue = 0.0;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again.");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (!double.TryParse(userInput, out doubleValue));

            return doubleValue;
        }

        /// <summary>
        /// Gets a user input boolean value for the input message prompt.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool GetBool(string message)
        {
            string userInput = String.Empty;
            bool boolValue = false;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again.");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (!bool.TryParse(userInput, out boolValue));

            return boolValue;
        }

        /// <summary>
        /// Gets a user input string value for the given message prompt.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string GetString(string message)
        {
            string userInput = String.Empty;
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format. Please try again.");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (String.IsNullOrEmpty(userInput));

            return userInput;
        }

        /// <summary>
        /// Gets a user input integer value from the provided allowable list.
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="allowableNumbers"></param>
        /// <param name="rangeErrorMessage"></param>
        /// <returns></returns>
        public static string GetStringInRange(string prompt, List<string> allowableStrings,
            string rangeErrorMessage = "Invalid entry. Please try again.")
        {
            string userInput = String.Empty;
            int numberOfAttempts = 0;

            while (!allowableStrings.Contains(userInput))
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine(rangeErrorMessage);
                }

                userInput = GetString(prompt);
                numberOfAttempts++;
            }

            return userInput;
        }

        /// <summary>
        /// Gets a user input DateTime object for the given message prompt.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static DateTime GetDate(string message)
        {
            string userInput = String.Empty;
            DateTime dateValue = new DateTime();
            int numberOfAttempts = 0;

            do
            {
                if (numberOfAttempts > 0)
                {
                    Console.WriteLine("Invalid input format or invalid date. Please try again.");
                }

                Console.Write(message + " ");
                userInput = Console.ReadLine();
                numberOfAttempts++;
            }
            while (!DateTime.TryParse(userInput, out dateValue));

            return dateValue;
        }

        public static Tuple<DateTime, DateTime> GetFutureDateRange(
            string startPrompt,
            string endPrompt,
            string wrongOrderMessage = "End date must be at least one day after start date.",
            bool allowAdministrativeOverride = false,
            string administrativeCode = null,
            string codeRequiredMessage = "Access code required.",
            string codePrompt = "Please enter code (0 to cancel): ")
        {

            DateTime startDate;
            DateTime endDate;
            int numberOfAttempts = 0;

            do
            {

                if (numberOfAttempts > 0)
                {
                    Console.WriteLine(wrongOrderMessage);
                }

                startDate = GetDate(startPrompt).Date;
                endDate = GetDate(endPrompt).Date;

                if (startDate < DateTime.Now.Date)
                {
                    if (allowAdministrativeOverride)
                    {
                        Console.WriteLine();
                        Console.WriteLine(codeRequiredMessage);
                        string administrativePass = GetStringInRange(codePrompt, new List<string> { "0", administrativeCode });
                        Console.WriteLine();
                        if (administrativePass == "0")
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }

                numberOfAttempts++;

            } while (startDate >= endDate);

            Tuple<DateTime, DateTime> dateRange = new Tuple<DateTime, DateTime>(startDate, endDate);

            return dateRange;
        }
    }
}
