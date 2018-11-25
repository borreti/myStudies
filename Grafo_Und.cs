using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_Grafos
{
    class Grafo_Und : Grafo
    {
        //construtor com rotulos
        public Grafo_Und(int numeroDeVertices, List<ParOrdenado> listaDePares, string[] vetorRotulos)
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

        //construtor sem rotulos
        public Grafo_Und(int numeroDeVertices, List<ParOrdenado> listaDePares)
        {
            Vertices = new Vertice[numeroDeVertices];

            for (int i = 0; i < Vertices.Length; i++)
                Vertices[i] = new Vertice(i);


            foreach (ParOrdenado parOrdenado in listaDePares)
                FormarNovaAresta(Vertices[parOrdenado.X].ID, Vertices[parOrdenado.Y].ID, parOrdenado.Pesos, parOrdenado.ListaDeHorarios);
        }

        public Grafo_Und(int numeroDeVertices)
        {
            Vertices = new Vertice[numeroDeVertices];

            for (int i = 0; i < Vertices.Length; i++)
                Vertices[i] = new Vertice(i);
        }


        public override bool isAdjacente(Vertice v1, Vertice v2)
        {
            //para toda aresta do vertice v1
            for (int k = 0; k < Vertices[v1.ID].ListaDeAdjacencia.Count; k++)
            {
                //verifica se algum dos itens adjacentes é igual ao v2
                if (Vertices[v1.ID].ListaDeAdjacencia[k].verticeDestino.ID == v2.ID)
                    return true;
            }

            return false;
        }

        public override int getGrau(Vertice v1)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                if (v1 == Vertices[i])
                    return Vertices[i].ListaDeAdjacencia.Count;
            }

            return -1;
        }

        public bool isCompleto()
        {
            return isCompleto(0);
        }

        private bool isCompleto(int indice)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                if (Vertices[i] != Vertices[indice])
                {
                    if (!isAdjacente(Vertices[indice], Vertices[i]))
                    {
                        return false;
                    }
                }
            }

            indice++;

            if (indice < Vertices.Length)
                return isCompleto(indice);
            else
                return true;
        }

        public override Grafo getComplementar()
        {
            Grafo_Und grafo;
            int nVertices;
            List<ParOrdenado> pares = new List<ParOrdenado>();


            for (int i = 0; i < Vertices.Length; i++)
            {
                for (int j = 0; j < Vertices.Length; j++)
                {
                    if (i != j)
                    {
                        if (!isAdjacente(Vertices[i], Vertices[j]))
                        {
                            if (!VerificarParExistente(Vertices[i].ID, Vertices[j].ID, pares))
                            {
                                ParOrdenado par = new ParOrdenado(Vertices[i].ID, Vertices[j].ID);
                                pares.Add(par);
                            }
                        }
                    }
                }
            }

            nVertices = Vertices.Length;
            return grafo = new Grafo_Und(nVertices, pares);
        }

        //evita pares ordenados iguais, evitando duplicação de arestas
        private bool VerificarParExistente(int x, int y, List<ParOrdenado> lista)
        {
            for (int h = 0; h < lista.Count; h++)
            {
                if ((lista[h].X == x && lista[h].Y == y) || (lista[h].Y == x && lista[h].X == y))
                    return true;
            }

            return false;
        }

        public override bool isEuleriano()
        {
            if (!isConexo())
                return false;

            for (int i = 0; i < Vertices.Length; i++)
            {
                if (getGrau(Vertices[i]) % 2 != 0)
                    return false;
            }

            return true;
        }


        public override string ListaDeAdjacencia()
        {
            string valor = "";

            for (int i = 0; i < Vertices.Length; i++)
            {
                valor += "Aeroporto " + Vertices[i].Rotulo + ":";

                for (int j = 0; j < Vertices[i].ListaDeAdjacencia.Count; j++)
                {
                    Aresta a = Vertices[i].ListaDeAdjacencia[j];

                    valor += (a.verticeDestino.Rotulo) + ":" + a.Pesos.Distancia;

                    if (j != Vertices[i].ListaDeAdjacencia.Count - 1)
                        valor += ", ";

                    else
                        valor += "\n \n";
                }
            }

            return valor;
        }

        private Grafo_Und ArvoreMinima()
        {
            //algoritmo só deve ser executado em grafos conexos
            if (!isConexo()) //se o grafo não é conexo, o algoritmo não será executado e retornará null
                return null;

            //pré processamento dos dados
            List<Aresta> listaArestas = new List<Aresta>();
            List<ParOrdenado> pares = new List<ParOrdenado>();
            Grafo_Und grafoAuxilir = new Grafo_Und(Vertices.Length, pares);

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

        public List<Grafo_Und> Kruskal()
        {
            //lista dos componentes conexos
            List<List<Vertice>> componentesConexos = TravessiaEmAplitude(0);
            List<Grafo_Und> listaGrafos = new List<Grafo_Und>();

            for (int b = 0; b < componentesConexos.Count; b++)
            {
                Vertice[] vetorVertices = new Vertice[componentesConexos[b].Count];
                List<Vertice> listaV = new List<Vertice>();
                Grafo_Und g;
                Dictionary<int, int> vDictionary = new Dictionary<int, int>();

                for (int k = 0; k < vetorVertices.Length; k++)
                    vetorVertices[k] = new Vertice(k);

                for (int c = 0; c < componentesConexos[b].Count; c++)
                {
                    listaV.Add(componentesConexos[b][c]);
                    vetorVertices[c].ID = componentesConexos[b][c].ID;
                    vetorVertices[c].Rotulo = componentesConexos[b][c].Rotulo;

                    vDictionary.Add(vetorVertices[c].ID, vetorVertices[c].ID);
                }

                g = new Grafo_Und(vetorVertices.Length);
                g.Vertices = vetorVertices;

                for (int k = 0; k < componentesConexos[b].Count; k++)
                {
                    List<Aresta> lAresta = componentesConexos[b][k].ListaDeAdjacencia;
                    for (int c = 0; c < lAresta.Count; c++)
                    {
                        try
                        {
                            int v1 = vDictionary[lAresta[c].verticeOrigem.ID];
                            int v2 = vDictionary[lAresta[c].verticeDestino.ID];

                            g.FormarNovaAresta(v1, v2, lAresta[c].Pesos);
                        }

                        catch (KeyNotFoundException)
                        {
                            continue;
                        }
                    }
                }

                g.CorrigiirIndices();
                listaGrafos.Add(g);
            }

            List<Grafo_Und> floresta = new List<Grafo_Und>();

            for (int ac = 0; ac < listaGrafos.Count; ac++)
                floresta.Add(listaGrafos[ac].ArvoreMinima());

            return floresta;
        }

        private bool VerificarCiclo(Queue<int> fila, int idOrigem, int idDestino, int idAtual, Grafo_Und grafoAux)
        {
            //MÉTODO RECURSIVO BASEADO NA TRAVESSIA EM AMPLITUDE
            grafoAux.Vertices[idAtual].EstadoCor = 2;

            //para cada item da lista de adjacencia do vértice atual
            for (int w = 0; w < grafoAux.Vertices[idAtual].ListaDeAdjacencia.Count; w++)
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

        protected override void Dijkstra(int[] vetorDistancias, int[] vetorPredecessor, int atual, int nPeso)
        {
            int idMenor = -1;

            Vertices[atual].EstadoCor = 3;

            //para todo item da lista de adjacencia, com direção 1
            for (int f = 0; f < Vertices[atual].ListaDeAdjacencia.Count; f++)
            {
                int indexDestino = Vertices[atual].ListaDeAdjacencia[f].verticeDestino.ID;

                if (Vertices[indexDestino].EstadoCor == 1)
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
                        int pred = vetorPredecessor[atual];
                        int tempoEmEsperaM = 0;
                        TimeSpan tempoEmEspera;

                        if (pred != -1)
                        {
                            Aresta voo = BuscarAresta(pred, atual);
                            DateTime horaAtual = voo.ListaDeVoos[0];
                            horaAtual.AddMinutes(voo.Pesos.DuracaoDoVoo);

                            int indiceHorario = -1;
                            DateTime horarioEscolhido = Vertices[atual].ListaDeAdjacencia[f].ListaDeVoos[0];

                            //for que procura um horário igual ou maior que o horário atual
                            for (int x = 0; x < Vertices[atual].ListaDeAdjacencia[f].ListaDeVoos.Count; x++)
                            {
                                if (Vertices[atual].ListaDeAdjacencia[f].ListaDeVoos[x] >= horaAtual && Vertices[atual].ListaDeAdjacencia[f].ListaDeVoos[x] < horarioEscolhido)
                                {
                                    horarioEscolhido = Vertices[atual].ListaDeAdjacencia[f].ListaDeVoos[x];
                                    indiceHorario = x;
                                }
                            }

                            if (indiceHorario != -1)
                            {
                                tempoEmEspera = Vertices[atual].ListaDeAdjacencia[f].ListaDeVoos[indiceHorario] - horaAtual;
                            }

                            else
                            {
                                horaAtual = horaAtual.AddDays(1);
                                tempoEmEspera = horaAtual - Vertices[atual].ListaDeAdjacencia[f].ListaDeVoos[0];
                            }

                            tempoEmEsperaM = (tempoEmEspera.Hours * 60) + tempoEmEspera.Minutes;
                            dist = Vertices[atual].ListaDeAdjacencia[f].Pesos.DuracaoDoVoo + vetorDistancias[atual] + tempoEmEsperaM;
                        }
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
                Dijkstra(vetorDistancias, vetorPredecessor, idMenor, nPeso);
        }

        public Aresta BuscarAresta(int v1, int v2)
        {
            for (int w = 0; w < Vertices[v1].ListaDeAdjacencia.Count; w++)
            {
                if (Vertices[v1].ListaDeAdjacencia[w].verticeDestino == Vertices[v2])
                    return Vertices[v1].ListaDeAdjacencia[w];
            }

            return null;
        }

        protected override List<List<Vertice>> TravessiaEmAplitude(int distancia, int tempo, int atual, Queue<int> fila)
        {
            List<List<Vertice>> componentes = new List<List<Vertice>>();
            List<Vertice> c = new List<Vertice>();
            TravessiaEmAplitude(distancia, tempo, atual, fila, 0,c, componentes);
            return componentes;
        }

        private void TravessiaEmAplitude(int distancia, int tempo, int atual, Queue<int> fila, int nComponente, List<Vertice> listaV, List<List<Vertice>> componentes)
        {
            listaV.Add(Vertices[atual]);

            for (int w = 0; w < Vertices[atual].ListaDeAdjacencia.Count; w++)
            {
                int indice = Vertices[atual].ListaDeAdjacencia[w].verticeDestino.ID;

                if (Vertices[indice].EstadoCor == 1)
                {
                    Vertices[indice].EstadoCor = 2;
                    Vertices[indice].Predecessor = Vertices[atual];
                    Vertices[indice].TempoDeDescoberta = tempo;
                    tempo++;
                    fila.Enqueue(indice);
                }
            }

            Vertices[atual].TempoDeFinalizacao = tempo;
            Vertices[atual].EstadoCor = 3;
            tempo++;

            int novoIndice;

            if (fila.Count > 0)
            {
                novoIndice = fila.Dequeue();
                TravessiaEmAplitude(distancia + 1, tempo, novoIndice, fila, nComponente, listaV, componentes);
            }

            else
            {

                novoIndice = ExisteVerticesEmBranco();
                componentes.Add(listaV);

                listaV = new List<Vertice>();

                if (novoIndice != -1)
                    TravessiaEmAplitude(distancia + 1, tempo, novoIndice, fila, nComponente + 1, listaV, componentes);
            }
        }

        public string ComponentesConexos()
        {
            ResetarCores();
            string val = "Conjunto 1: ";
            Queue<int> filaVertices = new Queue<int>();
            List<int> listaConjunto = new List<int>();
            Vertices[0].EstadoCor = 2;
            val = ComponentesConexos(0, val, 1, filaVertices, listaConjunto);
            return val;
        }

        private string ComponentesConexos(int atual, string valor, int nConjuntos, Queue<int> idVertices, List<int> listaConjunto)
        {
            valor += Vertices[atual].Rotulo + ", ";
            listaConjunto.Add(atual);

            //loop para visitar todos os vertices adjacentes
            for (int w = 0; w < Vertices[atual].ListaDeAdjacencia.Count; w++)
            {
                //captura o id para localizar o vertice
                int indiceVet = Vertices[atual].ListaDeAdjacencia[w].verticeDestino.ID;

                //verifica se ainda não foi visitado
                if (Vertices[indiceVet].EstadoCor == 1)
                {
                    //colore de azul
                    Vertices[indiceVet].EstadoCor = 2;
                    idVertices.Enqueue(indiceVet); //enfileira
                }
            }

            //apos o for, todos os adjacentes foram visitados
            Vertices[atual].EstadoCor = 3;

            int novoIndice = 0;

            //se há itens na fila
            if (idVertices.Count > 0)
            {
                //havendo itens na fila, é removido o primeiro item para fazer a chamada recursiva atualizando o indice do vertice que será acessado
                novoIndice = idVertices.Dequeue();
                return ComponentesConexos(novoIndice, valor, nConjuntos, idVertices, listaConjunto);
            }

            //o bloco de codigo abaixo, ocorre quando o primeiro componente conexo chegou ao seu fim
            else
            {
                //busca um vertice em branco
                novoIndice = ExisteVerticesEmBranco();
                string auxVal = "";

                //se o novoIndice for diferente de -1, é porque existe
                if (novoIndice != -1)
                {
                    //agora verifica os componentes que se removidos, acabam com a conectivdade desse sub conjunto
                    valor += "\nAeroportos que separadamente, se removidos, interrompem a conectividade do conjunto " + nConjuntos + ": ";
                    //para todos os itens do conjunto
                    for(int q = 0; q < listaConjunto.Count; q++)
                    {
                        if (isPendente(Vertices[listaConjunto[q]])) //verifica aqueles que possuem grau 1
                            auxVal += Vertices[listaConjunto[q]].Rotulo + ", "; //todos que possuem grau 1, são incluidos na string
                    }

                    if (auxVal == "")
                        valor += "Não há aeroportos que separadamente, se removidos, interrompem a conectividade";

                    else
                        valor += auxVal;

                    nConjuntos++;
                    listaConjunto = new List<int>(); //reseta a lista de conjuntos
                    valor += "\n\nConjunto " + nConjuntos + ": ";
                    return ComponentesConexos(novoIndice, valor, nConjuntos, idVertices, listaConjunto);
                }

                //novoIndice = -1, significa que não há vertices em branco para serem visitados
                //realiza quase o mesmo codigo do primeiro if, com a diferença que não há chamada recursiva
                //também não há preparação da string para a proxima chamada, e sim um acabamento final
                else
                {
                    valor += "\nAeroportos que separadamente, se removidos, interrompem a conectividade do conjunto " + nConjuntos + ": ";
                    for(int q = 0; q < listaConjunto.Count; q++)
                    {
                        if (isPendente(Vertices[listaConjunto[q]]))
                           auxVal += Vertices[listaConjunto[q]].Rotulo + ", ";
                    }

                    if (auxVal == "")
                        valor += "Não há aeroportos que separadamente, se removidos, interrompem a conectividade";

                    else
                        valor += auxVal;

                    return valor;
                }
            }
        }
    }
}
