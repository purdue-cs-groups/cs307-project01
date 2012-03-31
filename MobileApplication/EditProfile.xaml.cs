using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace MetrocamPan
{
    public partial class EditProfile : PhoneApplicationPage
    {
        public EditProfile()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Your profile has been updated.");
            NavigationService.GoBack();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Your changes have been discarded.");
            NavigationService.GoBack();
        }

        private void usernameInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Dispatcher.BeginInvoke(() => usernameInput.Focus());
                Dispatcher.BeginInvoke(() => textBox1.Focus());
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Dispatcher.BeginInvoke(() => textBox1.Focus());
                Dispatcher.BeginInvoke(() => textBox2.Focus());
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Dispatcher.BeginInvoke(() => textBox2.Focus());
                Dispatcher.BeginInvoke(() => textBox3.Focus());
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Focus();
            }
        }
    }
}