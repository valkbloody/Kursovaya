using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Кусовая;

namespace WinFormsApp1
{
    static class Data
    {
        public static List<Client> base_clients = new List<Client> { };
        public static List<VIP_CLIENT>VIP_base_clients = new List<VIP_CLIENT> { };
        public static List<Consignment> base_cons = new List<Consignment> { };
        public static List<Vessel> base_vessels = new List<Vessel> { };
        public static List<string> base_ports = new List<string> { "Мурманск", "Архангельск", "Мезень", "Певек", "Владивосток", "Магадан" };
        public static List<string> base_paths = new List<string> { };
        public static List<Flight> base_flights = new List<Flight> { };
    }
}
