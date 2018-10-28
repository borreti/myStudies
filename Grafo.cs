using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MainProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Grafo_Und grafoSemDirecao;

            //testes considerando um arquivo que dê um grafo com pelo menos 4 vertices
            grafoSemDirecao = MontagemGrafoUnd("grafo.txt");
            Vertice vertice1, vertice2, vertice3;
            vertice1 = grafoSemDirecao.SelecionarVertice(0);
            vertice2 = grafoSemDirecao.SelecionarVertice(1);
            vertice3 = grafoSemDirecao.SelecionarVertice(2);
            Console.WriteLine("TESTES EM GRAFOS NÃO DIRIGIDOS");
            Console.WriteLine("V1 é adjacente a V3? {0}", grafoSemDirecao.isAdjacente(vertice1, vertice3));
            Console.WriteLine("Qual o grau de V2?  {0}", grafoSemDirecao.getGrau(vertice2));
            Console.WriteLine("O vertice V1 é isolado? {0}", grafoSemDirecao.isIsolado(vertice1));
            Console.WriteLine("O vertice V2 é pendente? {0}", grafoSemDirecao.isPendente(vertice2));
            Console.WriteLine("O grafo é regular? {0}", grafoSemDirecao.isRegular());
            Console.WriteLine("O grafo é nulo? {0}", grafoSemDirecao.isNulo());
            Console.WriteLine("O grafo é completo? {0}", grafoSemDirecao.isCompleto());
            Console.WriteLine("O grafo é conexo? {0}", grafoSemDirecao.isConexo());
            */

            Grafo_Und grafoDirecionado = MontagemGrafoUnd("grafo.txt");
            Grafo_Und arvoreMinima = grafoDirecionado.Kruskal();

            List<int> caminho = grafoDirecionado.Dijkstra(1,4);

            foreach(int v in caminho)
                Console.WriteLine(v+1);


            Console.ReadKey();
        }


        static Grafo_Und MontagemGrafoUnd(string arqName)
        {
            Grafo_Und grafo;
            StreamReader reader = new StreamReader(arqName);
            int nVertices = int.Parse(reader.ReadLine());
            List<ParOrdenado> lista = new List<ParOrdenado>();
            while (!reader.EndOfStream)
            {
                int x, y, peso;
                string linha = reader.ReadLine();
                string[] vetSplit = linha.Split(';');
                x = int.Parse(vetSplit[0]) - 1;
                y = int.Parse(vetSplit[1]) - 1;
                peso = int.Parse(vetSplit[2]);
                ParOrdenado par = new ParOrdenado(x, y, peso);
                lista.Add(par);
            }

            reader.Close();

            return grafo = new Grafo_Und(nVertices, lista);
        }

        static Grafo_Dir MontagemGrafoDir(string arqName)
        {
            Grafo_Dir grafo;

            StreamReader reader = new StreamReader(arqName);
            int nVertices = int.Parse(reader.ReadLine());
            List<ParOrdenado> lista = new List<ParOrdenado>();

            while (!reader.EndOfStream)
            {
                int x, y, peso, direcao;
                string linha = reader.ReadLine();
                string[] vetSplit = linha.Split(';');
                x = int.Parse(vetSplit[0]) - 1;
                y = int.Parse(vetSplit[1]) - 1;
                peso = int.Parse(vetSplit[2]);
                direcao = int.Parse(vetSplit[3]);
                if (direcao == -1)
                {
                    int aux = x;
                    x = y;
                    y = aux;
                }
                ParOrdenado par = new ParOrdenado(x, y, peso);
                lista.Add(par);
            }

            reader.Close();

            return grafo = new Grafo_Dir(nVertices, lista);
        }
    }

    //Par ordenado é uma classe que será utilizada para montar as arestas
    //X representa a linha e Y a coluna
    class ParOrdenado
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Peso { get; set; }

        public ParOrdenado(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public ParOrdenado(int X, int Y, int Peso)
        {
            this.Peso = Peso;
            this.X = X;
            this.Y = Y;
        }
    }

    class Aresta
    {
        public Vertice verticeOrigem { get; set; }
        public Vertice verticeConectado { get; set; }
        public int Direcao { get; set; }
        public int Peso { get; set; }
        public int ID { get; set; }

        public Aresta(Vertice verticeOrigem, Vertice verticeConectado, int Direcao, int Peso)
        {
            this.verticeOrigem = verticeOrigem;
            this.verticeConectado = verticeConectado;
            this.Direcao = Direcao;
            this.Peso = Peso;
        }
    }

    abstract class Grafo
    {
        protected Vertice[] Vertices;

        protected void FormarNovaAresta(int idV1, int idV2, int Peso)
        {
            Aresta i1 = new Aresta(this.Vertices[idV2], this.Vertices[idV1], -1, Peso);
            Aresta i2 = new Aresta(this.Vertices[idV1], this.Vertices[idV2], 1, Peso);
            Vertices[idV1].ListaDeAdjacencia.Add(i2);
            Vertices[idV2].ListaDeAdjacencia.Add(i1);
        }


        public abstract bool isConexo();

        public abstract int getGrau(Vertice v1);

        public abstract bool isAdjacente(Vertice v1, Vertice v2);

        public abstract Grafo getComplementar();

        public abstract bool isEuleriano();

        public abstract void ImprimirListaDeAdjacencia();

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

    }

    class Grafo_Dir : Grafo
    {
        public Grafo_Dir(int numeroDeVertices, List<ParOrdenado> listaDePares)
        {
            Vertices = new Vertice[numeroDeVertices];

            for (int i = 0; i < Vertices.Length; i++)
                Vertices[i] = new Vertice(i);


            foreach (ParOrdenado parOrdenado in listaDePares)
                FormarNovaAresta(Vertices[parOrdenado.X].ID, Vertices[parOrdenado.Y].ID, parOrdenado.Peso);
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

        public override bool isConexo()
        {
            throw new NotImplementedException();
        }

        public override bool isAdjacente(Vertice v1, Vertice v2)
        {
            throw new NotImplementedException();
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
                    if (vetVertices[vetVertices[indice].ListaDeAdjacencia[p].verticeConectado.ID].EstadoCor == 1 && vetVertices[indice].ListaDeAdjacencia[p].Direcao == 1)
                    {
                        tempo++;  //acrescenta uma unidade de tempo
                        int predecessor = indice;  //define o predecessor
                        int novoIndice = vetVertices[indice].ListaDeAdjacencia[p].verticeConectado.ID; //define o proximo indice a ser acessado
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

        //Verifica se existem vertices em branco, e se existe, retorna o indice para acesso
        //Se não existem vertices em branco, retorna -1

        public override bool isEuleriano()
        {
            throw new NotImplementedException();
        }

        public override void ImprimirListaDeAdjacencia()
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                Console.Write("Vertice {0}: ", Vertices[i].ID + 1);

                for (int j = 0; j < Vertices[i].ListaDeAdjacencia.Count; j++)
                {
                    Console.Write("{0}:{1}:{2}", Vertices[i].ListaDeAdjacencia[j].verticeConectado.ID + 1, Vertices[i].ListaDeAdjacencia[j].Direcao, Vertices[i].ListaDeAdjacencia[j].Peso);

                    if (j != Vertices[i].ListaDeAdjacencia.Count - 1)
                        Console.Write(", ");

                    else
                        Console.WriteLine("");
                }
            }
        }
    }

    class Rede : Grafo_Dir
    {
        public Vertice Origem { get; set; }
        public Vertice Destino { get; set; }

        public Rede(int numeroVertices, int idOrigem, int idDestino)
        {
            Vertices = new Vertice[numeroVertices];
            Origem = Vertices[idOrigem];
            Destino = Vertices[idDestino];
        }

        public Rede(int numeroVertices, List<ParOrdenado> listaDePares, int idVerticeOrigem, int idVerticeDestino)
        {
            Vertices = new Vertice[numeroVertices];

            for (int i = 0; i < Vertices.Length; i++)
                Vertices[i] = new Vertice(i);

            foreach (ParOrdenado parOrdenado in listaDePares)
                FormarNovaAresta(Vertices[parOrdenado.X].ID, Vertices[parOrdenado.Y].ID, parOrdenado.Peso);

            Origem = Vertices[idVerticeOrigem];
            Destino = Vertices[idVerticeDestino];
        }

        private Rede EncontrarRedeResidual(Vertice atual, Queue<Vertice> filaVertices, Vertice[] vetVertices, int distancia, int tempo, Rede redeResidual)
        {
            if (atual.ID != Origem.ID)
            {
                vetVertices[atual.ID].EstadoCor = 2;

                filaVertices.Enqueue(atual);

                int verticesVisitados = 0;

                for (int i = 0; i < atual.ListaDeAdjacencia.Count; i++)
                {
                    if (vetVertices[atual.ListaDeAdjacencia[i].verticeConectado.ID].EstadoCor < 2)
                    {
                        vetVertices[atual.ListaDeAdjacencia[i].verticeConectado.ID].Predecessor = atual;
                        vetVertices[atual.ListaDeAdjacencia[i].verticeConectado.ID].TempoDeDescoberta = tempo;
                        vetVertices[atual.ListaDeAdjacencia[i].verticeConectado.ID].Distancia = distancia;
                        tempo++;
                        redeResidual.FormarNovaAresta(vetVertices[atual.ListaDeAdjacencia[i].verticeConectado.ID].ID, atual.ID, atual.ListaDeAdjacencia[i].Peso);
                        filaVertices.Enqueue(atual.ListaDeAdjacencia[i].verticeConectado);
                        verticesVisitados++;
                    }
                }

                if (verticesVisitados == 0)
                    return null;

                tempo++;
                vetVertices[atual.ID].EstadoCor = 3;
                filaVertices.Dequeue();
                return EncontrarRedeResidual(filaVertices.ElementAt(0), filaVertices, vetVertices, distancia + 1, tempo + 1, redeResidual);
            }

            return redeResidual;
        }
    }

    class Grafo_Und : Grafo
    {
        public Grafo_Und(int numeroDeVertices, List<ParOrdenado> listaDePares)
        {
            Vertices = new Vertice[numeroDeVertices];

            for (int i = 0; i < Vertices.Length; i++)
                Vertices[i] = new Vertice(i);


            foreach (ParOrdenado parOrdenado in listaDePares)
                FormarNovaAresta(Vertices[parOrdenado.X].ID, Vertices[parOrdenado.Y].ID, parOrdenado.Peso);
        }

        public override bool isAdjacente(Vertice v1, Vertice v2)
        {
            for (int k = 0; k < Vertices[v1.ID].ListaDeAdjacencia.Count; k++)
            {
                if (Vertices[v1.ID].ListaDeAdjacencia[k].verticeConectado.ID == v2.ID)
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

        public override bool isConexo()
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                int val = 0;
                Vertice v1 = Vertices[i];
                for (int j = 0; j < v1.ListaDeAdjacencia.Count; j++)
                {
                    if (v1 != v1.ListaDeAdjacencia[j].verticeConectado)
                        val++;
                }

                if (val == 0)
                    return false;
            }

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

        public override void ImprimirListaDeAdjacencia()
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                Console.Write("Vertice {0}: ", Vertices[i].ID + 1);

                for (int j = 0; j < Vertices[i].ListaDeAdjacencia.Count; j++)
                {
                    Console.Write("{0}:{1}", Vertices[i].ListaDeAdjacencia[j].verticeConectado.ID + 1, Vertices[i].ListaDeAdjacencia[j].Peso);

                    if (j != Vertices[i].ListaDeAdjacencia.Count - 1)
                        Console.Write(", ");

                    else
                        Console.WriteLine("");
                }
            }
        }

        public Grafo_Und Kruskal()
        {
            if (!isConexo())
                return null;

            List<Aresta> listaArestas = new List<Aresta>();
            List<ParOrdenado> pares = new List<ParOrdenado>();
            Grafo_Und grafoAuxilir = new Grafo_Und(Vertices.Length, pares);

            for (int g = 0; g < Vertices.Length; g++)
            {
                for (int a = 0; a < Vertices[g].ListaDeAdjacencia.Count; a++)
                {
                    if (Vertices[g].ListaDeAdjacencia[a].Direcao == 1)
                        listaArestas.Add(Vertices[g].ListaDeAdjacencia[a]);
                }
            }

            Aresta[] arestasOrdenadas = insertionSort(listaArestas.ToArray());

            for (int v = 0; v < arestasOrdenadas.Length; v++)
            {
                int idOrigem = arestasOrdenadas[v].verticeOrigem.ID;
                int idDestino = arestasOrdenadas[v].verticeConectado.ID;

                //if para ignorar os loops do grafo original
                if (idOrigem != idDestino)
                {
                    Queue<int> fila = new Queue<int>();

                    if (!VerificarCiclo(fila, idOrigem, idDestino, idDestino, grafoAuxilir))
                        grafoAuxilir.FormarNovaAresta(idOrigem, idDestino, arestasOrdenadas[v].Peso);
                }
            }

            return grafoAuxilir;
        }

        private bool VerificarCiclo(Queue<int> fila, int idOrigem, int idDestino, int idAtual, Grafo_Und grafoAux)
        {
            grafoAux.Vertices[idAtual].EstadoCor = 2;

            for (int w = 0; w < grafoAux.Vertices[idAtual].ListaDeAdjacencia.Count; w++)
            {
                int idLaco = grafoAux.Vertices[idAtual].ListaDeAdjacencia[w].verticeConectado.ID;

                if (idLaco == idOrigem)
                {
                    grafoAux.ResetarCores();
                    return true;
                }

                else
                {
                    if (grafoAux.Vertices[idLaco].EstadoCor == 1)
                    {
                        fila.Enqueue(idLaco);
                        grafoAux.Vertices[idLaco].EstadoCor = 2;
                    }
                }
            }

            grafoAux.Vertices[idAtual].EstadoCor = 3;

            if (fila.Count > 0)
            {
                int prox = fila.Dequeue();

                return VerificarCiclo(fila, idOrigem, idDestino, prox, grafoAux);
            }

            else
            {
                grafoAux.ResetarCores();
                return false;
            }
        }

        private Aresta[] insertionSort(Aresta[] vetor)
        {
            int i, j;

            Aresta atual;

            for (i = 1; i < vetor.Length; i++)
            {
                atual = vetor[i];
                j = i;
                while ((j > 0) && (vetor[j - 1].Peso > atual.Peso))
                {
                    vetor[j] = vetor[j - 1];
                    j = j - 1;
                }

                vetor[j] = atual;
            }

            return vetor;
        }

        //retorna uma lista com os ids do menor caminho entre dois vertices
        public List<int> Dijkstra(int idOrigem, int idDestino)
        {
            //só é executado o algoritmo se o grafo for conexo
            if (!isConexo())
                return null;

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
                    vetorPredecessor[q] = int.MaxValue;
                }
            }

            Dijkstra(vetorDistancias, vetorPredecessor, idOrigem, idDestino, 0);

            Stack<int> pilhaIndices = new Stack<int>();

            pilhaIndices.Push(idDestino);

            int proxId = idDestino;

            while (true)
            {
                pilhaIndices.Push(vetorPredecessor[proxId]);
                proxId = vetorPredecessor[proxId];
                if (proxId == idOrigem)
                    break;
            }

            List<int> listaIds = new List<int>();

            foreach (int valor in pilhaIndices)
                listaIds.Add(valor);

            return listaIds;
        }

        private void Dijkstra(int[] vetorDistancias, int[] vetorPredecessor, int atual, int idDestino, int distAnterior)
        {
            int idMenor = -1, menorCaminho = int.MaxValue;

            for (int f = 0; f < Vertices[atual].ListaDeAdjacencia.Count; f++)
            {
                if (Vertices[Vertices[atual].ListaDeAdjacencia[f].verticeConectado.ID].EstadoCor == 1)
                {
                    int destino = Vertices[atual].ListaDeAdjacencia[f].verticeConectado.ID;

                    int dist = Vertices[atual].ListaDeAdjacencia[f].Peso + distAnterior;

                    if (dist < vetorDistancias[destino])
                    {
                        vetorPredecessor[destino] = atual;
                        vetorDistancias[destino] = dist;
                    }

                    if (vetorDistancias[destino] < menorCaminho && vetorDistancias[destino] != 0)
                    {
                        menorCaminho = vetorDistancias[destino];
                        idMenor = destino;
                    }
                }
            }

            Vertices[atual].EstadoCor = 3;

            if (idMenor == -1)           
                idMenor = BuscarVerticeAberto(vetorDistancias);


            if (idMenor != -1)
                Dijkstra(vetorDistancias, vetorPredecessor, idMenor, idDestino, vetorDistancias[idMenor]);
        }

        //retorna o indice de um vertice aberto com a menor distancia
        private int BuscarVerticeAberto(int[] vetorDistancias)
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
    }

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
