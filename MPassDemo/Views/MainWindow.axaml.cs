using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using MPassDemo.ViewModels;
using ReactiveUI;

namespace MPassDemo.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();

        this.WhenActivated(a => a(ViewModel.FileDialog.RegisterHandler(ShowFileDialog)));
    }

    private async Task ShowFileDialog(InteractionContext<Unit, IStorageFile?> interaction)
    {
        var top = GetTopLevel(this);
        var files = await top.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open File",
            AllowMultiple = false
        });
        interaction.SetOutput(files.FirstOrDefault());
    }

}
