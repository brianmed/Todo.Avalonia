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
    public class MainWindowViewModel : ReactiveObject, IActivatableViewModel
    {
        public ViewModelActivator Activator { get; }

        public ObservableCollection<TodoFormDto> Todos { get; private set; } = new();

        private ObservableAsPropertyHelper<bool> _HasTodos;
        private bool HasTodos => _HasTodos.Value;

        private string TodoTitle { get; set; }

        public MainWindowViewModel()
        {
            Activator = new();

            Todos.ToObservableChangeSet(t => t.TodoEntityId)
                .AsObservableCache()
                .CountChanged
                .Select(v => v > 0)
                .ToProperty(this, vm => vm.HasTodos, out _HasTodos);

            using (IServiceScope serviceScope = Program.Host.Services.CreateScope())
            {
                IServiceProvider services = serviceScope.ServiceProvider;

                ITodoService todoService = services
                    .GetRequiredService<ITodoService>();

                Todos.AddRange(todoService.ReadAsync().GetAwaiter().GetResult()
                    .Adapt<IEnumerable<TodoFormDto>>());
            }            
        }

        public void OnAddButtonClicked()
        {
            using (IServiceScope serviceScope = Program.Host.Services.CreateScope())
            {
                IServiceProvider services = serviceScope.ServiceProvider;

                ITodoService todoService = services
                    .GetRequiredService<ITodoService>();

                TodoFormDto todo = todoService
                    .CreateAsync(TodoTitle)
                    .GetAwaiter().GetResult().Adapt<TodoFormDto>();

                Todos.Insert(0, todo);
            }
        }

        public void OnDoneCheckboxClicked(TodoFormDto todo)
        {
            using (IServiceScope serviceScope = Program.Host.Services.CreateScope())
            {
                IServiceProvider services = serviceScope.ServiceProvider;

                ITodoService todoService = services
                    .GetRequiredService<ITodoService>();

                todo.IsDone = todoService.ToggleAsync(todo.TodoEntityId)
                    .GetAwaiter().GetResult();
            }
        }
    }
}