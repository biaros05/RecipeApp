using System.Runtime.InteropServices;
using System.Text;

namespace Ui;

/// <summary>
/// Provides utility methods for getting various forms of valid user input. Most
/// of these methods clear the console when called.
/// </summary>
public static class ConsoleUtils
{
  /// <summary>
  /// Describes the key sequence to send an EOF character in the terminal. This
  /// key sequence depends on the platform (Windows, Mac, Linux, etc.)
  /// </summary>
  private static string EOFKeySequence { get; }

  /// <summary>
  /// Initializes `EOFKeySequence` based on the current platform
  /// </summary>
  static ConsoleUtils()
  {
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
      EOFKeySequence = "CTRL+Z, ENTER";
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
      RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    {
      EOFKeySequence = "CTRL+D";
    }
    else
    {
      throw new PlatformNotSupportedException();
    }
  }

  /// <summary>
  /// Blocks code execution until the user presses a key
  /// </summary>
  public static void WaitUserPressKey()
  {
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey(true);
  }

  /// <summary>
  /// Gets a valid bool value (true/false, yes/no, confirm/cancel, etc.) from
  /// the user, but will only ask once and then use a default value if the
  /// provided value is invalid. Careful, this clears the console.
  /// </summary>
  /// <param name="prompt">Prompt displayed to the user</param>
  /// <param name="defaultValue">When the user fails to select either valid
  /// answer, this is the default value used.</param>
  /// <param name="trueLabel">What string is used to represent `true`</param>
  /// <param name="falseLabel">What string is used to represent `false`</param>
  public static bool GetValidBool(
    string prompt, bool defaultValue = true,
    string trueLabel = "Y", string falseLabel = "N")
  {
    string defaultLabel = defaultValue ? trueLabel : falseLabel;
    prompt += $"\n\n{trueLabel}/{falseLabel} ({defaultLabel}): ";

    return GetValidInput(prompt,
      // No validation needed, an invalid value will be parsed as `defaultValue`
      (_) => true,
      (string? input) =>
      {
        if (trueLabel.Equals(input, StringComparison.OrdinalIgnoreCase))
        {
          return true;
        }
        else if (falseLabel.Equals(input, StringComparison.OrdinalIgnoreCase))
        {
          return false;
        }
        else
        {
          return defaultValue;
        }
      });
  }

  /// <summary>
  /// Gets a non-empty and non-null string, this will keep asking the user until
  /// they provide such a string. Careful, this clears the console.
  /// </summary>
  /// <param name="prompt">Prompt displayed to the user</param>
  public static string GetNonEmptyString(string prompt) => GetValidInput(
    prompt,
    // The only validation needed is that the string is non empty/null
    (string? input) => !string.IsNullOrEmpty(input),
    // Once the string is ready to be parsed, it shouldn't be null because of
    // our validation. But the compiler doesn't know that, hence why we need to
    // have null-coalescing here.
    (string? input) => input ?? ""
  );

  /// <summary>
  /// Provides the user with a console interface to enter a multi-line string.
  /// This allows for any non-null string (i.e. could be empty). Careful, this
  /// clears the console.
  /// </summary>
  /// <param name="prompt">Prompt displayed to the user</param>
  public static string GetMultiLineText(string prompt)
  {
    var text = new StringBuilder();
    var stop = false;

    prompt = $"(Press {EOFKeySequence} to exit)\n{prompt}";

    Console.Clear();
    Console.WriteLine(prompt);

    do
    {
      string? nextLine = Console.ReadLine();

      if (nextLine == null)
      {
        stop = GetValidBool("Stop entering text?");

        if (!stop)
        {
          // Clear GetValidBool's output ...
          Console.Clear();
          // ... show the prompt again (because it got cleared) ...
          Console.WriteLine(prompt);
          // ... show text entered so far, to keep UI consistent
          Console.Write(text.ToString());
        }
      }
      else
      {
        text.AppendLine(nextLine);
      }
    } while (!stop);

    // Remove the last new line character that gets added because we're using
    // the `AppendLine()` method.
    if (text.Length >= Environment.NewLine.Length)
    {
      text.Remove(text.Length - Environment.NewLine.Length,
        Environment.NewLine.Length);
    }

    return text.ToString();
  }

  /// <summary>
  /// Provides the user with a console interface to select one choice among a
  /// numbered list, using the choice's number. The chosen element is returned
  /// or null if the user decided to cancel. Careful, this clears the console.
  /// </summary>
  /// <param name="prompt">Prompt displayed to the user</param>
  /// <param name="choices">Choices for the user to choose among</param>
  public static T? GetUserChoice<T>(
    string prompt, T[] choices) where T : struct
  {
    // Building the prompt message, to include the choices
    prompt = $"(Press {EOFKeySequence} to exit)\n{prompt}";
    for (var i = 0; i < choices.Length; i++)
    {
      prompt += $"\n {i + 1}.\t{choices[i]}";
    }
    prompt += "\n\n: ";

    return GetValidInput<T?>(prompt,
      (string? input) => input == null || int.TryParse(input, out int parsedInt)
        // User's choice is 1-indexed, so we validate that range
        && parsedInt > 0 && parsedInt <= choices.Length,
      (string? input) => input == null ? null :
        // Need to subtract 1, so it becomes 0-indexed
        choices[int.Parse(input) - 1]);
  }

  /// <summary>
  /// General method for getting valid user input, given a definition of what
  /// constitutes a "valid" value and how to "parse" that value from string to
  /// some type T. Keeps on asking until the user provides a valid value.
  /// Careful, this clears the console.
  /// </summary>
  /// <param name="prompt">Prompt displayed to the user</param>
  /// <param name="validate">Defines what's a valid value. This function should
  /// only return true if the provided string is considered valid</param>
  /// <param name="parse">Parses a valid value from a string. This function will
  /// only be called on a valid string</param>
  public static T GetValidInput<T>(
    string prompt,
    Func<string?, bool> validate,
    Func<string?, T> parse)
  {
    string? userInput;

    do
    {
      Console.Clear();
      Console.Write(prompt);
      userInput = Console.ReadLine();
    } while (!validate(userInput));

    return parse(userInput);
  }
}