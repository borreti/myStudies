using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_Grafos
{
    class Vertice
    {
        public int ID { get; }
        public int EstadoCor { get; set; }
        public int TempoDeDescoberta { get; set; }
        public int TempoDeFinalizacao { get; set; }
        public int Distancia { get; set; }
        public string Rotulo { get; set; }
        public Vertice Predecessor { get; set; }
        public List<Aresta> ListaDeAdjacencia { get; set; }

        public Vertice(int ID)
        {
            this.ID = ID;
            ListaDeAdjacencia = new List<Aresta>();
            EstadoCor = 1;
            TempoDeDescoberta = -1;
            TempoDeFinalizacao = -1;
            Predecessor = null;
        }

        public Vertice(int ID, string Rotulo)
        {
            this.ID = ID;
            this.Rotulo = Rotulo;
            ListaDeAdjacencia = new List<Aresta>();
            EstadoCor = 1;
            TempoDeDescoberta = -1;
            TempoDeFinalizacao = -1;
            Predecessor = null;
        }
    }
}
