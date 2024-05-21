using System;
using System.Reactive;
using App.ViewModels;
using App.Views;
using ReactiveUI;
using users;

namespace App.ViewModels;

public class UserUpdateViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, User?> Confirm { get; }

    private string? _username;
    public string? Username
    {
    get => _username;
    set => this.RaiseAndSetIfChanged(ref _username, value);
    }

    private string? _password;
    public string? Password{
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    private string? _description;
    public string? Description{
        get => _description;
        set => this.RaiseAndSetIfChanged(ref _description, value);
    }

    private byte[]? _image;
    public byte[]? Image{
        get => _image;
        set => this.RaiseAndSetIfChanged(ref _image, value);
    }
    
    public UserUpdateViewModel()
    {
        IObservable<bool> areAllFilledIn = this.WhenAnyValue(
            loginViewModel => loginViewModel.Username,
            loginViewModel => loginViewModel.Password,
            // loginViewModel => loginViewModel.Image,
            //need to chjange this bec if a a field is empty or null it shjould leave those fields as the current info
            //or i should do that the textboxes are already filled with current info.
            (username, password) =>
            !(string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(password))
        );

        Confirm = ReactiveCommand.Create(() =>
        {
        // we know both values are not null, because of `areBothFilledIn`
        
            UserController.Instance.UpdateUser(Username, Description, Image=null,Password);

            // if (loggedIn == false)
            // {
            //     ErrorMessage = "Invalid username or password";
            // }
            User currentLoggedIn=UserController.Instance.ActiveUser;
            return currentLoggedIn;
        }, areAllFilledIn);
    }
}