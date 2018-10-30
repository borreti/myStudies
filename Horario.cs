using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_Grafos
{
    class Horario
    {
        public int Hora { get; set; }
        public int Minuto { get; set; }

        public Horario(int Hora, int Minuto)
        {
            this.Hora = Hora;
            this.Minuto = Minuto;
        }

        public string RetornarHora()
        {
            string value = Hora + ":" + Minuto;
            return value;
        }
    }
}
