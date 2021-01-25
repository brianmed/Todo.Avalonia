using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

using Microsoft.Extensions.DependencyInjection;

using DynamicData;
using DynamicData.Binding;
using Mapster;
using ReactiveUI;

using Todo.FormDto;
using Todo.Services;

namespace Todo.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IActivatableViewModel
    {
        public ViewModelActivator Activator { get; }

        public ObservableCollection<TodoFormDto> Todos { get; private set; } = new();

        private ObservableAsPropertyHelper<bool> _HasTodos;
        private bool HasTodos => _HasTodos.Value;


        private string _TodoTitle;
        public string TodoTitle
        {
            get => _TodoTitle;
            set => this.RaiseAndSetIfChanged(ref _TodoTitle, value);
        }

        private ITodoService TodoService { get; init; }

        public MainWindowViewModel(ITodoService todoService)
        {
            TodoService = todoService;

            Activator = new();

            Todos.ToObservableChangeSet(t => t.TodoEntityId)
                .AsObservableCache()
                .CountChanged
                .Select(v => v > 0)
                .ToProperty(this, vm => vm.HasTodos, out _HasTodos);

            Todos.AddRange(TodoService.ReadAsync().GetAwaiter().GetResult()
                .Adapt<IEnumerable<TodoFormDto>>());
        }

        public void OnAddButtonClicked()
        {
            TodoFormDto todo = TodoService
                .CreateAsync(TodoTitle)
                .GetAwaiter().GetResult().Adapt<TodoFormDto>();

            Todos.Insert(0, todo);

            TodoTitle = String.Empty;
        }

        public void OnDoneCheckboxClicked(TodoFormDto todo)
        {
            todo.IsDone = TodoService.ToggleAsync(todo.TodoEntityId)
                .GetAwaiter().GetResult();
        }
    }
}