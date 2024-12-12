using Кусовая;

namespace WinFormsApp1
{
    public enum _state_subs // состояние подписки
    {
        undefined,
        expired,
        paid,
    }
    public class VIP_CLIENT : Client
    {
        // поля
        _state_subs _state;
        int _price_subs;
        int _amount_points;
        // свойства
        public string Price_subs
        {
            get { return Convert.ToString(_price_subs); }
            set
            {
                if (!int.TryParse(value, out int i)) throw new ArgumentException("Цена подписки должна быть числом");
                if (Convert.ToInt32(value) < 0) throw new ArgumentException("Цена подписки не может быть отрицательной");
                if (Convert.ToInt32(value) > 100000) throw new ArgumentException("Цена подписки должна быть меньше 100000 р.");
                else _price_subs = Convert.ToInt32(value);
            }
        }
        public string Amount_points
        {
            get { return Convert.ToString(_amount_points); }
            set
            {
                if (!int.TryParse(value, out int i)) throw new ArgumentException("Колиечество баллов должно быть числом");
                if (Convert.ToInt32(value) < 0) throw new ArgumentException("Колиечество баллов не может быть отрицательной");
                if (Convert.ToInt32(value) > 1000) throw new ArgumentException("Колиечество баллов должен быть меньше 1000 р.");
                else _amount_points = Convert.ToInt32(value);
            }
        }
        public _state_subs State
        {
            get { return _state; }
            set
            {
                if (value == _state_subs.undefined) throw new ArgumentException("Статус подписки не уставновлен");
                else _state = value;
            }
        }
        // методы
        public VIP_CLIENT(string _name_client, string _INN, string _bank, string _address, _state_subs state, string price_subs, string amount_points) : base(_name_client, _INN, _bank, _address)
        {
            State = state;
            Price_subs = price_subs;
            Amount_points = amount_points;
        }
        public int Payment_On_Next_Month() // рассчет платы за подписку на следующий месяц
        {
            if (_state == _state_subs.paid)
            {
                if (_price_subs - _amount_points <= 0) return 1;
                else return _price_subs - _amount_points;
            }
            else
            {
                return _price_subs;
            }

        }
        public override string ToString(char c)
        {
            return $"{base.ToString('#')}{c}{_state}{c}{_price_subs}{c}{_amount_points}";
        }

    }
}
