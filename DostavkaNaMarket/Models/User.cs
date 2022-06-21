using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Components.Web;
using MongoDB.Bson.Serialization.Attributes;

namespace DostavkaNaMarket.Models
{
    public class User
    {
        public enum City
        {
            Владимир,
            Иваново,
            Муром,
            Ковров
        }
        public string CitySell { get; set; } = $"{City.Владимир}";

        public enum Date
        {
            Понедельник,
            Среда,
        }
        [Required(ErrorMessage = "Место и дату отправки нужно выбрать обязательно")]
        public Date? DateSell { get; set; } = null;

        public DateTime date = DateTime.Now;
        public DateTime dateWednesday = new DateTime();
        public DateTime dateMonday = new DateTime();

        [Required(ErrorMessage = "Поле ФИО обязательно для заполнения")]
        public string? ClientName { get; set; }

        [Required(ErrorMessage = "Поле Номер телефона обязательно для заполнения")]
        [MaxLength(10, ErrorMessage = "Номер телефона должен быть в формате 9011418686")]
        [DataType(DataType.PhoneNumber)]
        public string? ClientPhone { get; set; }

        public string? ClientMail { get; set; }

        public enum OfficeOrDelivery
        {
            Sam,
            Vivoz,
        }
        public OfficeOrDelivery? OfDelSell { get; set; } = OfficeOrDelivery.Sam;
        public Dictionary<string, object>? InputAttributes { get; set; }
        
        public string? ClientAdress { get; set; }

        public List<string> AllNumbersBarcode = new List<string>();
        public string? barcode { get; set; }

        public enum PaymentMethod
        {
            Cash,
            Card,
        }
        public PaymentMethod? PayMet { get; set; } = PaymentMethod.Cash;

        public int BoxPrice = 200;
        public int? BoxAmount { get; set; }

        public int? DeliveryPrice { get; set; }

        public int? FinalAmount { get; set; }

        [Required]
        [Range(typeof(bool), "true", "true",
        ErrorMessage = "This form disallows unapproved ships.")]
        public bool isChecked { get; set; }
        public Dictionary<string, object>? InputAttributesSubmit { get; set; }

        public string? dateDB { get; set; }
        public string? getMethodDB { get; set; }
        public string? paymentMethodDB { get; set; }

        [BsonId]
        public string userIdDB { get; set; }

        // Не красивый, но рабочий метод определения даты отправки
        public void DateDelivery()
        {
            if ((date.ToString("dddd")) == "понедельник")
            {
                dateMonday = date;
                dateWednesday = date.AddDays(2);
            }
            else if ((date.ToString("dddd")) == "вторник")
            {
                dateMonday = date.AddDays(6);
                dateWednesday = date.AddDays(1);
            }
            else if ((date.ToString("dddd")) == "среда")
            {
                dateMonday = date.AddDays(5);
                dateWednesday = date;
            }
            else if ((date.ToString("dddd")) == "четверг")
            {
                dateMonday = date.AddDays(4);
                dateWednesday = date.AddDays(6);
            }
            else if ((date.ToString("dddd")) == "пятница")
            {
                dateMonday = date.AddDays(3);
                dateWednesday = date.AddDays(5);
            }
            else if ((date.ToString("dddd")) == "суббота")
            {
                dateMonday = date.AddDays(2);
                dateWednesday = date.AddDays(4);
            }
            else if ((date.ToString("dddd")) == "воскресенье")
            {
                dateMonday = date.AddDays(1);
                dateWednesday = date.AddDays(3);
            }
        }

        public void OnSelected()
        {
            if (OfDelSell is OfficeOrDelivery.Sam)
            {
                InputAttributes = new Dictionary<string, object>()
                {
                    { "style", "display:none;" }
                };
            }
            else if (OfDelSell is OfficeOrDelivery.Vivoz)
            {
                InputAttributes = new Dictionary<string, object>()
                {
                    { "style", "display:inline;" }
                };
            }
        }

        public void AddToList()
        {
            if(barcode != null)
            {
                AllNumbersBarcode.Add(barcode);
                barcode = null;
            }
            else barcode = null;
        }

        public void RemoveBarcode(string bar)
        {
            AllNumbersBarcode.Remove(bar);
        }

        public void Enter(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                AddToList();
            }
        }

        public void SubmitChanged()
        {
            if (isChecked)
            {
                InputAttributesSubmit = new Dictionary<string, object>()
                {
                    { "class", "w-100 btn btn-primary btn-lg" },
                    { "type", "submit" },
                };
            }
            else if (isChecked == false)
            {
                InputAttributesSubmit = new Dictionary<string, object>()
                {
                    { "class", "w-100 btn btn-secondary btn-lg" },
                    { "type", "button" }
                };
            }
        }

        public void CalculateFinalAmount()
        {
            BoxAmount = AllNumbersBarcode.Count * BoxPrice;

            if ((ClientAdress == null) | (OfDelSell == OfficeOrDelivery.Sam))
            {
                DeliveryPrice = 0;
                ClientAdress = null;
            }
            else DeliveryPrice = 400;

            if ((DeliveryPrice == null) | (DeliveryPrice == 0))
            {
                if (BoxAmount == 0)
                {
                    FinalAmount = 0;
                }
                else FinalAmount = BoxAmount + DeliveryPrice;
            }
            else FinalAmount = BoxAmount + DeliveryPrice;

            if(PayMet == PaymentMethod.Card)
            {
                FinalAmount += (((BoxAmount + DeliveryPrice) * 2) / 100);
            }
        }
    }
}
