using Microsoft.AspNetCore.Components.Web;
using MongoDB.Bson;

namespace DostavkaNaMarket.Models
{
    public class AdminModel
    {
        public string? OrderNumber { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public bool isChecked { get; set; }

        public enum GetMethod
        {
            All,
            Sam,
            Vivoz,
        }
        public GetMethod? GetMeth { get; set; } = GetMethod.All;

        public enum PaymentMethod
        {
            All,
            Cash,
            Card,
        }
        public PaymentMethod? PayMet { get; set; } = PaymentMethod.All;

        
    }
}

