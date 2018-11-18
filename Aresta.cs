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
        public int Direcao { get; set; }//se a direção for 1, o vertice parte da origem para o destino, se for -1, destino para a origem
        public Peso Pesos { get; set; }//pesos de duração do voo e distancia
        public int ID { get; set; }
        public List<DateTime> ListaDeVoos { get; set; } //lista com todos os horarios de voo
        public List<DateTime> PrevisaoChegada { get; set; } //lista com todas as previsões de chegada


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

            this.ListaDeVoos.Sort(); //ordena os horários de voo

            PrevisaoChegada = new List<DateTime>();

            //for que gera a previsão de chegada, dando o horario inicial, mais o tempo de viagem em minutos
            for (int e = 0; e < ListaDeVoos.Count; e++)
            {
                PrevisaoChegada.Add(this.ListaDeVoos[e]);
                PrevisaoChegada[e] = PrevisaoChegada[e].AddMinutes(Pesos.DuracaoDoVoo);
            }
        }
    }
}
