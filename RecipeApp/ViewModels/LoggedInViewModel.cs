
using System.Reactive;
using App.ViewModels;
using ReactiveUI;
using users;
namespace App.ViewModels;

public class LoggedInViewModel: ViewModelBase
{
    public ReactiveCommand<Unit, Unit> Logout { get; }
    public string Greeting{get;}

    public LoggedInViewModel()
    {
    Logout = ReactiveCommand.Create(() =>
        {
    // we know both values are not null, because of `areBothFilledIn`
        UserController.Instance.Logout();
        });
    // .CurrentlyLoggedInUser.DisplayName
    User u=UserController.Instance.ActiveUser;
    Greeting=$"hello {u.Username}";

    }

}