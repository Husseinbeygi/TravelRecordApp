﻿using SQLite;

namespace TravelRecordApp.Model
{
    public class Post
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Experience { get; set; }
        public string VenueName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Distance { get; set; }
        public string Address { get; set; }

    }
}
