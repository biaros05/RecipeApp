using System.Collections.ObjectModel;
using users;

namespace App.ViewModels;

public class WelcomeViewModel : ViewModelBase
{
  public ObservableCollection<User> Users { get; }

  public WelcomeViewModel()
  {
    // Users = new(UserController.GetAllUsers());
    //we dont want to display the users info
  }
}
