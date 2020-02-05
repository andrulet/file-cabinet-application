using System;
using System.Collections.Generic;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// This class validates start commands of application.
    /// </summary>
    internal static class StartCommandValidator
    {
        /// <summary>
        /// Validates start command.
        /// </summary>
        /// <param name="args">Array of commands.</param>
        /// <returns>Dictionary of validating command.</returns>
        internal static Dictionary<string, string> ArgsValidator(string[] args)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            if (args == null || args.Length > 4)
            {
                return result;
            }

            string keyParam;
            string valueParam;
            int numberOfCommand = 0;
            while (numberOfCommand < args.Length)
            {
                if (args[numberOfCommand].Trim().StartsWith("--", StringComparison.InvariantCultureIgnoreCase))
                {
                    var command = args[numberOfCommand].Substring(2).Split('=', 2);
                    if (command != null && command.Length > 1)
                    {
                        keyParam = command[0].ToUpperInvariant();
                        valueParam = command[1].ToUpperInvariant();
                        result.TryAdd(keyParam, valueParam);
                    }
                }
                else if (args[numberOfCommand].Trim().StartsWith("-", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (args.Length > 1)
                    {
                        keyParam = args[numberOfCommand].Substring(1).ToUpperInvariant();
                        valueParam = args[numberOfCommand + 1].ToUpperInvariant();
                        result.TryAdd(keyParam, valueParam);
                    }
                }

                numberOfCommand++;
            }

            return result;
        }
    }
}
