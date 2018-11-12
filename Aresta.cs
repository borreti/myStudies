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
        public List<DateTime> ListaDeVoos { get; set; }
        public List<DateTime> PrevisaoChegada { get; set; }

        public Aresta(Vertice verticeOrigem, Vertice verticeConectado, int Direcao, Peso Pesos)
        {
            this.verticeOrigem = verticeOrigem;
            this.verticeDestino = verticeConectado;
            this.Direcao = Direcao;
            this.Pesos = Pesos;
        }

        public Aresta(Vertice verticeOrigem, Vertice verticeConectado, int Direcao, Peso Pesos, List<DateTime> ListaDeVoos)
        {
            this.verticeOrigem = verticeOrigem;
            this.verticeDestino = verticeConectado;
            this.Direcao = Direcao;
            this.Pesos = Pesos;
            this.ListaDeVoos = ListaDeVoos;

            this.ListaDeVoos.Sort();

            this.PrevisaoChegada = new List<DateTime>();

            for (int e = 0; e < ListaDeVoos.Count; e++)
            {
                PrevisaoChegada.Add(this.ListaDeVoos[e]);
                PrevisaoChegada[e] = PrevisaoChegada[e].AddMinutes(Pesos.DuracaoDoVoo);
            }
        }
    }
}
