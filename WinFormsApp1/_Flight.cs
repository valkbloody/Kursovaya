using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Кусовая;

namespace WinFormsApp1
{
    public class Flight
    {
        // поля
        private Vessel _vessel;
        private string _path;
        private List<Consignment> _consigmnents;
        // свойства
        public Vessel GetVessel
        {
           get { return _vessel; }
        }
        public string Path
        {
            get { return _path; }
        }
        public List<Consignment> Cons
        {
            get { return _consigmnents; }
            set
            {
                if (value == null) _consigmnents = new List<Consignment> { };
                else _consigmnents = value;
            }
        }
        // методы
        public Flight(Vessel vessel, string path)
        {
            _vessel = vessel;
            _path = path;
            Cons = _consigmnents;
        }
        public string ToString(char c)
        {

            string res;
            res = "/Flight/\n";
            res = res +  _vessel.ToString(c) + "\n";
            res = res + Path;
            foreach(Consignment cons in Cons)
            {
                res = res + "\n" + cons.ToString(c) ;
            }
            return res;
        }
        public void Add_Cons(List<Consignment> add_cons)
        {
            foreach (Consignment add_con in add_cons)
                _consigmnents.Add(add_con);
        }
        public string ToStringPort(string port) 
        // выводит какие грузы были загружены и выгружены в порту
        {

            string res;
            res = $"Порт {port}\n";
            res = res + "Загружено(номера):\n";
            foreach (Consignment cons in _consigmnents)
            {
            if (cons._Place_dispatch == port)  res = res + cons._Сons_number + ";\n";
            }
            res = res + "Выгружено(номера):\n";
            foreach (Consignment cons in _consigmnents)
            {
                if (cons._Place_arrivel == port) res = res + cons._Сons_number + ";\n";
            }
            return res;
        }
        public string[] ToPortsPath()
        {
            string res = "";
            foreach(string port in _path.Split(" "))
            {
                if (port != "-")
                res = res + "#" +ToStringPort(port);
            }
            return res.Split("#");
        }
    }
}
