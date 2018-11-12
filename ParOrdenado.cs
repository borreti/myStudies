using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_Grafos
{
    //Par ordenado é uma classe que será utilizada para montar as arestas
    //X representa a linha e Y a coluna
    class ParOrdenado
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Peso Pesos { get; set; }
        public List<DateTime> ListaDeHorarios;

        public ParOrdenado(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public ParOrdenado(int X, int Y, Peso Pesos)
        {
            this.Pesos = Pesos;
            this.X = X;
            this.Y = Y;
        }

        public ParOrdenado(int X, int Y, Peso Pesos, List<DateTime> ListaDeHorarios)
        {
            this.Pesos = Pesos;
            this.X = X;
            this.Y = Y;
            this.ListaDeHorarios = ListaDeHorarios;
        }
    }
}
