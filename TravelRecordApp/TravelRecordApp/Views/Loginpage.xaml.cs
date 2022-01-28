using System;
using Xamarin.Forms;

namespace TravelRecordApp
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            var assembly = typeof(LoginPage);
            LoginLogo.Source = ImageSource.FromResource("TravelRecordApp.Assets.Images.LoginPage.group.png", assembly.Assembly);

        }

        private void btn_Login_Clicked(object sender, EventArgs e)
        {
            if (IsLoginDataEmpty())
            {
                Navigation.PushAsync(new HomePage());
            }
        }

        private bool IsLoginDataEmpty()
        {
            return !string.IsNullOrWhiteSpace(UserName.Text) && !string.IsNullOrWhiteSpace(Password.Text);
        }
    }
}
