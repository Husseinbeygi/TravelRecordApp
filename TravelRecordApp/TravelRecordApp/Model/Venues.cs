using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TravelRecordApp.Helper;

namespace TravelRecordApp.Model
{

    public class Icon
    {
        public string prefix { get; set; }
        public string suffix { get; set; }
    }

    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
        public Icon icon { get; set; }
    }

    public class Main
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class Geocodes
    {
        public Main main { get; set; }
    }

    public class Location
    {
        public string address { get; set; }
        public string country { get; set; }
        public string cross_street { get; set; }
        public string locality { get; set; }
        public string postcode { get; set; }
        public string region { get; set; }
    }

    public class RelatedPlaces
    {
    }

    public class Result
    {
        public string fsq_id { get; set; }
        public IList<Category> categories { get; set; }
        public IList<object> chains { get; set; }
        public int distance { get; set; }
        public Geocodes geocodes { get; set; }
        public Location location { get; set; }
        public string name { get; set; }
        public RelatedPlaces related_places { get; set; }
        public string timezone { get; set; }
    }



    public class Venues
    {
        public IList<Result> results { get; set; }
        public static string GenerateUrl(double longitude, double latitude)
        {

            return string.Format(Constant.VENUS_SEARCH,latitude, longitude);
        }

        public async static Task<List<Result>> GetVenuesAsync(double longitude, double latitude)
        {
            List<Result> venues = new List<Result>();
            using (HttpClient httpClient = new HttpClient())
            {
                string contentType = "application/json";
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", Constant.CLIENT_AUTHCODE);
                var response = await httpClient.GetAsync(Venues.GenerateUrl(longitude, latitude));
                var json = await response.Content.ReadAsStringAsync();


                var venueroot = JsonConvert.DeserializeObject<Venues>(json);

                venues = venueroot.results as List<Result>;

            }

            return venues;
        }

    }



}
