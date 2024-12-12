using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Кусовая;

namespace WinFormsApp1
{
    public class Consignment
    {
        // поля
        private string _сons_number;
        private int _declaration_number;
        private Client _sender;
        private Client _receiver;
        private string _place_dispatch;
        private string _place_arrivel;
        private DateTime _dispatch_date;
        private DateTime _arrivel_date;
        private List <Load> _content;
        // свойства
        public string _Сons_number
        {
            get
            {
                return _сons_number;
            }
            set
            {
              string[] str = value.Split('/');
                if (str.Count()!=2) throw new ArgumentException("Номер партии не корректен");
                foreach (string s in str)
                {
                    if (!int.TryParse(s, out int i)) throw new ArgumentException("Номер партии должен быть числом");
                    if (Convert.ToInt32(s) <= 0) throw new ArgumentException("Номер партии должен быть положительным");
                }
                _сons_number = value;
            }
        }
        public string _Declaration_number
        {
            get
            {
                return Convert.ToString(_declaration_number);
            }
            set
            {
                if (!int.TryParse(value, out int i)) throw new ArgumentException("Номер декларации должен быть числом");
                if (Convert.ToInt32(value) < 0) throw new ArgumentException("Номер декларации не может быть отрицательным");
                else _declaration_number = Convert.ToInt32(value);
            }
        }
        public DateTime _Dispatch_date
        {
            get
            {
                return _dispatch_date;
            }
            set
            {
                DateTime date = new DateTime();
                if (value == date) throw new ArgumentException("Дата отправки не введена");
                else  _dispatch_date = value;
            }
        }
        public Client Reciever
        {
            get { return _receiver; }
            set
            {
                if (value._INN_ == _sender._INN_) throw new ArgumentException("Отправитель не должен совпадать с получателем");
                else _receiver = value;
            }
        }
        public Client Sender
        {
            get { return _sender; }
            set
            {
                if (value._INN_ == _receiver._INN_) throw new ArgumentException("Отправитель не должен совпадать с получателем");
                else _sender = value;
            }
        }
        public DateTime _Arrivel_date
        {
            get
            {
                return _arrivel_date;
            }
            set
            {
                DateTime date = new DateTime();
                if (value == date) throw new ArgumentException("Дата прибытия не введена");
                else if (_Dispatch_date >= value) throw new ArgumentException("Дата отправки не должна быть позже даты доставки");
                else _arrivel_date = value;
            }
        }
        public string _Place_dispatch
        {
            get
            {
                return _place_dispatch;
            }
            set
            {
                if (value == "") throw new ArgumentException("Место отправки не введено");
                else _place_dispatch = value;
            }
        }
        public string _Place_arrivel
        {
            get
            {
                return _place_arrivel;
            }
            set
            {
                if (value == "") throw new ArgumentException("Место прибытия не введено");
                if (value == _Place_dispatch) throw new ArgumentException("Место прибытия не должно совпадать с местом отправки");
                else _place_arrivel = value;
            }
        }
        public List<Load> _Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (value.Count == 0) throw new ArgumentException("Груз не выбран");
                else _content = value;
            }
        }
        // методы
        public Consignment(string _сons_number, string _declaration_number, Client sender, Client receiver, string _place_dispatch,string _place_arrivel,DateTime _dispatch_date, DateTime _arrivel_date, List<Load> _load)
        {
            _Сons_number = _сons_number;
            _Declaration_number = _declaration_number;
            _Dispatch_date = _dispatch_date;
            _Arrivel_date = _arrivel_date;
            _Place_dispatch = _place_dispatch;
            _Place_arrivel = _place_arrivel;
            _Content = _load;
            _sender = sender;
            Reciever = receiver;
            _content = _load;
        }
        public Consignment(string _сons_number, string _declaration_number, string sender, string receiver, string _place_dispatch, string _place_arrivel, DateTime _dispatch_date, DateTime _arrivel_date, List<Load> _load)
        {
            _Сons_number = _сons_number;
            _Declaration_number = _declaration_number;
            _Dispatch_date = _dispatch_date;
            _Arrivel_date = _arrivel_date;
            _Place_dispatch = _place_dispatch;
            _Place_arrivel = _place_arrivel;
            _Content = _load;
            Client sende = new Client(sender);
            Client _rec = new Client(receiver);
            _sender = sende;
            Reciever = _rec;
            _content = _load;
        }
        public int GetWieght() // рассчет веса партии
        {
            int weight = 0;
            if (_content == null) throw new Exception("Не добавлено ни одного типа груза");
            foreach (Load load in _content)
            {
                if (load._Measurement == __measurement.t)
                    weight = Convert.ToInt32(load._Insured_weight) * 1000;
                else if (load._Measurement == __measurement.kuntal)
                    weight = Convert.ToInt32(load._Insured_weight) * 100;
                else weight = Convert.ToInt32(load._Insured_weight);
            }
            return weight;
        }
        public string ToString(char c)
        {
            string res = "/Loads/\n";
            foreach (Load load in _content)
            {
                  res = res + load.ToString('#') + "\n";
            }
            res = res +  "/Consigment/\n";
            res= res +  $"{_сons_number}{c}{_declaration_number}{c}{_receiver.ToString_Cons()}{c}{_sender.ToString_Cons()}{c}{_dispatch_date.ToShortDateString()}{c}{_arrivel_date.ToShortDateString()}{c}{_place_dispatch}{c}{_place_arrivel}";
            return res;
        }
        public string ToStringLoads()
        {
            string res="";
            foreach(Load load in _content)
            {
                res = res + load.ToString_For_Cons();
            }
            return res;
        }
        public string ToStringInRaces()
        {
            return $"Номер партии: {_сons_number} ; Место отправки: {_place_dispatch} ; Место доставки: {_place_arrivel} ; Вес партии: {GetWieght()} кг. ";
        }
    }
}

