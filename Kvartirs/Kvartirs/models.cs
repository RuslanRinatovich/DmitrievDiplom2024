using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kvartirs
{
    class models
    {
        public class Eventname
        {
            public int ID_event { get; set; }
            public string event_name { get; set; }
            public DateTime event_date { get; set; }
            public int ID_category { get; set; }
            public string category_name { get; set; }
            public string info { get; set; }
            public byte[] photo { get; set; }
            public int status { get; set; }
        }

        public class Flat
        {
            public int ID_flat { get; set; }
            public int flat_number { get; set; }
            public string address { get; set; }
            public int storey { get; set; }
            public int people_count { get; set; }
            public double square_flat { get; set; }
            public bool estove { get; set; }
            public int bonus { get; set; }
            public int ID_Customer { get; set; }
            public int ID_tarif { get; set; }
            public string ID_kadastr { get; set; }
        }
        public class Tarif
        {
            public int ID_tarif { get; set; }
            public string tarif_name { get; set; }
            public double price1 { get; set; }
            public double price2 { get; set; }
            public double price3 { get; set; }
        }
    }
}
