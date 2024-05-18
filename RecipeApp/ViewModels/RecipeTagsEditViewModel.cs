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
    private string? _tag;
    public string? Tag
    {
        get => _tag;
        set => this.RaiseAndSetIfChanged(ref _tag, value);
    }

    private ObservableCollection<Tag> _tags;
    public ObservableCollection<Tag> Tags
    {
        get => new(_tags);
        set => this.RaiseAndSetIfChanged(ref _tags, value);
    }

    public RecipeTagsEditViewModel(Recipe? recipe = null)
    {
        this.Recipe = new();
        if (recipe != null)
            this.Recipe = recipe;
        
        
    }
}