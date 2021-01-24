using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

using Microsoft.Extensions.DependencyInjection;

using Todo.Services;
using Todo.ViewModels;

namespace Todo.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        private void OnAddButtonClicked(object sender, RoutedEventArgs e)
        {
            TextBox addMe = this.FindControl<TextBox>("TodoHack");

            using (IServiceScope serviceScope = Program.Host.Services.CreateScope())
            {
                IServiceProvider services = serviceScope.ServiceProvider;

                ITodoService todoService = services.GetRequiredService<ITodoService>();

                (this.DataContext as MainWindowViewModel).AddTodo(addMe.Text);

                addMe.Text = String.Empty;
            }
        }
    }
}