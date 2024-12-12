using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1;

namespace Кусовая
{
    public enum _type_vessel // тип судна
    {
        undefined,
        tanker,
        bulker,
        dry_cargo,
        ro_ro,
        ferry,
        container_ships
    }
    public class Vessel
    {
        //поля
        private int _number;
        private string _FN_capitan;
        private _type_vessel _type;
        private int _lifting_capacity;
        private int _year_building;
        private string _photo;
        private string _port_postscripts;
        //свойства
        public string _Number
        {
            get 
            {
                return Convert.ToString(_number);
            }
            set {
                if (!int.TryParse(value, out int i)) throw new ArgumentException("Номер должен быть числом");
                if (Convert.ToInt32(value) < 0) throw new ArgumentException("Номер должен не может быть отрицательным");
                _number = Convert.ToInt32(value);
            }
        }
        public string _FN_Capitan
        {
            get
            {
                return _FN_capitan;
            }
            set
            {
                if (value == "") throw new ArgumentException("ФИО капитана не введено");
                if (value.Split(" ").Count() != 3) throw new ArgumentException("ФИО капитана не корректно");
                else _FN_capitan = value;
            }
        }
        public _type_vessel _Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (value == _type_vessel.undefined) throw new ArgumentException("Тип судна не введен");
                else _type = value;
            }
        }
        public string _Lifting_capacity
        {
            get
            {
                return Convert.ToString(_lifting_capacity);
            }
            set
            {
                if (!int.TryParse(value, out int i)) throw new ArgumentException("Грухоподъемность должна быть числом");
                if (Convert.ToInt32(value) < 0) throw new ArgumentException("Грухоподъемность не может быть отрицательной");
                else _lifting_capacity = Convert.ToInt32(value);
            }
        }
        public string _Year_building
        {
            get
            {
                return Convert.ToString(_year_building);
            }
            set
            {
                if (!int.TryParse(value, out int i)) throw new ArgumentException("Год строительства должен быть числом");
                if (Convert.ToInt32(value) < 1960 || Convert.ToInt32(value) > 2024) throw new ArgumentException("Год строительства не корренктный");
                else _year_building = Convert.ToInt32(value);
            }
        }
        public string _Port_postscripts
        {
            get
            {
                return _port_postscripts;
            }
            set
            {
                if (value == "" || value == "Не установлен") throw new ArgumentException("Порт не введен");
                else _port_postscripts = value;
            }
        }
        public string Photo
        {
            get
            {
                return _photo;
            }
            set
            {
                if (value.ToString() == "") throw new ArgumentException("Фото не выбрано");
                else _photo= value;
            }
        }
        public System.Drawing.Image _Get_Photo
        {
            get { return System.Drawing.Image.FromFile(Photo); }
        }
        // методы
        public Vessel(string _number, string _FN_capitan, _type_vessel _type, string _lifting_capcity, string _year_building, string _photo, string _port_postscripts)
        {
            _Number = _number;
            _FN_Capitan = _FN_capitan;
            _Type = _type;
            _Lifting_capacity = _lifting_capcity;
            _Year_building = _year_building;
            _Port_postscripts = _port_postscripts;
            Photo = _photo;

        }
        public override string ToString()
        {
            return $"{_number} Грузоподъемность: {_lifting_capacity}кг. Порт приписки: {_port_postscripts}";
        }
        public string ToString(char c)
        {
            return $"{_number}{c}{_FN_capitan}{c}{_type}{c}{_lifting_capacity}{c}{_year_building}{c}{_photo}{c}{_port_postscripts}";
        }
    }
}
