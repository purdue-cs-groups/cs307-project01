using System;
using System.Windows;
using Coding4Fun.Phone.Controls;

namespace MetrocamPan.Helpers
{
    public class GlobalToastPrompt
    {
        public static ToastPrompt CreateToastPrompt(string title = "Title goes here", 
            string message = "Message goes here", 
            int milliSecondsUntilHidden = 2000, 
            TextWrapping wrapOption = TextWrapping.Wrap)
        {
            ToastPrompt newToastPrompt = new ToastPrompt();

            newToastPrompt.Title = title;
            newToastPrompt.Message = message;
            newToastPrompt.MillisecondsUntilHidden = milliSecondsUntilHidden;
            newToastPrompt.TextWrapping = wrapOption;

            return newToastPrompt;
        }
    }
}
