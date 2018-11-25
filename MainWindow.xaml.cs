using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Trabalho_Grafos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int indiceAcesso = 0;
        int nGrafos = 0;
        List<Grafo_Und> ListaGrafosNaoDirigido = new List<Grafo_Und>();
        List<Grafo_Dir> ListaGrafosDirigidos = new List<Grafo_Dir>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void box_grafo_selecionado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            indiceAcesso = int.Parse(box_grafo_selecionado.SelectedItem.ToString()) - 1; //indice de acesso da lista dos grafos
            grid_lista_adjacencia.Items.Clear(); //limpa o quadro
            PreencherComboBox(box_aeroporto_origem, ListaGrafosDirigidos[indiceAcesso]); //preenche todas as caixas necessárias
            PreencherComboBox(box_aeroporto_destino, ListaGrafosDirigidos[indiceAcesso]);
            PreencherComboBox(box_aeroporto_origem_ultimo_voo, ListaGrafosDirigidos[indiceAcesso]);
            PreencherComboBox(box_aeroporto_destino_ultimo_voo, ListaGrafosDirigidos[indiceAcesso]);

            box_aeroporto_origem.SelectedIndex = 0;
            box_aeroporto_destino.SelectedIndex = box_aeroporto_destino.Items.Count - 1;
            box_aeroporto_origem_ultimo_voo.SelectedIndex = 0;
            box_aeroporto_destino_ultimo_voo.SelectedIndex = box_aeroporto_destino_ultimo_voo.Items.Count - 1;

            for (int ps = 0; ps < ListaGrafosDirigidos[indiceAcesso].Vertices.Length; ps++)
            {
                for (int fs = 0; fs < ListaGrafosDirigidos[indiceAcesso].Vertices[ps].ListaDeAdjacencia.Count; fs++)
                {
                    if (ListaGrafosDirigidos[indiceAcesso].Vertices[ps].ListaDeAdjacencia[fs].Direcao == 1)
                    grid_lista_adjacencia.Items.Add(ListaGrafosDirigidos[indiceAcesso].Vertices[ps].ListaDeAdjacencia[fs]);
                }
            }
        }

        private void PreencherComboBox(ComboBox box, Grafo grafo)
        {
            box.Items.Clear();
            Vertice[] vetorV = grafo.Vertices;

            foreach (Vertice item in vetorV)
                box.Items.Add(item.Rotulo);
        }

        private void ler_arquivo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string arqName = text_arq_nome.Text; //captura o nome digitado pelo usuario
                Grafo_Dir grafoDir;
                Grafo_Und grafoUnd;

                StreamReader reader = new StreamReader(arqName);
                int nVertices = int.Parse(reader.ReadLine());
                List<ParOrdenado> lista = new List<ParOrdenado>();
                string[] vetorNomes = new string[nVertices];

                int verticesLidos = 0;

                while (!reader.EndOfStream)
                {
                    int x, y, direcao;
                    Peso peso;
                    string linha = reader.ReadLine();
                    string rotulo1, rotulo2;

                    string[] vetSplit = linha.Split(';');
                    rotulo1 = vetSplit[0];
                    rotulo2 = vetSplit[1];

                    x = BuscarIndiceRotulo(vetorNomes, rotulo1);

                    if (x == -1)
                    {
                        vetorNomes[verticesLidos] = rotulo1;
                        x = verticesLidos;
                        verticesLidos++;
                    }

                    y = BuscarIndiceRotulo(vetorNomes, rotulo2);

                    if (y == -1)
                    {
                        vetorNomes[verticesLidos] = rotulo2;
                        y = verticesLidos;
                        verticesLidos++;
                    }

                    peso = TratarPesos(vetSplit[3], vetSplit[4]);
                    direcao = int.Parse(vetSplit[2]);

                    if (direcao == -1)
                    {
                        int aux = x;
                        x = y;
                        y = aux;
                    }

                    List<DateTime> ListaHorarios = new List<DateTime>();

                    for (int r = 5; r < vetSplit.Length; r++)
                    {
                        string [] novoVet = vetSplit[r].Split(':');
                        int hora = int.Parse(novoVet[0]);
                        int minuto = int.Parse(novoVet[1]);
                        DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hora, minuto, 0);
                        ListaHorarios.Add(dateTime);
                    }


                    ParOrdenado par = new ParOrdenado(x, y, peso, ListaHorarios);
                    lista.Add(par);
                }

                reader.Close();

                text_arq_nome.Clear();

                grafoDir = new Grafo_Dir(nVertices, lista, vetorNomes);
                grafoUnd = new Grafo_Und(nVertices, lista, vetorNomes);
                ListaGrafosDirigidos.Add(grafoDir);
                ListaGrafosNaoDirigido.Add(grafoUnd);

                nGrafos++;
                box_grafo_selecionado.Items.Add(nGrafos);
                box_grafo_selecionado.SelectedIndex = box_grafo_selecionado.Items.Count - 1;
                box_aeroporto_origem.SelectedIndex = 0;
                box_aeroporto_destino.SelectedIndex = box_aeroporto_destino.Items.Count - 1;
                MessageBox.Show("Novo grafo incluido na memória", "Arquivo foi lido com sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Erro ao tentar ler o arquivo", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Erro ao tentar ler o arquivo", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            catch(FormatException ex)
            {
                MessageBox.Show("Arquivo com formatação incorreta", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show("Arquivo com falha nas linhas", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Peso TratarPesos(string dist, string duracao_Voo)
        {
            string[] vetorSplit = duracao_Voo.Split(':');

            int distancia = int.Parse(dist), duracaoVoo = 0;

            duracaoVoo += int.Parse(vetorSplit[0]) * 60;
            duracaoVoo += int.Parse(vetorSplit[1]);

            Peso p = new Peso(distancia, duracaoVoo);

            return p;
        }

        private int BuscarIndiceRotulo(string[] vetorRotulos, string rotuloAtual)
        {
            for (int t = 0; t < vetorRotulos.Length; t++)
            {
                if (vetorRotulos[t] == rotuloAtual)
                    return t;
            }

            return -1;
        }

        private int BuscarIndiceRotulo(Vertice[] vertices, string rotuloAtual)
        {
            for (int b = 0; b < vertices.Length; b++)
            {
                if (vertices[b].Rotulo == rotuloAtual)
                    return b;
            }

            return -1;
        }

        private void btn_viagem_minima_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int tipoPeso = 0;
                int origem, destino;
                string aeOrigem, aeDestino;

                aeOrigem = box_aeroporto_origem.SelectedItem.ToString();
                aeDestino = box_aeroporto_destino.SelectedItem.ToString();
                origem = BuscarIndiceRotulo(ListaGrafosDirigidos[indiceAcesso].Vertices, aeOrigem);
                destino = BuscarIndiceRotulo(ListaGrafosDirigidos[indiceAcesso].Vertices, aeDestino);

                if (radio_0.IsChecked == true)
                    tipoPeso = 0;

                else if (radio_1.IsChecked == true)
                    tipoPeso = 1;

                else if (radio_2.IsChecked == true)
                    tipoPeso = 2;

                else if (radio_3.IsChecked == true)
                    tipoPeso = 3;

                List<int> listaCaminho = new List<int>();
                string caminho = "";
                Vertice[] vetorVertices = null;

                int pesoF = 0;

                if (tipoPeso < 2 || tipoPeso == 3)
                {
                    vetorVertices = ListaGrafosDirigidos[indiceAcesso].Vertices;
                    listaCaminho = ListaGrafosDirigidos[indiceAcesso].Dijkstra(origem, destino, tipoPeso, ref pesoF);
                }

                else if (tipoPeso == 2)
                {
                    ListaGrafosDirigidos[indiceAcesso].TravessiaEmAplitude(origem);
                    vetorVertices = ListaGrafosDirigidos[indiceAcesso].Vertices;

                    Vertice predecessor = vetorVertices[destino].Predecessor;
                    listaCaminho.Add(destino);

                    while (predecessor != null)
                    {
                        listaCaminho.Add(predecessor.ID);
                        predecessor = predecessor.Predecessor;
                    }

                    List<int> auxLista = listaCaminho;

                    listaCaminho = new List<int>();

                    for (int d = auxLista.Count - 1; d >= 0; d--)
                        listaCaminho.Add(auxLista[d]);
                }

                if (listaCaminho.Count > 0)
                {
                    foreach (int ex in listaCaminho)
                        caminho += ListaGrafosNaoDirigido[indiceAcesso].Vertices[ex].Rotulo + ",";

                    int tam = caminho.Length;
                    caminho = caminho.Substring(0, caminho.Length - 1);

                    caminho += "\nPeso total: " + pesoF;
                    MessageBox.Show(caminho, "Rota disponível", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                else
                {
                    MessageBox.Show("Não existe rota para esse aeroporto, partindo da sua atual origem", "Rota indisponível", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }

            catch(IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_arvore_minima_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Grafo_Und> floresta = ListaGrafosNaoDirigido[indiceAcesso].Kruskal();
                string mensagem = "";

                for (int k = 0; k < floresta.Count; k++)
                {
                    mensagem += "Arvore " + (k + 1) + "\n";

                    mensagem += floresta[k].ListaDeAdjacencia();

                    int nArestas = 0;

                    for (int l = 0; l < floresta[k].Vertices.Length; l++)
                        nArestas += floresta[k].Vertices[l].ListaDeAdjacencia.Count;

                    nArestas /= 2;

                    mensagem += "\nSerão necessárias " + nArestas + " aeronaves para realizar os serviços de entrega entre os aeroportos" + "\n\n";
                }

                MessageBox.Show(mensagem, "Lista de adjacência com as rotas de custo minímo", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            catch (NullReferenceException ex)
            {
                MessageBox.Show("O grafo não é conexo.", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_conectividade_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<List<Vertice>> componentes = ListaGrafosDirigidos[indiceAcesso].ComponentensConexos();
                string mensagem = "";

                for (int q = 0; q < componentes.Count; q++)
                {
                    mensagem += "Componente conexo " + (q+1) + ": ";
                    for (int c = (componentes[q].Count - 1); c >= 0; c--)
                    {
                        mensagem += componentes[q][c].Rotulo + ", ";
                    }

                    mensagem += "\n";
                }

                MessageBox.Show(mensagem, "Componentes conexos do grafo", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            catch(IndexOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message);
            }

            catch(ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_ultimo_horario_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dt;
                string val = text_horario.Text;

                string[] vetSplit = val.Split(':');

                int hora = int.Parse(vetSplit[0]);
                int minuto = int.Parse(vetSplit[1]);

                string origemSt = box_aeroporto_origem_ultimo_voo.SelectedItem.ToString();
                string destinoSt = box_aeroporto_destino_ultimo_voo.SelectedItem.ToString();

                int origem = BuscarIndiceRotulo(ListaGrafosDirigidos[indiceAcesso].Vertices, origemSt);
                int destino = BuscarIndiceRotulo(ListaGrafosDirigidos[indiceAcesso].Vertices, destinoSt);

                dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hora, minuto, 0);

               string message = ListaGrafosDirigidos[indiceAcesso].BuscarUltimoHorario(dt,origem, destino);

                if (message == null)
                {
                    message = "Não há voo que cumpre os requisitos de horário";
                    MessageBox.Show(message,"Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                else
                {
                    MessageBox.Show(message, "Rota disponível", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

            catch(FormatException ex)
            {
                MessageBox.Show("Horário em formato inválido", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            catch(NullReferenceException ex)
            {
                MessageBox.Show("Não há aeroportos selecionados", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            catch(ArgumentOutOfRangeException ex)
            {
                MessageBox.Show("Parametros de origem e destino são inválidos",ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show("Não há voos disponiveis que cumprem o horário solicitado", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
