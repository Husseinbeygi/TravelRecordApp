
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelRecordApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {

        private bool islocationPermissionGranted = false;
        public MapPage()
        {
            InitializeComponent();
            GetPermissions();
        }

        private async Task GetPermissions()
        {
            try
            {

                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.LocationWhenInUse);

                if (status != PermissionStatus.Granted)
                {

                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.LocationWhenInUse))
                    {
                        await DisplayAlert("Need Your Location", "We Need Your Location", "OK");
                    }
                    var result = await CrossPermissions.Current.RequestPermissionsAsync(Permission.LocationWhenInUse);
                    status = result[Permission.LocationWhenInUse];
                }

                if (status == PermissionStatus.Granted)
                {
                    locationMap.IsShowingUser=true;
                    locationMap.IsVisible= true;
                    islocationPermissionGranted = true;
                }
                else
                {
                    await DisplayAlert("Need Your Location", "You didn't give us the permission", "OK");
                }
            }
            catch (Exception ex)
            {

                await DisplayAlert("Error", $"Error : {ex.Message}", "OK");
            }

        }



        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (islocationPermissionGranted)
            {

                var locator = CrossGeolocator.Current;
                locator.PositionChanged +=Locator_PositionChanged;
                locator.StartListeningAsync(TimeSpan.Zero, 100);
            }
            GetLocation();
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {

                conn.CreateTable<Post>();
                var posts = conn.Table<Post>().ToList();
                DisplayOnMap(posts);
            };
        }

        private void DisplayOnMap(List<Post> posts)
        {
            try
            {

            foreach (var item in posts)
            {
                var postion = new Xamarin.Forms.Maps.Position(item.Latitude, item.Longitude);

                var pin = new Xamarin.Forms.Maps.Pin()
                {
                    Position = postion,
                    Address = item.Address,
                    Type = Xamarin.Forms.Maps.PinType.SavedPin,
                    Label = item.VenueName,
                };

                locationMap.Pins.Add(pin);
            }
            }
            catch (Exception ex)
            {

                DisplayAlert("Error", ex.Message, "OK");

            }
        }

        protected override void OnDisappearing()
        {
            CrossGeolocator.Current.PositionChanged -=Locator_PositionChanged;
            CrossGeolocator.Current.StopListeningAsync();
            base.OnDisappearing();
        }
        private void Locator_PositionChanged(object sender, PositionEventArgs e)
        {
            MoveMap(e.Position);
        }

        private async void GetLocation()
        {

            if (islocationPermissionGranted)
            {

                var locator = CrossGeolocator.Current;
                var position = await locator.GetPositionAsync();
                
                Console.WriteLine(position.Latitude+ ":" +  position.Longitude);   

                MoveMap(position);
            }

        }

        private void MoveMap(Position position)
        {
            Xamarin.Forms.Maps.Position center = new Xamarin.Forms.Maps.Position(position.Latitude, position.Longitude);
            Xamarin.Forms.Maps.MapSpan mapSpan = new Xamarin.Forms.Maps.MapSpan(center, 1, 1);
            locationMap.MoveToRegion(mapSpan);
        }
    }
}