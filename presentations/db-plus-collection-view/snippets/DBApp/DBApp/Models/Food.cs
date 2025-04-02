using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBApp.Models
{
    // #region snippet
    public enum FoodType
    {
        Vegetable, Fruit, Seafood, Meat
    }
    [Table("Food")]
    public class Food
    {
        [Column("id")]
        [AutoIncrement, PrimaryKey]
        public long Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column ("in_stock")]
        public int InStock { get; set; }
        [Column("type")]
        public FoodType Type { get; set; }
    }
    // #endregion snippet
}