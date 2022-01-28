using Plugin.Geolocator;
using SQLite;
using System;
using System.Linq;
using TravelRecordApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTravelPage : ContentPage
    {
        public NewTravelPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();
            var venues = await Venues.GetVenuesAsync(position.Longitude, position.Latitude);
            VenuesList.ItemsSource = venues;

        }
        private async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {

                var ven = VenuesList.SelectedItem as Result;
                var cat = ven.categories.FirstOrDefault();
                Post p = new Post()
                {
                    Experience = Exp_ent.Text,
                    Address = ven.location.address,
                    Distance = ven.distance,
                    Latitude = ven.geocodes.main.latitude,
                    Longitude  = ven.geocodes.main.longitude,
                    VenueName = ven.name,
                    CategoryId = cat.id,
                    CategoryName = cat.name,

                };
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Post>();
                    int rows = conn.Insert(p);
                    conn.Close();

                    if (rows > 0)
                    {
                        DisplayAlert("Success", "Experience successfully inserted", "OK");
                    }
                    else
                    {
                        DisplayAlert("Failure", "Experience Failed to  inserted", "OK");
                    }
                };

                await App.MobileService
            }
            catch (Exception er)
            {

                DisplayAlert("Error", er.Message, "OK");
            }

        }
    }
}