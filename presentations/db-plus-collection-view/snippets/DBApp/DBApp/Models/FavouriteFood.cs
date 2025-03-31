using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBApp.Models
{
    [Table("FavouriteFood")]
    public class FavouriteFood
    {
        [Column("id")]
        [AutoIncrement, PrimaryKey]
        public long Id { get; set; }

        [Column("food_id")]
        public long FoodId { get; set; }

        [Column("client_id")]
        public long ClientId { get; set; }

        [Ignore]
        public Food Food { get; set; }

        [Ignore]
        public Client Client { get; set; }
    }
}