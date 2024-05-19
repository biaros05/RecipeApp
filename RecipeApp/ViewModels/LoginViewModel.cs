using System;
using System.Reactive;
using ReactiveUI;
using users;


namespace App.ViewModels;
public class LoginViewModel : ViewModelBase{
    public ReactiveCommand<Unit, User?> Login { get; }

    private string? _username;
    public string? Username
    {
    get => _username;
    set => this.RaiseAndSetIfChanged(ref _username, value);
    }

    private string _password;
    public string Password{
        get=>_password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    private string _errorMessage;
    public string ErrorMessage{
        get=>_errorMessage;
        set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
    }

    public LoginViewModel()
    {
        IObservable<bool> areBothFilledIn = this.WhenAnyValue(
            loginViewModel => loginViewModel.Username,
            loginViewModel => loginViewModel.Password,
            (username, password) =>
            !(string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(password))
        );
    

        Login = ReactiveCommand.Create(() =>
        {
        // we know both values are not null, because of `areBothFilledIn`
        
            bool loggedIn = UserController.Instance.AuthenticateUser(Username,Password);

            if (loggedIn == false)
            {
                ErrorMessage = "Invalid username or password";
            }
            User currentLoggedIn=UserController.Instance.ActiveUser;
            return currentLoggedIn;
        }, areBothFilledIn);


    }

}
