using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_Grafos
{
    class Grafo_Dir : Grafo
    {
        //construtor com rotulos
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

        //construtor sem rotulos
        public Grafo_Dir(int numeroDeVertices, List<ParOrdenado> listaDePares)
        {
            Vertices = new Vertice[numeroDeVertices];

            for (int i = 0; i < Vertices.Length; i++)
                Vertices[i] = new Vertice(i);


            foreach (ParOrdenado parOrdenado in listaDePares)
                FormarNovaAresta(Vertices[parOrdenado.X].ID, Vertices[parOrdenado.Y].ID, parOrdenado.Pesos, parOrdenado.ListaDeHorarios);
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

            //entrada --> dir = -1
            //saida --> dir = 1

            //para todos as adjacencias do vertice
            for (int i = 0; i < Vertices[v1.ID].ListaDeAdjacencia.Count; i++)
            {
                if (Vertices[v1.ID].ListaDeAdjacencia[i].Direcao == dir) //se o vertice possui um adjacente com a direção requisitada
                    grau++;//o grau aumenta mais um
            }

            return grau;
        }

        //pega o grau geral do vertice, considerando as entradas e saidas
        public override int getGrau(Vertice v1)
        {
            int grau = 0;
            grau += getGrauEntrada(v1);
            grau += getGrauSaida(v1);

            return grau;
        }


        public override bool isAdjacente(Vertice v1, Vertice v2)
        {
            //para todos os itens adjacentes
            for (int k = 0; k < Vertices[v1.ID].ListaDeAdjacencia.Count; k++)
            {
                //compara os ids para verificar se a aresta existe
                //considera a direção
                if (Vertices[v1.ID].ListaDeAdjacencia[k].verticeDestino.ID == v2.ID && Vertices[v1.ID].ListaDeAdjacencia[k].Direcao == 1)
                    return true;
            }

            return false;
        }

        public override Grafo getComplementar()
        {
            throw new NotImplementedException();
        }

        public override bool isEuleriano()
        {
            throw new NotImplementedException();
        }

        //verifica se o grafo é fortemente conexo
        public override bool isConexo()
        {
           for (int x = 0; x < Vertices.Length; x++)
            {
                int gEnt = 0, gSai = 0;
                for (int w = 0; w < Vertices[x].ListaDeAdjacencia.Count; w++)
                {
                    Aresta a = Vertices[x].ListaDeAdjacencia[w];

                    int orig = a.verticeOrigem.ID;
                    int dest = a.verticeDestino.ID;

                    //desconsidera os loops
                    if (orig != dest)
                    {

                        if (a.Direcao == 1)
                            gSai++;

                        else
                            gEnt++;
                    }
                }

                if (!(gEnt > 0 && gSai > 0))
                    return false;
            }

            return true;
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

        protected override void Dijkstra(int[] vetorDistancias, int[] vetorPredecessor, int atual, int nPeso)
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

                    //dijkstra por distancia total
                    if (nPeso == 0)
                    {
                        dist = Vertices[atual].ListaDeAdjacencia[f].Pesos.Distancia + vetorDistancias[atual];
                    }


                    //dijkstra por duração total dos voos
                    else if (nPeso == 1)
                    {
                        dist = Vertices[atual].ListaDeAdjacencia[f].Pesos.DuracaoDoVoo + vetorDistancias[atual];
                    }

                    //dijkstra por duração total da viagem
                    else if (nPeso == 3)
                    {
                        //!----ALGORITMO QUE CALCULA O TEMPO EM ESPERA----!//
                        int pred = vetorPredecessor[atual];
                        int tempoEmEsperaM = 0;
                        TimeSpan tempoEmEspera;

                        //se houver predecessor
                        if (pred != -1)
                        {
                            Aresta voo = BuscarAresta(pred, atual, 1);
                            DateTime horaAtual = voo.PrevisaoChegada[0]; // a previsão de chegada mais curta define a hora atual

                            int indiceHorario = -1; //inicializa com -1 para controle

                            DateTime horarioEscolhido = Vertices[atual].ListaDeAdjacencia[f].ListaDeVoos[0];

                            //for que procura um horário igual ou maior que o horário atual e menor ou igual ao horario que havia sido escolhido antes
                            for (int x = 0; x < Vertices[atual].ListaDeAdjacencia[f].ListaDeVoos.Count; x++)
                            {
                                if (Vertices[atual].ListaDeAdjacencia[f].ListaDeVoos[x] >= horaAtual && Vertices[atual].ListaDeAdjacencia[f].ListaDeVoos[x] <= horarioEscolhido)
                                {
                                    horarioEscolhido = Vertices[atual].ListaDeAdjacencia[f].ListaDeVoos[x];
                                    indiceHorario = x;
                                }
                            }

                            //se a variavel tem valor -1, significa que não foi encontrado um horário que cumprisse as condições necessárias
                            if (indiceHorario != -1)
                            {
                                //calculo dateTime
                                //!---- Situação desejada, pois essa parte do código executa quando ainda há um voo disponível para aquele mesmo dia ----!
                                tempoEmEspera = Vertices[atual].ListaDeAdjacencia[f].ListaDeVoos[indiceHorario] - horaAtual;
                            }

                            //indiceHorario terminou como menos, então não há um voo disponivel para hoje
                            else
                            {
                                //pega o voo mais cedo do próximo dia
                                DateTime aux = Vertices[atual].ListaDeAdjacencia[f].ListaDeVoos[0], aux2;

                                //cálculo para definir se o valor será negativo ou positivo
                                int varCompar = ((aux.Hour * 60) + aux.Minute) - ((horaAtual.Hour * 60) + horaAtual.Minute);

                                //se for negativo, adiciona um dia num novo auxiliar
                                if (varCompar < 0)
                                {
                                    aux2 = new DateTime(aux.Year, aux.Month, aux.Day + 1, aux.Hour, aux.Minute, aux.Second);
                                    tempoEmEspera = aux2 - horaAtual;
                                }

                                //se for positivo, subtrai a hora atual do primeiro auxiliar
                                else
                                {
                                    tempoEmEspera = horaAtual - aux;
                                }
                            }

                            //transforma a espera em minutos
                            tempoEmEsperaM = ((tempoEmEspera.Days * 24) * 60) + (tempoEmEspera.Hours * 60) + tempoEmEspera.Minutes;
                            dist = Vertices[atual].ListaDeAdjacencia[f].Pesos.DuracaoDoVoo + vetorDistancias[atual] + tempoEmEsperaM;
                        }

                        //se não há predecessor
                        else
                        {
                            dist = Vertices[atual].ListaDeAdjacencia[f].Pesos.DuracaoDoVoo + vetorDistancias[atual];
                        }
                    }

                    //verifica se a distancia encontrada é menor do que a que estava no vetor de distancias
                    //se for, atualiza a distancia e o predecessor
                    if (dist < vetorDistancias[indexDestino])
                    {
                        vetorPredecessor[indexDestino] = atual;
                        vetorDistancias[indexDestino] = dist;
                    }
                }
            }

                      //método auxiliar para encontrar o vértice aberto com a menor distancia
            idMenor = BuscarVerticeAberto(vetorDistancias);

            if (idMenor != -1)
                Dijkstra(vetorDistancias, vetorPredecessor, idMenor, nPeso);
        }

        public Aresta BuscarAresta(int v1, int v2, int dir)
        {
            for (int w = 0; w < Vertices[v1].ListaDeAdjacencia.Count; w++)
            {
                if (Vertices[v1].ListaDeAdjacencia[w].verticeDestino == Vertices[v2] && Vertices[v1].ListaDeAdjacencia[w].Direcao == dir)
                    return Vertices[v1].ListaDeAdjacencia[w];
            }

            return null;
        }

        protected override List<List<Vertice>> TravessiaEmAplitude(int distancia, int tempo, int atual, Queue<int> fila)
        {
            //para todo vertice adjacente ao atual
            for (int w = 0; w < this.Vertices[atual].ListaDeAdjacencia.Count; w++)
            {
                //captura o id do destino
                int indiceAux = this.Vertices[atual].ListaDeAdjacencia[w].verticeDestino.ID;

                //verifica se a direção é 1, e se o vertice ainda não foi visitado
                if (this.Vertices[indiceAux].EstadoCor == 1 && this.Vertices[atual].ListaDeAdjacencia[w].Direcao == 1)
                {
                    //colore o vertice, muda o predecessor, e poe o tempo de descoberta
                    this.Vertices[indiceAux].EstadoCor = 2;
                    this.Vertices[indiceAux].Predecessor = Vertices[atual];
                    this.Vertices[indiceAux].TempoDeDescoberta = tempo;
                    tempo++;
                    //enfileira o indice do vertice que foi visitado
                    fila.Enqueue(indiceAux);
                }
            }

            //finaliza o tempo do vertice atual
            this.Vertices[atual].TempoDeFinalizacao = tempo;
            tempo++;

            //verifica se ainda há vertices na fila
            if (fila.Count > 0)
            {
                //se houver, cria o novo indice
                int novoIndice = fila.Dequeue();
                //chamada recursiva
                return this.TravessiaEmAplitude(distancia, tempo, novoIndice, fila);
            }

            else
            {
                return null;
            }
        }

        public string BuscarUltimoHorario(DateTime horarioLimite, int origem, int destino)
        {
            //busca a menor rota
            int peso = 0;
            List<int> caminho = Dijkstra(origem, destino, 3,ref peso);

            Queue<int> FilaId = new Queue<int>();

            //enfileira o caminho
            for (int s = caminho.Count - 1; s >= 0; s--)
                FilaId.Enqueue(caminho[s]);

            //o resultado do caminho é uma pilha
            //o algoritmo começa no destino e vai voltando até a origem, efetuando todos os cálculos necessários
            Stack<string> resultadoString = new Stack<string>();
            string res = "";

            BuscarUltimoHorario(caminho[caminho.Count - 2], caminho[0], caminho[caminho.Count - 1], horarioLimite, resultadoString, FilaId);

            //se houver itens stackados
            if (resultadoString.Count > 0)
            {
                //monta a string de resposta
                foreach (string st in resultadoString)
                    res += st;
            }

            //não há itens stackados
            //ocorre quando não há uma rota possível para o destino
            else
            {
                return null;
            }

            return res;
        }

        private void BuscarUltimoHorario(int atual, int origem, int destino, DateTime horarioLimite, Stack<string> resultado, Queue<int> fila)
        {
            //desenfileira o atual
            fila.Dequeue();

            int indiceVoo = -1;
            DateTime horario;
            DateTime novoHor = DateTime.Now;

            //para toda aresta do vertice atual
            for (int k = 0; k < Vertices[atual].ListaDeAdjacencia.Count; k++)
            {
                horario = Vertices[atual].ListaDeAdjacencia[k].ListaDeVoos[0]; //horário escolhido arbitrariamente para efetuar as primeiras comparações

                //if para verificar a direção do voo e se essa é a aresta que corresponde a onde deseja-se chegar
                if (Vertices[atual].ListaDeAdjacencia[k].Direcao == 1 && Vertices[atual].ListaDeAdjacencia[k].verticeDestino.ID == destino)
                {
                    //para todo voo na lista de voos
                    for (int q = 0; q < Vertices[atual].ListaDeAdjacencia[k].PrevisaoChegada.Count; q++)
                    {
                        //se a previsão de chegada, é menor ou igual ao horario limite, e maior ou igual ao ultimo horário selecionado
                        if (Vertices[atual].ListaDeAdjacencia[k].PrevisaoChegada[q] <= horarioLimite && Vertices[atual].ListaDeAdjacencia[k].PrevisaoChegada[q] >= horario)
                        {
                            indiceVoo = q; //captura o indice desse voo
                            horario = Vertices[atual].ListaDeAdjacencia[k].PrevisaoChegada[q]; //define o horário de chegada ao proximo aeroporto
                            novoHor = Vertices[atual].ListaDeAdjacencia[k].ListaDeVoos[q]; //define o horário de saida do aeroporto atual
                        }
                    }

                    string r = Vertices[atual].Rotulo + " para " + Vertices[destino].Rotulo + " ";

                    //se indiceVoo é -1, significa que não encontrou voos na rota, que cumprisse o hórario exigido
                    if (indiceVoo == -1)
                    {
                        //tira todo o conteudo da pilha
                        for (int tam = resultado.Count; tam > 0; tam--)
                            resultado.Pop();

                        //retorna a pilha vazia
                        return;                       
                    }

                    r += Vertices[atual].ListaDeAdjacencia[k].ListaDeVoos[indiceVoo] + "\n";

                    //empilha o resultado
                    resultado.Push(r);

                    //interrompe o loop das arestas, pois o voo já foi encontrado
                    break;
                }
            }

            if (fila.Count > 1)
                BuscarUltimoHorario(fila.ElementAt(fila.Count - 1), origem, fila.ElementAt(fila.Count - 2), novoHor, resultado, fila);
        }

        public List<List<Vertice>> ComponentensConexos()
        {
            ResetarIndex();
            List<List<Vertice>> listaComponentes = new List<List<Vertice>>();
            int index = 0;
            Stack<int> S = new Stack<int>();

            foreach (Vertice v in Vertices)
            {
                if (v.Index < 0)
                {
                    strongconnect(v, index, S, listaComponentes);
                }
            }

            return listaComponentes;
        }

        private void strongconnect(Vertice v, int index, Stack<int> S, List<List<Vertice>> componentesConexos)
        {
            v.Index = index;
            v.LowLink = index;
            index++;
            S.Push(v.ID);

            for (int q = 0; q < v.ListaDeAdjacencia.Count; q++)
            {
                Aresta a = v.ListaDeAdjacencia[q];

                if (a.Direcao == 1)
                {
                    int d = a.verticeDestino.ID;
                    int o = a.verticeOrigem.ID;

                    if (Vertices[d].Index < 0)
                    {
                        strongconnect(Vertices[d], index, S, componentesConexos);
                        Vertices[d].LowLink = Math.Min(v.LowLink, Vertices[d].LowLink);
                    }
                    else if (S.Contains(Vertices[d].ID))
                    {
                        v.LowLink = Math.Min(v.LowLink, Vertices[d].Index);
                    }
                }
            }

            if (v.LowLink == v.Index)
            {
                List<Vertice> scc = new List<Vertice>();
                int idW;
                int idV = v.ID;
                do
                {
                    idW = S.Pop();
                    scc.Add(Vertices[idW]);
                } while (idV != idW);

                componentesConexos.Add(scc);
            }
        }   
    }
}
