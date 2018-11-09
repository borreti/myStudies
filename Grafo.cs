using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_Grafos
{
    abstract class Grafo
    {
        public Vertice[] Vertices;

        protected void FormarNovaAresta(int idV1, int idV2, Peso pesos, List<Horario> listaVoos)
        {
            Aresta i1 = new Aresta(this.Vertices[idV2], this.Vertices[idV1], -1, pesos, listaVoos);
            Aresta i2 = new Aresta(this.Vertices[idV1], this.Vertices[idV2], 1, pesos, listaVoos);
            Vertices[idV1].ListaDeAdjacencia.Add(i2);
            Vertices[idV2].ListaDeAdjacencia.Add(i1);
        }

        protected void FormarNovaAresta(int idV1, int idV2, Peso pesos)
        {
            Aresta i1 = new Aresta(this.Vertices[idV2], this.Vertices[idV1], -1, pesos);
            Aresta i2 = new Aresta(this.Vertices[idV1], this.Vertices[idV2], 1, pesos);
            Vertices[idV1].ListaDeAdjacencia.Add(i2);
            Vertices[idV2].ListaDeAdjacencia.Add(i1);
        }


        protected abstract void Dijkstra(int[] vetorDistancias, int[] vetorPredecessor, int atual, int nPeso, Horario horarioAtual);

        public abstract int getGrau(Vertice v1);

        public abstract bool isAdjacente(Vertice v1, Vertice v2);

        public abstract Grafo getComplementar();

        public abstract bool isEuleriano();

        public abstract string ListaDeAdjacencia();

        protected abstract void TravessiaEmAplitude(int distancia, int tempo, int atual, Queue<int> fila);

        public Vertice SelecionarVertice(int indice)
        {
            return Vertices[indice];
        }

        public bool isNulo()
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                if (Vertices[i].ListaDeAdjacencia.Count > 0)
                    return false;
            }

            return true;
        }

        public bool isPendente(Vertice v1)
        {
            if (getGrau(v1) == 1)
                return true;

            return false;
        }

        public bool isIsolado(Vertice v1)
        {
            if (getGrau(v1) == 0)
                return true;

            return false;
        }

        public bool isRegular()
        {
            int grau = getGrau(Vertices[0]);

            for (int i = 1; i < Vertices.Length; i++)
            {
                if (getGrau(Vertices[i]) != grau)
                    return false;
            }

            return true;
        }

        public void AnularGrafo()
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                int tamanhoLista = Vertices[i].ListaDeAdjacencia.Count;
                for (int j = 0; j < tamanhoLista; j++)
                {
                    Vertices[i].ListaDeAdjacencia.RemoveAt(0);
                }
            }
        }

        protected int ExisteVerticesEmBranco()
        {
            for (int h = 0; h < Vertices.Length; h++)
            {
                if (Vertices[h].EstadoCor == 1)
                    return h;
            }

            return -1;
        }

        public void ResetarCores()
        {
            for (int c = 0; c < Vertices.Length; c++)
                Vertices[c].EstadoCor = 1;
        }

        //ordena arestas por peso
        protected Aresta[] insertionSort(Aresta[] vetor, int nPeso)
        {
            //nPeso = 0 --> distancia
            //npeso = 1 --> duracao do voo em minutos
            int k, y;

            Aresta atual;

            for (k = 1; k < vetor.Length; k++)
            {
                int pesoVet = 0;
                int pesoAtual = 0;

                atual = vetor[k];
                y = k;

                //ordenar pela menor distancia
                if (nPeso == 0)
                {
                    pesoVet = vetor[y - 1].Pesos.Distancia;
                    pesoAtual = atual.Pesos.Distancia;
                }

                //ordenar pela duração do voo
                else if (nPeso == 1)
                {
                    pesoVet = vetor[y - 1].Pesos.DuracaoDoVoo;
                    pesoAtual = atual.Pesos.DuracaoDoVoo;
                }

                while ((y > 0) && (pesoVet > pesoAtual))
                {
                    vetor[y] = vetor[y - 1];

                    if (nPeso == 0)
                        pesoVet = vetor[y - 1].Pesos.Distancia;

                    else if (nPeso == 1)
                        pesoVet = vetor[y - 1].Pesos.DuracaoDoVoo;

                    y = y - 1;
                }

                vetor[y] = atual;
            }

            return vetor;
        }

        //retorna o indice de um vertice aberto com a menor distancia
        protected int BuscarVerticeAberto(int[] vetorDistancias)
        {
            int indice = -1, menorDistancia = int.MaxValue;

            for (int z = 0; z < Vertices.Length; z++)
            {
                if (Vertices[z].EstadoCor == 1)
                {
                    if (vetorDistancias[z] < menorDistancia)
                    {
                        indice = z;
                        menorDistancia = vetorDistancias[z];
                    }
                }
            }

            return indice;
        }

        //retorna uma string com os aeroportos na ordem e a informação do peso total
        public string Dijkstra(int idOrigem, int idDestino, int nPeso)
        {
            /*nPeso = 0 --> Dijkstra por distancia
             *nPeso = 1 --> Dijkstra por tempo total de voo
             *nPeso = 2 --> Travessia em amplitude buscando o menor número de conexões
            */

            ResetarCores();

            int tamVet = Vertices.Length;
            int[] vetorDistancias = new int[tamVet];
            int[] vetorPredecessor = new int[tamVet];

            vetorDistancias[idOrigem] = 0;
            vetorPredecessor[idOrigem] = -1;

            for (int q = 0; q < tamVet; q++)
            {
                if (q != idOrigem)
                {
                    vetorDistancias[q] = int.MaxValue;
                    vetorPredecessor[q] = -1;
                }
            }
            Horario now = new Horario(DateTime.Now.Hour, DateTime.Now.Minute);

            Dijkstra(vetorDistancias, vetorPredecessor, idOrigem, nPeso, now);

            Stack<int> pilhaIndices = new Stack<int>();

            pilhaIndices.Push(idDestino);

            int proxId = idDestino;

            while (proxId != idOrigem && proxId!= -1)
            {
                pilhaIndices.Push(vetorPredecessor[proxId]);
                proxId = vetorPredecessor[proxId];
            }

            List<int> listaIds = new List<int>();

            foreach (int valor in pilhaIndices)
                listaIds.Add(valor);

            string caminho = "";

            foreach (int ex in listaIds)
                caminho += Vertices[ex].Rotulo + ",";

            int tam = caminho.Length;
            caminho = caminho.Substring(0, caminho.Length - 1);

            caminho += "\nPeso total: " + vetorDistancias[idDestino];

            ResetarCores();

            return caminho;
        }

        public void TravessiaEmAplitude(int vInicial)
        {
            ResetarCores();
            Vertices[vInicial].EstadoCor = 2;
            Vertices[vInicial].Distancia = 0;
            Vertices[vInicial].TempoDeDescoberta = 0;
            Vertices[vInicial].Predecessor = null;
            Queue<int> fila = new Queue<int>();
            TravessiaEmAplitude(1, 1, vInicial, fila);
        }

        public bool isConexo()
        {
            TravessiaEmAplitude(0);

            //se houver um vertice que não foi visitado, significa que o grafo não é conexo
            
            for (int e = 0; e < Vertices.Length; e++)
            {
                if (Vertices[e].EstadoCor == 1)
                    return false;
            }

            ResetarCores();
            return true;
        }
    }
}
