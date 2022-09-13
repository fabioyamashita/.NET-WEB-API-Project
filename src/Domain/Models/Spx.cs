using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SPX_WEBAPI.Domain.Models
{
    public class Spx
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("Date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("Close")]
        public decimal Close { get; set; }

        [JsonPropertyName("Open")]
        public decimal Open { get; set; }

        [JsonPropertyName("High")]
        public decimal High { get; set; }

        [JsonPropertyName("Low")]
        public decimal Low { get; set; }

        public Spx(int id, DateTime date, decimal close, decimal open, decimal high, decimal low)
        {
            Id = id;
            Date = date;
            Close = close;
            Open = open;
            High = high;
            Low = low;
        }
    }
}
