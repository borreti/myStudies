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

            Grafo_Dir grafoDirecionado = MontagemGrafoDir("grafoDir.txt");

            Stack<Vertice> stack = grafoDirecionado.OrdenacaoTopologica();

            foreach(Vertice v in stack)
                Console.WriteLine(v.ID + 1);

            grafoDirecionado.ImprimirListaDeAdjacencia();

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

    class ItemListaAdj
    {
        public Vertice vertice { get; set; }
        public int Direcao { get; set; }
        public int Peso { get; set; }

        public ItemListaAdj(Vertice vertice, int Direcao, int Peso)
        {
            this.vertice = vertice;
            this.Direcao = Direcao;
            this.Peso = Peso;
        }
    }

    abstract class Grafo
    {
        protected Vertice[] Vertices;

        protected List<Aresta> ListaDeArestas;

        protected abstract void FormarNovaAresta(Vertice v1, Vertice v2, int Peso);

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
            if (ListaDeArestas.Count == 0)
                return true;

            else
                return false;
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

    }

    class Grafo_Dir: Grafo
    {
        public Grafo_Dir(int numeroDeVertices, List<ParOrdenado> listaDePares)
        {
            Vertices = new Vertice[numeroDeVertices];
            ListaDeArestas = new List<Aresta>();

            for (int i = 0; i < Vertices.Length; i++)
                Vertices[i] = new Vertice(i);


            foreach (ParOrdenado parOrdenado in listaDePares)
                FormarNovaAresta(Vertices[parOrdenado.X], Vertices[parOrdenado.Y], parOrdenado.Peso);
        }

        public Grafo_Dir()
        {

        }

        protected override void FormarNovaAresta(Vertice v1, Vertice v2, int Peso)
        {
            Aresta novaAresta1 = new Aresta(Peso,v1, v2);
            ItemListaAdj i1 = new ItemListaAdj(v1, -1, Peso);
            ItemListaAdj i2 = new ItemListaAdj(v2, 1, Peso);
            Vertices[v1.ID].ListaDeAdjacencia.Add(i2);
            Vertices[v2.ID].ListaDeAdjacencia.Add(i1);
            ListaDeArestas.Add(novaAresta1);
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
        private void VisitarVerticeOT(Vertice[] vetVertices, int indice,ref int tempo, Stack<Vertice> pilhaVertices)
        {
            //se o vertice ainda não foi pintado de vermelho
            if (vetVertices[indice].EstadoCor != 3)
            {
                //percorre toda a lista de adjacencia do vertice atual
                for (int p = 0; p < vetVertices[indice].ListaDeAdjacencia.Count; p++)
                {
                    //se existe um vertice ainda não visitado
                    if (vetVertices[vetVertices[indice].ListaDeAdjacencia[p].vertice.ID].EstadoCor == 1 && vetVertices[indice].ListaDeAdjacencia[p].Direcao == 1)
                    {                    
                        tempo++;  //acrescenta uma unidade de tempo
                        int predecessor = indice;  //define o predecessor
                        int novoIndice = vetVertices[indice].ListaDeAdjacencia[p].vertice.ID; //define o proximo indice a ser acessado
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
                if (indice !=-1)
                OrdenacaoTopologica(vetVertices, indice, tempo, pilhaVertices); //chamada do método de ordenação com o indice do vertice em branco
            }
        }

        //Verifica se existem vertices em branco, e se existe, retorna o indice para acesso
        //Se não existem vertices em branco, retorna -1
        private int ExisteVerticesEmBranco()
        {
            for (int h= 0; h < Vertices.Length; h++)
            {
                if (Vertices[h].EstadoCor == 1)
                    return h;
            }

            return -1;
        }

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
                    Console.Write("{0}:{1}", Vertices[i].ListaDeAdjacencia[j].vertice.ID + 1, Vertices[i].ListaDeAdjacencia[j].Direcao);

                    if (j != Vertices[i].ListaDeAdjacencia.Count - 1)
                        Console.Write(", ");

                    else
                        Console.WriteLine("");
                }
            }
        }
    }

    class Rede: Grafo_Dir
    {
        public Vertice Origem { get; set; }
        public Vertice Destino { get; set; }

        public Rede(int numeroVertices, List<ParOrdenado> listaDePares, int idVerticeOrigem, int idVerticeDestino)
        {
            Vertices = new Vertice[numeroVertices];
            ListaDeArestas = new List<Aresta>();

            for (int i = 0; i < Vertices.Length; i++)
                Vertices[i] = new Vertice(i);

            foreach (ParOrdenado parOrdenado in listaDePares)
                FormarNovaAresta(Vertices[parOrdenado.X], Vertices[parOrdenado.Y], parOrdenado.Peso);

            Origem = Vertices[idVerticeOrigem];
            Destino = Vertices[idVerticeDestino];
        }

       public int FluxoMaximo()
        {
            Rede redeAux = this;
            List<Aresta> ArestasCaminhadas = new List<Aresta>();


        }

        private void AlgAuxiliar(Vertice atual)
        {
           
        }

        private Aresta BuscarMaiorCaminho(Vertice v)
        {
            int idMaiorCaminho = -1;
            int maiorCaminho = 0;

            for (int i = 0; i < v.ListaDeAdjacencia.Count; i++)
            {
                if (v.ListaDeAdjacencia[i].Direcao == 1)
                {
                    if (v.ListaDeAdjacencia[i].Peso > maiorCaminho)
                    {
                        idMaiorCaminho = v.ListaDeAdjacencia[i].vertice.ID;
                        maiorCaminho = v.ListaDeAdjacencia[i].Peso;
                    }
                }
            }

            if (idMaiorCaminho != 1)
            {
                Aresta a = new Aresta(maiorCaminho,v, Vertices[idMaiorCaminho]);
                return a;
            }

            else
            {
                return null;
            }
        }
    }

    class Grafo_Und: Grafo
    {       
        public Grafo_Und(int numeroDeVertices, List<ParOrdenado> listaDePares)
        {
            Vertices = new Vertice[numeroDeVertices];
            ListaDeArestas = new List<Aresta>();

            for (int i = 0; i < Vertices.Length; i++)
                Vertices[i] = new Vertice(i);
            

            foreach (ParOrdenado parOrdenado in listaDePares)
                FormarNovaAresta(Vertices[parOrdenado.X], Vertices[parOrdenado.Y], parOrdenado.Peso);
        }

        protected override void FormarNovaAresta(Vertice v1, Vertice v2, int Peso)
        {
            Aresta novaAresta = new Aresta(Peso, v1, v2);
            ItemListaAdj i1 = new ItemListaAdj(v1, 0, Peso);
            ItemListaAdj i2 = new ItemListaAdj(v2, 0, Peso);
            Vertices[v1.ID].ListaDeAdjacencia.Add(i2);
            Vertices[v2.ID].ListaDeAdjacencia.Add(i1);
            ListaDeArestas.Add(novaAresta);
        }

        public override bool isAdjacente(Vertice v1, Vertice v2)
        {
            ItemListaAdj item = new ItemListaAdj(Vertices[v2.ID], 0);
            
            for (int k = 0; k < Vertices[v1.ID].ListaDeAdjacencia.Count; k++)
            {
                if (Vertices[v1.ID].ListaDeAdjacencia[k].vertice == item.vertice)
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
            for(int i = 0; i < Vertices.Length; i++)
            {
                int val = 0;
                Vertice v1 = Vertices[i];
                for (int j = 0; j < v1.ListaDeAdjacencia.Count; j++)
                {
                    if (v1 != v1.ListaDeAdjacencia[j].vertice)
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
            return  grafo = new Grafo_Und(nVertices, pares);
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
                    Console.Write("{0}", Vertices[i].ListaDeAdjacencia[j].vertice.ID + 1);

                    if (j != Vertices[i].ListaDeAdjacencia.Count - 1)
                        Console.Write(", ");

                    else
                        Console.WriteLine("");
                }
            }
        }
    }

    class Vertice
    {
        public int ID { get; set; }
        public int EstadoCor { get; set; }
        public int TempoDeDescoberta { get; set; }
        public int TempoDeFinalizacao { get; set; }
        public string Rotulo { get; set; }
        public Vertice Predecessor { get; set; }
        public List<ItemListaAdj> ListaDeAdjacencia { get; set; }

        public Vertice(int ID)
        {
            this.ID = ID;
            ListaDeAdjacencia = new List<ItemListaAdj>();
            EstadoCor = 1;
            TempoDeDescoberta = -1;
            TempoDeFinalizacao = -1;
            Predecessor = null;
        }

        public Vertice(int ID, string Rotulo)
        {
            this.ID = ID;
            this.Rotulo = Rotulo;
            ListaDeAdjacencia = new List<ItemListaAdj>();
            EstadoCor = 1;
            TempoDeDescoberta = -1;
            TempoDeFinalizacao = -1;
            Predecessor = null;
        }
    }

    class Aresta
    { 
        public int Peso { get; set; }
        public Vertice V1 { get; set; }
        public Vertice V2 { get; set; }

        public Aresta(int Peso, Vertice V1, Vertice V2)
        {
            this.Peso = Peso;
            this.V1 = V1;
            this.V2 = V2;
        }

        public Aresta(Vertice V1, Vertice V2)
        {
            this.V1 = V1;
            this.V2 = V2;
            Peso = -1; //não existe peso
        }
    }


}
