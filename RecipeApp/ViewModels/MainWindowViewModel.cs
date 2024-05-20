using App.Views;
using ReactiveUI;
using recipes;

namespace App.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private ViewModelBase _contentViewModel;
    public ViewModelBase ContentViewModel
    {
        get => _contentViewModel;
        private set => this.RaiseAndSetIfChanged(ref _contentViewModel, value);
    }

    public MainWindowViewModel(){
        _contentViewModel = new RecipeListViewModel(RecipeController.Instance.AllRecipes);
    }

    public void NavigateToRecipe(){
        _contentViewModel = new RecipeListViewModel(RecipeController.Instance.AllRecipes);
    }

    public void CancelButton() {
        //NOTE - back to welcome page
        _contentViewModel = new MainWindowViewModel();
    }
}   