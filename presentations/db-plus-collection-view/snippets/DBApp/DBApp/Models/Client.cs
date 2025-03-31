using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBApp.Models
{
    [Table("Client")]
    public class Client
    {
        [Column("id")]
        [PrimaryKey, AutoIncrement]
        public long Id { set; get; }
        [Column("name")]
        public string Name { set; get; }
        [Column("surname")]
        public string Surname { set; get; }
    }
}
