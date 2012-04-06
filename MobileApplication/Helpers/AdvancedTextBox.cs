using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MetrocamPan.Helpers
{
    public class AdvancedTextBox : TextBox
    {
        private String defaultText = String.Empty;

        public String DefaultText
        {
            get
            {
                return defaultText;
            }
            set
            {
                defaultText = value;
                SetDefaultText();
            }
        }

        public AdvancedTextBox()
        {
            this.GotFocus += (sender, e) =>
            {
                if (this.Text.Equals(DefaultText))
                {
                    this.Text = String.Empty;
                }
            };
            this.LostFocus += (sender, e) =>
            { 
                SetDefaultText(); 
            };
        }

        private void SetDefaultText()
        {
            if (this.Text.Trim().Length == 0)
            {
                this.Text = DefaultText;
            }
        }
    }
}
