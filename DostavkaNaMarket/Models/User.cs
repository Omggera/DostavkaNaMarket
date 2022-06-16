using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

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
        public string ClientName { get; set; }

        [Required(ErrorMessage = "Поле Номер телефона обязательно для заполнения")]
        [MaxLength(10, ErrorMessage = "Номер телефона должен быть в формате 9011418686")]
        [DataType(DataType.PhoneNumber)]
        public string ClientPhone { get; set; }

        public string ClientMail { get; set; }

        public enum OfficeOrDelivery
        {
            Sam,
            Vivoz,
        }
        public OfficeOrDelivery? OfDelSell { get; set; } = OfficeOrDelivery.Sam;
        public Dictionary<string, object> InputAttributes { get; set; }
        
        public string ClientAdress { get; set; }
        

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
    }
}
