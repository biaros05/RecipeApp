using System;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using ReactiveUI;
using users;

namespace App.ViewModels;

public class UpdateImageViewModel : ViewModelBase
{
    public ReactiveCommand<Window, Task<byte[]>> SelectImage { get; }

    public ReactiveCommand<Unit, Unit> ClearImage { get; }

    public ReactiveCommand<Unit, Unit> Okay { get; }

    private static readonly Bitmap PLACEHOLDER =
    // This shows an example of loading an image from the assets directory.
    new(AssetLoader.Open(new Uri("avares://App/Assets/default.jpg")));

    private Bitmap _imageDisplayed;

    public Bitmap ImageDisplayed
    {
    get => _imageDisplayed;
    set => this.RaiseAndSetIfChanged(ref _imageDisplayed, value);
    }

    public UpdateImageViewModel()
    {
        if (UserController.Instance.ActiveUser.Image==null)
        {
            ImageDisplayed=PLACEHOLDER;
        }
        else{
            ImageDisplayed= new(new MemoryStream(UserController.Instance.ActiveUser.Image));
        }
        
        Okay = ReactiveCommand.Create(() => { });

        SelectImage = ReactiveCommand.Create(async (Window window) =>
        {
        // https://docs.avaloniaui.net/docs/basics/user-interface/file-dialogs
            var files = await window.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
        Title = "Open Image File",
        AllowMultiple = false,
        // https://docs.avaloniaui.net/docs/concepts/services/storage-provider/file-picker-options
        FileTypeFilter = new[] { FilePickerFileTypes.ImageAll }
        });


        // https://stackoverflow.com/a/221941
            using var memoryStream = new MemoryStream();

            if (files.Count >= 1)
            {
            // Open reading stream from the first file.
            await using var fileStream = await files[0].OpenReadAsync();
            await fileStream.CopyToAsync(memoryStream);
            }
        // Reads all the content of file as an image.
            byte[] image = memoryStream.ToArray();

        // Here I could save the image into a user object:
        // myUser.ProfilePicture = image;
        string username =null;
        string description=null;
        string password=null;
            UserController.Instance.UpdateUser(username,description,image,password);
            return image;
        });

        SelectImage.Subscribe(async (readingImageTask) =>
        {
            byte[] imageData = await readingImageTask;
            
        // For the project, there is no need to have this async lambda, simply
        // create a Bitmap using the byte array saved in DB and set the property
        // (`ImageDisplayed` in this case) that is bound to the Image in the view.
            ImageDisplayed = new(new MemoryStream(imageData));
        });

        ClearImage = ReactiveCommand.Create(() =>
        {
            // Byte[] array = new Byte[64]
            // UserController.Instance.UpdateUser(null,null,empty,null);
            string username =null;
            string description=null;
            string password=null;
            ImageDisplayed = PLACEHOLDER;
            UserController.Instance.UpdateUser(username,description,null,password);

        });
    }

}