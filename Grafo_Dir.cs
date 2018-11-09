using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_Grafos
{
    class Grafo_Dir : Grafo
    {
        public Grafo_Dir(int numeroDeVertices, List<ParOrdenado> listaDePares, string[] vetorRotulos)
        {
            Vertices = new Vertice[numeroDeVertices];

            int control = 1;

            for (int i = 0; i < Vertices.Length; i++)
            {
                if (vetorRotulos[i] == null)
                {
                    vetorRotulos[i] = "SEM RÓTULO " + control;
                    control++;
                }
                Vertices[i] = new Vertice(i, vetorRotulos[i]);
            }


            foreach (ParOrdenado parOrdenado in listaDePares)
                FormarNovaAresta(Vertices[parOrdenado.X].ID, Vertices[parOrdenado.Y].ID, parOrdenado.Pesos, parOrdenado.ListaDeHorarios);
        }

        public Grafo_Dir(int numeroDeVertices, List<ParOrdenado> listaDePares)
        {
            Vertices = new Vertice[numeroDeVertices];

            for (int i = 0; i < Vertices.Length; i++)
                Vertices[i] = new Vertice(i);


            foreach (ParOrdenado parOrdenado in listaDePares)
                FormarNovaAresta(Vertices[parOrdenado.X].ID, Vertices[parOrdenado.Y].ID, parOrdenado.Pesos, parOrdenado.ListaDeHorarios);
        }

        public Grafo_Dir()
        {

        }


        public int getGrauEntrada(Vertice v1)
        {
            return getGrau(v1, -1);
        }

        public int getGrauSaida(Vertice v1)
        {
            return getGrau(v1, 1);
        }

        protected int getGrau(Vertice v1, int dir)
        {
            int grau = 0;

            for (int i = 0; i < Vertices[v1.ID].ListaDeAdjacencia.Count; i++)
            {
                if (Vertices[v1.ID].ListaDeAdjacencia[i].Direcao == dir)
                    grau++;
            }

            return grau;
        }

        public override int getGrau(Vertice v1)
        {
            int grau = 0;
            grau += getGrauEntrada(v1);
            grau += getGrauSaida(v1);

            return grau;
        }


        public override bool isAdjacente(Vertice v1, Vertice v2)
        {
            for (int k = 0; k < Vertices[v1.ID].ListaDeAdjacencia.Count; k++)
            {
                if (Vertices[v1.ID].ListaDeAdjacencia[k].verticeDestino.ID == v2.ID && Vertices[v1.ID].ListaDeAdjacencia[k].Direcao == 1)
                    return true;
            }

            return false;
        }

        public override Grafo getComplementar()
        {
            throw new NotImplementedException();
        }

        public Stack<Vertice> OrdenacaoTopologica()
        {
            Stack<Vertice> pilha = new Stack<Vertice>();
            Vertice[] vetVertices = Vertices;
            OrdenacaoTopologica(vetVertices, 0, 1, pilha);
            return pilha;
        }

        private void OrdenacaoTopologica(Vertice[] vetVertices, int indice, int tempo, Stack<Vertice> pilhaVertices)
        {
            //se o vertice ainda não foi visitaod
            if (vetVertices[indice].EstadoCor == 1)
            {
                //colore de azul e define o tempo de descoberta
                vetVertices[indice].EstadoCor = 2;
                vetVertices[indice].TempoDeDescoberta = tempo;
            }

            //visita toda a lista de adjacencia do vertice atual
            VisitarVerticeOT(vetVertices, indice, ref tempo, pilhaVertices);
        }

        //Visitar vertice por ordenacao topologica
        private void VisitarVerticeOT(Vertice[] vetVertices, int indice, ref int tempo, Stack<Vertice> pilhaVertices)
        {
            //se o vertice ainda não foi pintado de vermelho
            if (vetVertices[indice].EstadoCor != 3)
            {
                //percorre toda a lista de adjacencia do vertice atual
                for (int p = 0; p < vetVertices[indice].ListaDeAdjacencia.Count; p++)
                {
                    //se existe um vertice ainda não visitado
                    if (vetVertices[vetVertices[indice].ListaDeAdjacencia[p].verticeDestino.ID].EstadoCor == 1 && vetVertices[indice].ListaDeAdjacencia[p].Direcao == 1)
                    {
                        tempo++;  //acrescenta uma unidade de tempo
                        int predecessor = indice;  //define o predecessor
                        int novoIndice = vetVertices[indice].ListaDeAdjacencia[p].verticeDestino.ID; //define o proximo indice a ser acessado
                        vetVertices[novoIndice].Predecessor = vetVertices[predecessor]; //define o predecessor do vertice atual
                        OrdenacaoTopologica(vetVertices, novoIndice, tempo, pilhaVertices); // faz uma chamada recursiva implicita
                        return;
                    }
                }

                tempo++;
                vetVertices[indice].EstadoCor = 3; //colore o vertice de vermelho
                vetVertices[indice].TempoDeFinalizacao = tempo; //define o tempo de finalização
                pilhaVertices.Push(vetVertices[indice]); //acrescenta o vertice na pilha
            }

            //se existe um predecessor
            if (vetVertices[indice].Predecessor != null)
            {
                //chamada do método de ordenação com o predecessor do vertice atual
                OrdenacaoTopologica(vetVertices, vetVertices[indice].Predecessor.ID, tempo, pilhaVertices);
            }

            //se não existe predecessor
            else
            {
                //efetua uma busca por vertices em branco
                indice = ExisteVerticesEmBranco();

                //se existe vertices em branco
                if (indice != -1)
                    OrdenacaoTopologica(vetVertices, indice, tempo, pilhaVertices); //chamada do método de ordenação com o indice do vertice em branco
            }
        }

        public override bool isEuleriano()
        {
            throw new NotImplementedException();
        }

        public override string ListaDeAdjacencia()
        {
            string valor = "";

            for (int i = 0; i < Vertices.Length; i++)
            {
                valor += "Aeroporto " + Vertices[i].Rotulo + ": ";

                for (int j = 0; j < Vertices[i].ListaDeAdjacencia.Count; j++)
                {
                    Aresta a = Vertices[i].ListaDeAdjacencia[j];
                    valor += a.verticeDestino.Rotulo + ":" + a.Direcao + ":" + a.Pesos.Distancia + ":" + a.Pesos.DuracaoDoVoo;

                    if (j != Vertices[i].ListaDeAdjacencia.Count - 1)
                        valor += ", ";

                    else
                        valor += "\n \n";
                }
            }

            return valor;
        }

        public Grafo_Dir Kruskal()
        {
            //algoritmo só deve ser executado em grafos conexos
            if (!isConexo()) //se o grafo não é conexo, o algoritmo não será executado e retornará null
                return null;

            //pré processamento dos dados
            List<Aresta> listaArestas = new List<Aresta>();
            List<ParOrdenado> pares = new List<ParOrdenado>();

            Grafo_Dir grafoAuxilir = new Grafo_Dir(Vertices.Length, pares);

            for (int a = 0; a < grafoAuxilir.Vertices.Length; a++)
                grafoAuxilir.Vertices[a].Rotulo = this.Vertices[a].Rotulo;

            for (int g = 0; g < Vertices.Length; g++)
            {
                for (int a = 0; a < Vertices[g].ListaDeAdjacencia.Count; a++)
                {
                    if (Vertices[g].ListaDeAdjacencia[a].Direcao == 1)
                        listaArestas.Add(Vertices[g].ListaDeAdjacencia[a]);
                }
            }

            Aresta[] arestasOrdenadas = insertionSort(listaArestas.ToArray(), 0);
            //dados processados e arestas já ordenadas

            //para cada aresta no vetor de aresta
            for (int v = 0; v < arestasOrdenadas.Length; v++)
            {
                //capturar origem e destino da aresta, ignorando a direção, pois é um grafo não dirigido
                int idOrigem = arestasOrdenadas[v].verticeOrigem.ID;
                int idDestino = arestasOrdenadas[v].verticeDestino.ID;

                //if para ignorar os loops do grafo original
                if (idOrigem != idDestino)
                {
                    //fila auxiliar para executar a busca pelo ciclo
                    Queue<int> fila = new Queue<int>();

                    //verifica se a adição da nova aresta vai gerar um ciclo
                    if (!VerificarCiclo(fila, idOrigem, idDestino, idDestino, grafoAuxilir))
                        grafoAuxilir.FormarNovaAresta(idOrigem, idDestino, arestasOrdenadas[v].Pesos); //não formando ciclo, nova aresta é criada
                }
            }

            return grafoAuxilir;
        }

        private bool VerificarCiclo(Queue<int> fila, int idOrigem, int idDestino, int idAtual, Grafo_Dir grafoAux)
        {
            //MÉTODO RECURSIVO BASEADO NA TRAVESSIA EM AMPLITUDE
            grafoAux.Vertices[idAtual].EstadoCor = 2;

            //para cada item da lista de adjacencia do vértice atual
            for (int w = 0; w < grafoAux.Vertices[idAtual].ListaDeAdjacencia.Count; w++)
            {
                //se a direção é origem -> destino
                if (grafoAux.Vertices[idAtual].ListaDeAdjacencia[w].Direcao == 1)
                {
                    //captura o indice do vertice destino
                    int idLaco = grafoAux.Vertices[idAtual].ListaDeAdjacencia[w].verticeDestino.ID;

                    //se o indice capturado for igual a origem, singnifica que esses componentes ja são conexos
                    //a adição de uma nova aresta, formaria um ciclo, portanto, retorna verdadeiro
                    if (idLaco == idOrigem)
                    {
                        //reseta as cores do grafo auxiliar, para não atrapalhar as próximas execuções do algoritmo
                        grafoAux.ResetarCores();
                        return true;
                    }

                    else
                    {
                        //verifica se o vertice já não foi visitado anteriormente
                        if (grafoAux.Vertices[idLaco].EstadoCor == 1)
                        {
                            fila.Enqueue(idLaco); //enfileira o indice do vertice que está sendo visitado
                            grafoAux.Vertices[idLaco].EstadoCor = 2; //pinta o vertice de azul
                        }
                    }
                }
            }

            grafoAux.Vertices[idAtual].EstadoCor = 3; //pinta o vertice de vermelho

            //condição para chamada recursiva
            //se existem itens na fila, ainda há vertices para verificar a condição de ciclo
            if (fila.Count > 0)
            {
                //remove da fila, detectando o próximo vertice a ser verificado
                int prox = fila.Dequeue();

                //chamada recursiva com parametros atualizados
                return VerificarCiclo(fila, idOrigem, idDestino, prox, grafoAux);
            }

            //else que será executado quando todos os vértices já tiverem sido visitados
            else
            {
                //reseta as cores do grafo auxiliar, para não atrapalhar as próximas execuções do algoritmo
                grafoAux.ResetarCores();
                return false;
            }
        }

        protected override void Dijkstra(int[] vetorDistancias, int[] vetorPredecessor, int atual, int nPeso, Horario horarioAtual)
        {
            int idMenor = -1;

            Vertices[atual].EstadoCor = 3;

            //para todo item da lista de adjacencia, com direção 1
            for (int f = 0; f < Vertices[atual].ListaDeAdjacencia.Count; f++)
            {
                int indexDestino = Vertices[atual].ListaDeAdjacencia[f].verticeDestino.ID;

                //se o vertice ainda não tiver sido fechado
                if (Vertices[indexDestino].EstadoCor == 1 && Vertices[atual].ListaDeAdjacencia[f].Direcao == 1)
                {
                    int dist = 0;

                    if (nPeso == 0)
                    {
                        dist = Vertices[atual].ListaDeAdjacencia[f].Pesos.Distancia + vetorDistancias[atual];
                    }

                    else if (nPeso == 1)
                    {
                        dist = Vertices[atual].ListaDeAdjacencia[f].Pesos.DuracaoDoVoo + vetorDistancias[atual];
                    }

                    else if (nPeso == 3)
                    {
                        int tempoEmEspera = Horario.CalcularTempoEmEspera(Vertices[atual].ListaDeAdjacencia[f].ListaDeVoos, horarioAtual);
                        dist = Vertices[atual].ListaDeAdjacencia[f].Pesos.DuracaoDoVoo + vetorDistancias[atual] + tempoEmEspera;
                        horarioAtual.Minuto += tempoEmEspera;
                    }

                    if (dist < vetorDistancias[indexDestino])
                    {
                        vetorPredecessor[indexDestino] = atual;
                        vetorDistancias[indexDestino] = dist;
                    }
                }
            }

            idMenor = BuscarVerticeAberto(vetorDistancias);

            if (idMenor != -1)
                Dijkstra(vetorDistancias, vetorPredecessor, idMenor, nPeso, horarioAtual);
        }

        protected override void TravessiaEmAplitude(int distancia, int tempo, int atual, Queue<int> fila)
        {
            for (int w = 0; w < this.Vertices[atual].ListaDeAdjacencia.Count; w++)
            {
                int indiceAux = this.Vertices[atual].ListaDeAdjacencia[w].verticeDestino.ID;

                if (this.Vertices[indiceAux].EstadoCor == 1 && this.Vertices[atual].ListaDeAdjacencia[w].Direcao == 1)
                {
                    this.Vertices[indiceAux].EstadoCor = 2;
                    this.Vertices[indiceAux].Predecessor = Vertices[atual];
                    this.Vertices[indiceAux].TempoDeDescoberta = tempo;
                    tempo++;
                    fila.Enqueue(indiceAux);
                }
            }

            this.Vertices[atual].TempoDeFinalizacao = tempo;
            tempo++;

            if (fila.Count > 0)
            {
                int novoIndice = fila.Dequeue();
                this.TravessiaEmAplitude(distancia, tempo, novoIndice, fila);
            }
        }

        public void BuscarUltimoHorario(Horario horarioChegada, int destino, int origem)
        {
          
        }
    }
}
