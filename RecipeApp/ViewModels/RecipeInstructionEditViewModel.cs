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

public class RecipeInstructionEditViewModel : ViewModelBase
{
    private Recipe Recipe;
    private ObservableCollection<Instruction> _instructions = new();
    public ObservableCollection<Instruction> Instructions
    {
        get => _instructions;
        set => this.RaiseAndSetIfChanged(ref _instructions, value);
    }

    private Instruction? _selectedInstruction;
    public Instruction? SelectedInstruction
    {
        get => _selectedInstruction;
        set => this.RaiseAndSetIfChanged(ref _selectedInstruction, value);
    }

    private string? _toAdd;
    public string? ToAdd
    {
        get => _toAdd;
        set => this.RaiseAndSetIfChanged(ref _toAdd, value);
    }

    public ReactiveCommand<Unit, Unit> Remove { get; }
    public ReactiveCommand<Unit, Recipe> Save { get; }
    public ReactiveCommand<Unit, Recipe> Cancel { get; }

    public ReactiveCommand<Unit, Unit> Add { get; }

    public RecipeInstructionEditViewModel(Recipe? recipe = null)
    {
        this.Recipe = new();
        if (recipe != null)
            this.Recipe = recipe;

        this.Instructions = new(this.Recipe.Instructions);

        IObservable<bool> isSelected = this.WhenAnyValue(
            recipeViewModel => recipeViewModel.SelectedInstruction,
            (Instruction? instr) => instr != null
        );

        Remove = ReactiveCommand.Create(() =>
            {
                Instructions.Remove(SelectedInstruction!);
            }, isSelected
        );

        IObservable<bool> atLeastOneInstruction = this.WhenAnyValue(vm => vm.Instructions.Count).Select(count => count > 0);

        Save = ReactiveCommand.Create(() =>
        {
            this.Recipe.Instructions = Instructions.ToList();
            return this.Recipe;
        }, atLeastOneInstruction);

        IObservable<bool> addNotNull = this.WhenAnyValue(
            recipeViewModel => recipeViewModel.ToAdd,
            (toAdd) => !string.IsNullOrEmpty(toAdd) //toAdd != null
        );

        Add = ReactiveCommand.Create(() =>
        {
            Instructions.Add(new Instruction(Instructions.Count(), ToAdd!));
            ToAdd = null;
        }, addNotNull);

        Cancel = ReactiveCommand.Create(() =>
        {
            return Recipe;
        });
    }
}