using System;

using Microsoft.Extensions.DependencyInjection;

using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

using Todo.AppCtx;
using Todo.FormDto;
using Todo.Services;
using Todo.ViewModels;

namespace Todo.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            ViewModel = ViewModelBase.Create<MainWindowViewModel>();

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}