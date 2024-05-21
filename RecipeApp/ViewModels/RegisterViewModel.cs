using System;
using System.Reactive;
using App.ViewModels;
using App.Views;
using ReactiveUI;
using users;

namespace App.ViewModels;

public class RegisterViewModel : ViewModelBase
{
  /// <summary>
  /// This is the user we are in the process of creating, its prperties are also
  /// used to perform validation.
  /// </summary>
  private User UserToRegister { get; } = new();

  private string? _username;
  public string? Username
  {
    get => _username;
    set {
      UserToRegister.Username = value;

      this.RaiseAndSetIfChanged(ref _username, value);
    }
      // if this throws an exception (i.e. value is invalid), the exception's
      // message is shown in the GUI.
      
  }

  private string? _password;
  public string? Password
  {
    get => _password;
    set
    {
      if (value == null)
      {
        throw new ArgumentNullException(nameof(Password));
      }
      else if (value.Length < UserController.Instance.MIN_PASSWORD_LENGTH
        || value.Length > UserController.Instance.MAX_PASSWORD_LENGTH)
      {
    
      }

      this.RaiseAndSetIfChanged(ref _password, value);
    }
  }

  private string? _confirmPassword;
  public string? ConfirmPassword
  {
    get => _confirmPassword;
    set
    {
      if (!string.Equals(value, Password))
      {
        throw new ArgumentException("Must match the first password",
          nameof(ConfirmPassword));
      }

      this.RaiseAndSetIfChanged(ref _confirmPassword, value);
    }
  }

  private string? _description;
  public string? Description
  {
    get => _description;
    set {
      UserToRegister.Description = value;
      this.RaiseAndSetIfChanged(ref _description, value);
    }
      // if this throws an exception (i.e. value is invalid), the exception's
      // message is shown in the GUI.
  }

  private string? _errorMessage;
  public string? ErrorMessage
  {
    get => _errorMessage;
    set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
  }

  public ReactiveCommand<Unit, User?> Register { get; }

  public RegisterViewModel()
  {
    Register = ReactiveCommand.Create(() =>
    {
      try
      {
            User user1=UserController.Instance.CreateAccount(Username, Password!,Description);
            // LoginViewModel viewModel = new();
            return user1;
      }
      catch (Exception exc)
      when (exc is ArgumentException || exc is NullReferenceException)
      {
        this.ErrorMessage = exc.Message;
        Console.WriteLine(this.ErrorMessage);
        return null;
      }

      
    });
  }

//   public void NavigateToLogin()
// {
//   LoginViewModel viewModel = new();

//   viewModel.Login.Subscribe(user =>
//   {
//     if (user != null)
//     {
//       NavigateToLoggedIn();
//     }
//   });

  

}
