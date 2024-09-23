using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData.Binding;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReactiveUI;
using recipes;
using users;

namespace App.ViewModels;

public class RecipeTagsEditViewModel : ViewModelBase
{
    private Recipe? Recipe;

    private string? _errorMessage;
    public string? ErrorMessage
    {
        get => _errorMessage;
        set => _errorMessage = value;
    }

    // display recipe tags 
    private ObservableCollection<Tag> _tags = new();
    public ObservableCollection<Tag> Tags
    {
        get => _tags;
        set => this.RaiseAndSetIfChanged(ref _tags, value);
    }

    // delete a tag
    private Tag? _selectedTag;
    public Tag? SelectedTag
    {
        get => _selectedTag;
        set => this.RaiseAndSetIfChanged(ref _selectedTag, value);
    }

    public ReactiveCommand<Unit, Unit> Remove {get;}

    // adding a tag to recipe
    private string? _newTag;
    public string? NewTag
    {
        get => _newTag;
        set => this.RaiseAndSetIfChanged(ref _newTag, value);
    }

    public ReactiveCommand<Unit, Unit> Add {get;}

    // save or cancel changes
    public ReactiveCommand<Unit, Recipe> Cancel {get;}
    public ReactiveCommand<Unit, Recipe> Save {get;}

    public RecipeTagsEditViewModel(Recipe? recipe = null)
    {
        this.Recipe = new();
        if (recipe != null)
            this.Recipe = recipe;
        
        this.Tags = new(this.Recipe.Tags);

        // add new tag

        IObservable<bool> newTagNotEmpty = this.WhenAnyValue(
            recipeViewModel => recipeViewModel.NewTag,
            (tag) => !string.IsNullOrEmpty(tag)
        );

        Add = ReactiveCommand.Create(() => 
        {
            Tag newTag = new(NewTag!);
            if (!Tags.Contains(newTag))
            {
                Tags.Add(newTag);
            }
            else
            {
                this.ErrorMessage = "This tag has already been added";
            }
            NewTag = "";
        }, newTagNotEmpty);

        // delete a tag
        IObservable<bool> selectedTag = this.WhenAnyValue(
            recipeViewModel => recipeViewModel.SelectedTag,
            (Tag? tag) => tag != null
        );

        Remove = ReactiveCommand.Create(() =>
        {
            Tags.Remove(SelectedTag!);
            SelectedTag = null;
        }, selectedTag);

        // Cancelling or saving
        Cancel = ReactiveCommand.Create(() =>
        {
            return Recipe;
        });

        Save = ReactiveCommand.Create(() =>
        {
            this.Recipe.Tags = Tags.ToList();
            return Recipe;
        });
    }
}