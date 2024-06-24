﻿using CommunityToolkit.Mvvm.ComponentModel;

using Controls.UserDialogs.Maui;

namespace SweetMeSoft.Mobile.Base.ViewModels;

public class DialogsViewModel : ObservableObject
{
    public Task DisplayAlert(string title, string message, string cancel)
    {
        return UserDialogs.Instance.AlertAsync(message, title, cancel);
    }

    public Task<bool> DisplayAlert(string title, string message, string okText, string cancelText)
    {
        return UserDialogs.Instance.ConfirmAsync(message, title, okText, cancelText);
        //var tcs = new TaskCompletionSource<bool>();
        //Application.Current?.Dispatcher.Dispatch(async () =>
        //{
        //    try
        //    {
        //        var t = await UserDialogs.Instance.ConfirmAsync(message, title, okText, cancelText);
        //        tcs.SetResult(t);
        //    }
        //    catch (Exception e)
        //    {
        //        tcs.SetException(e);
        //    }
        //});

        //return await tcs.Task;
    }

    public async Task<string> DisplayPrompt(string title, string message, string accept = "Ok", string cancel = "Cancel", Keyboard keyboard = null, string initial = "")
    {
        var tcs = new TaskCompletionSource<string>();
        Application.Current?.Dispatcher.Dispatch(async () =>
        {
            try
            {
                var t = await Application.Current.MainPage.DisplayPromptAsync(title, message, accept, cancel, "", 50, keyboard, initial);
                tcs.SetResult(t);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        });

        return await tcs.Task;
    }

    public async Task DisplaySnackbar(string message, Action action = null)
    {
        var result = await UserDialogs.Instance.ShowSnackbarAsync(message, actionText: action != null ? "Ok" : null, showCountDown: action != null);
        if (action != null)
        {
            if (result == SnackbarActionType.UserInteraction)
            {
                action();
            }
        }
    }

    public void DisplayToast(string message, Action action = null)
    {
        UserDialogs.Instance.ShowToast(message);
    }
}