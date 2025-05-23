﻿using Controls.UserDialogs.Maui;

using SweetMeSoft.Mobile.Base.Popup;

namespace SweetMeSoft.Mobile.Base.Tools;

public class PhotoHandler
{
    public static async Task<PhotoResult> TakePhoto()
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            try
            {
                PopupsService.Instance.ShowLoading("Tomando foto...");
                var result = new PhotoResult();
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
                await Task.Delay(1000); //Delay is needed to wait back to the previous page
                if (photo != null)
                {
                    if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                    {
                        result.Path = photo.FullPath;
                    }
                    else
                    {
                        var fullPath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                        using var stream = await photo.OpenReadAsync();
                        using var newStream = File.Create(fullPath);
                        await stream.CopyToAsync(newStream);
                        result.Path = fullPath;
                    }
                    PopupsService.Instance.HideLoading();
                    return result;
                }

                PopupsService.Instance.HideLoading();
                return null;
            }
            catch (Exception e)
            {
                PopupsService.Instance.HideLoading();
                if (e.GetType() == typeof(PermissionException))
                {
                    await UserDialogs.Instance.AlertAsync("No se tienen los permisos necesarios para tomar fotos. Habilita los permisos en los ajustes de la app para poder continuar.", "Error", "Aceptar");
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync("Error al tomar la foto", "Error", "Aceptar");
                }
                return null;
            }
        }
        else
        {
            await UserDialogs.Instance.AlertAsync("No se puede tomar fotos en este dispositivo", "Error", "Aceptar");
            return null;
        }
    }

    public static async Task<List<PhotoResult>> TakePhotos()
    {
        var results = new List<PhotoResult>();
        PhotoResult photo;
        do
        {
            photo = await TakePhoto();
            if (photo != null)
            {
                results.Add(photo);
            }
        }
        while (photo != null);
        return results;
    }
}