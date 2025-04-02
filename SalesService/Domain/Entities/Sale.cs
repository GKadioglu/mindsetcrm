using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace SalesService.Domain.Entities
{
    public class Sale
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string CustomerName { get; set; }     // Müşteri ismi
        public string Product { get; set; }           // Satılan ürün
        public decimal Amount { get; set; }           // Tutar
        public string Status { get; set; } = "Yeni";  // Yeni, İletişimde, Anlaşma, Kapandı

        public List<SaleNote> Notes { get; set; } = new(); // Notlar ve tarihleri
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastStatusChange { get; set; } = DateTime.UtcNow;
    }

    public class SaleNote
    {
        public string Note { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}