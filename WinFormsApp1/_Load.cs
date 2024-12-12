using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Кусовая;

namespace WinFormsApp1
{
    public enum __measurement // единицы измерения
    {
        kg,
        t,
        kuntal,
        undefined
    }
    public class Load
    {
        // поля
        private int _number;
        private string _name;
        private int _declared_weight;
        private __measurement _measurement;
        private int _insured_weight;
        // свойства
        public string _Number
        {
            get
            {
                return Convert.ToString(_number);
            }
            set
            {
                if (!int.TryParse(value, out int i)) throw new ArgumentException("Номер груза должен быть числом");
                if (Convert.ToInt32(value) <= 0) throw new ArgumentException("Номер груза должен быть больше 0");
                else _number = Convert.ToInt32(value);
            }
        }
        public string _Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value == "") throw new ArgumentException("Название груза не введено");
                else _name = value;
            }
        }
        public string _Declared_weight
        {
            get
            {
                return Convert.ToString(_declared_weight);
            }
            set
            {
                if (!int.TryParse(value, out int i)) throw new ArgumentException("Заявленный вес должен быть числом");
                if (Convert.ToInt32(value) <= 0) throw new ArgumentException("Заявленный вес должен быть больше 0");
                else _declared_weight = Convert.ToInt32(value);
            }
        }
        public __measurement _Measurement
        {
            get
            {
                return _measurement;
            }
            set
            {
                if (value == __measurement.undefined) throw new ArgumentException("Единица измерений не введена");
                else _measurement = value;
            }
        }
        public string _Insured_weight
        {
            get
            {
                return Convert.ToString(_insured_weight);
            }
            set
            {
                if (!int.TryParse(value, out int i)) throw new ArgumentException("Застрахованный вес должен быть числом");
                if (Convert.ToInt32(value) <= 0) throw new ArgumentException("Застрахованный вес должен быть больше 0");
                if (Convert.ToInt32(value) > _declared_weight) throw new ArgumentException("Застрахованный вес не может быть меньше заявленного");
                else _insured_weight = Convert.ToInt32(value);
            }
        }
        //методы
        public Load(string _number, string _name, string _declared_weight, string _measurement, string _insured_weight)
        {
            _Number = _number;
            _Name = _name;
            _Declared_weight = _declared_weight;
            _Insured_weight = _insured_weight;
            if (_measurement == "kg") _Measurement = __measurement.kg;
            else if (_measurement == "kuntal") _Measurement = __measurement.kuntal;
            else if (_measurement == "t") _Measurement = __measurement.t;
            else _Measurement = __measurement.undefined;
        }
        public string ToString(char c)
        {
            return $"{_number}{c}{_name}{c}{_declared_weight}{c}{_measurement}{c}{_insured_weight}";
        }
        public string ToString_For_Cons()
        {
            return $"{_number} {_name}\n";
        }
    }
}
