using SQLite;
using System;
using TravelRecordApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostDetailsPage : ContentPage
    {
        private Post selectedPost;
        public PostDetailsPage(Post postModel)
        {
            InitializeComponent();
            EntryText.Text = postModel.Experience;
            selectedPost = postModel;
        }

        private void UpdateButton_Clicked(object sender, EventArgs e)
        {
            selectedPost.Experience = EntryText.Text;   
            using (SQLiteConnection conn = new SQLite.SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Post>();
                int rows = conn.Update(selectedPost);
                conn.Close();

                if (rows > 0)
                {
                    var d = DisplayAlert("Success", "Experience successfully Updated", "OK");
                    Navigation.PopAsync();

               
                }
                else
                {
                    DisplayAlert("Failure", "Experience Failed to  Update", "OK");
                }
            };


        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLite.SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Post>();
                int rows = conn.Delete(selectedPost);
                conn.Close();

                if (rows > 0)
                {
                    var d = DisplayAlert("Success", "Experience successfully Deleted", "OK");
                    if (d.IsCompleted)
                    {
                        Navigation.PopAsync();

                    }

                }
                else
                {
                    DisplayAlert("Failure", "Experience Failed to  Delete", "OK");
                }
            };

        }
    }
}