using System;
using Newtonsoft.Json;

namespace WebApplication1.Models
{
    public class Currencies
    {
        public DateTime Date { get; set; }
        public DateTime PreviousDate { get; set; }
        public string PreviousURL { get; set; }
        public DateTime Timestamp { get; set; }
        //public List<Currency> Valute { get; set; }

        [JsonProperty(PropertyName = "Valute")]
        public Dictionary<string, Currency> Valute { get; set; }

        //public Currencies()
        //{
        //}
    }
}

