using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData.Binding;
using ReactiveUI;
using recipes;
using users;

namespace App.ViewModels;

public class RecipeInstructionEditViewModel : ViewModelBase
{
    private ObservableCollection<Instruction> _instructions;
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
    public ReactiveCommand<Unit, List<Instruction>> Save { get; }
    public ReactiveCommand<Unit, Unit> Cancel { get; }

    public ReactiveCommand<Unit, Unit> Add { get; }

    public RecipeInstructionEditViewModel()
    {
        RecipeEditViewModel viewModel = new();
        viewModel.InstructionButton.Subscribe(recipe => 
        {
            if (recipe != null)
            {
                Instructions = new(recipe.Instructions);
            }
        });

        IObservable<bool> isSelected = this.WhenAny(
        recipeViewModel => recipeViewModel.SelectedInstruction,
        (instr) =>
            !(instr == null)
        );

        Remove = ReactiveCommand.Create(() =>
            {
                Instructions.Remove(SelectedInstruction!);
            }, isSelected
        );

        IObservable<bool> atLeastOneInstruction = this.WhenAnyValue(
        recipeViewModel => recipeViewModel.Instructions,
        (instr) =>
            (instr != null && instr.Count > 0)
        );

        Save = ReactiveCommand.Create(() =>
        {
            return Instructions.ToList();
        }, atLeastOneInstruction);

        IObservable<bool> addNotNull = this.WhenAnyValue<RecipeInstructionEditViewModel, bool>(
            recipeViewModel => recipeViewModel.ToAdd != null
        );

        Add = ReactiveCommand.Create(() => {
            Instructions.Add(new Instruction(Instructions.Count(), ToAdd!));
            ToAdd = null;
        }, addNotNull);
    }
}