using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_Grafos
{
    class Peso
    {
        public int Distancia { get; set; }
        public int DuracaoDoVoo { get; set; }//é tratado em minutos por ser mais fácil de comparar valores

        public Peso(int Distancia, int DuracaoDoVoo)
        {
            this.Distancia = Distancia;
            this.DuracaoDoVoo = DuracaoDoVoo;
        }
    }
}
