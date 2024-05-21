using App.Views;
using System;
using System.Reactive;
using ReactiveUI;
using App.ViewModels;

namespace App.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _contentViewModel;
    public MainWindowViewModel()
    {
        _contentViewModel = new WelcomeViewModel();
    }
    public ViewModelBase ContentViewModel
    {
        get => _contentViewModel;
        private set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
    }

    public void NavigateToWelcome()
    {
    ContentViewModel = new WelcomeViewModel();
    }

    public void NavigateToRegister()
    {
        RegisterViewModel viewModel = new();

        viewModel.Register.Subscribe(user =>
        {
            if (user != null)
            {
                NavigateToWelcome();
            }
            });

        ContentViewModel = viewModel;
    }

    public void NavigateToLogin()
    {
        LoginViewModel viewModel = new();

        viewModel.Login.Subscribe(user =>
        {
            if (user != null)
            {
            NavigateToUserDetail();
            }
        });

        ContentViewModel = viewModel;
    }

    public void NavigateToLoggedIn()
    {
        LoggedInViewModel viewModel = new();

        viewModel.Logout.Subscribe(_ => NavigateToWelcome());

        ContentViewModel = viewModel;
    }


    public void NavigateToUserDetail()
    {
        UserDetailsViewModel viewModel= new();

        viewModel.Update.Subscribe(_ => NavigateToUserUpdate());

        ContentViewModel = viewModel;
    }

    public void NavigateToUserUpdate()
    {
        UserUpdateViewModel viewModel= new();

        viewModel.Confirm.Subscribe(_ => NavigateToUserDetail());

        ContentViewModel = viewModel;
    }
}