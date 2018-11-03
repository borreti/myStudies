using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_Grafos
{
    class Aresta
    {
        public Vertice verticeOrigem { get; set; }
        public Vertice verticeDestino { get; set; }
        public int Direcao { get; set; }
        public Peso Pesos { get; set; }
        public int ID { get; set; }
        public List<Horario> ListaDeVoos { get; set; }
        public List<Horario> PrevisaoChegada { get; set; }

        public Aresta(Vertice verticeOrigem, Vertice verticeConectado, int Direcao, Peso Pesos)
        {
            this.verticeOrigem = verticeOrigem;
            this.verticeDestino = verticeConectado;
            this.Direcao = Direcao;
            this.Pesos = Pesos;
        }

        public Aresta(Vertice verticeOrigem, Vertice verticeConectado, int Direcao, Peso Pesos, List<Horario> ListaDeVoos)
        {
            this.verticeOrigem = verticeOrigem;
            this.verticeDestino = verticeConectado;
            this.Direcao = Direcao;
            this.Pesos = Pesos;
            this.ListaDeVoos = ListaDeVoos;

            foreach (Horario item in ListaDeVoos)
            {
                Horario horario = item;
                horario.Minuto += Pesos.DuracaoDoVoo;
            }
        }
    }
}
