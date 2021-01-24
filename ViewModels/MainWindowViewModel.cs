using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

using Microsoft.Extensions.DependencyInjection;

using Mapster;

using Todo.FormDto;
using Todo.Services;

namespace Todo.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ObservableCollection<TodoFormDto> Todos { get; private set; }

        public MainWindowViewModel()
        {
            using (IServiceScope serviceScope = Program.Host.Services.CreateScope())
            {
                IServiceProvider services = serviceScope.ServiceProvider;

                ITodoService todoService = services
                    .GetRequiredService<ITodoService>();

                Todos = new ObservableCollection<TodoFormDto>(todoService.ReadAsync()
                    .GetAwaiter().GetResult().Adapt<IEnumerable<TodoFormDto>>());

                _HasTodos = Todos.Count > 0;
            }            

            Todos.CollectionChanged += OnCollectionChanged;
        }

        ~MainWindowViewModel()
        {
            Todos.CollectionChanged -= OnCollectionChanged;
        }

        private bool _HasTodos;
        public bool HasTodos
        {
            get
            {
                return _HasTodos;
            }

            set
            {
                _HasTodos = value;

                OnPropertyChanged(nameof(HasTodos));
            }
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<TodoFormDto> todos = sender as ObservableCollection<TodoFormDto>;

            _HasTodos = todos.Count > 0;

            OnPropertyChanged(nameof(HasTodos));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public void AddTodo(string title)
        {
            using (IServiceScope serviceScope = Program.Host.Services.CreateScope())
            {
                IServiceProvider services = serviceScope.ServiceProvider;

                ITodoService todoService = services
                    .GetRequiredService<ITodoService>();

                TodoFormDto todo = todoService
                    .CreateAsync(title)
                    .GetAwaiter().GetResult().Adapt<TodoFormDto>();

                Todos.Insert(0, todo);
            }
        }
    }
}