using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace Кусовая
{
    public class Client
    {
        // поля
        private string _name_client;
        private string _INN;
        private string _bank;
        private string _address;
        // свойства
        public string _Name_client
        {
            get
            {
                return _name_client;
            }
            set
            {
                if (value == "") throw new ArgumentException("Имя клиента не введено");
                else _name_client = value;
            }
        }
        public string _Address
        {
            get
            {
                return _address;
            }
            set
            {
                if (value == "") throw new ArgumentException("Адрес клиента не введен");
                else _address = value;
            }
        }
        public string _INN_
        {
            get
            {
                return _INN;
            }
            set
            {
                for (int i = 0; i < value.Length; i++) if (!char.IsDigit(value[i])) throw new ArgumentException("ИНН не корректен");
                if (Convert.ToInt64(value) <= 1 || value.Length != 10) throw new ArgumentException("ИНН не корректен");
                else _INN = value;
            }
        }
        public string _Bank
        {
            get
            {
                return _bank;
            }
            set
            {
                if (value == "") throw new ArgumentException("Банк клиента не введен");
                else _bank = value;
            }
        }
        //методы
        public Client(string _name_client, string _INN, string _bank, string _address)
        {
            _Name_client = _name_client;
            _INN_ = _INN;
            _Bank = _bank;
            _Address = _address;
        }
        public Client(string _name)
        {
            if (Data.base_clients.Count == 0 && Data.VIP_base_clients.Count == 0) throw new ArgumentException("Клиентов нет");
            string[] name = _name.Split(" ");
            foreach (Client client in Data.base_clients)
            {
                if (client._Name_client == name[0] && client._Address == name[1])
                {
                    _Name_client = client._name_client;
                    _INN_ = client._INN;
                    _Bank = client._bank;
                    _Address = client._address;
                    break;
                }
            }
            foreach (Client client in Data.VIP_base_clients)
            {
                if (client._Name_client == name[0])
                {
                    _Name_client = client._name_client;
                    _INN_ = client._INN;
                    _Bank = client._bank;
                    _Address = client._address;
                    break;
                }
            }
        }
        public virtual string ToString(char c)
        {
            return $"{_name_client}{c}{_INN}{c}{_bank}{c}{_Address}";
        }
        public string ToString_Cons()
        {
            return $"{_Name_client} {_Address}";
        }
    }
}
