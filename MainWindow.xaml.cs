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
        int nGrafos = 0;
        List<Grafo_Und> ListaGrafosNaoDirigido = new List<Grafo_Und>();
        List<Grafo_Dir> ListaGrafosDirigidos = new List<Grafo_Dir>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void box_grafo_selecionado_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = int.Parse(box_grafo_selecionado.SelectedItem.ToString());
            index--;
            rotulo_lista_adjacencia.Content = ListaGrafosDirigidos[index].ListaDeAdjacencia();
        }

        private void ler_arquivo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string arqName = text_arq_nome.Text;
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

                    ParOrdenado par = new ParOrdenado(x, y, peso);
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

        private int BuscarIndiceRotulo(string [] vetorRotulos, string rotuloAtual)
        {
            for (int t = 0; t < vetorRotulos.Length; t++)
            {
                if (vetorRotulos[t] == rotuloAtual)
                    return t;
            }

            return -1;
        }

        private void btn_viagem_minima_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int tipoPeso = 0;

                if (radio_0.IsChecked == true)
                    tipoPeso = 0;

                else if (radio_1.IsChecked == true)
                    tipoPeso = 1;

                else if (radio_2.IsChecked == true)
                    tipoPeso = 2;

                List<int> caminho;

                if (tipoPeso < 2)
                {
                    caminho = ListaGrafosNaoDirigido[int.Parse(box_grafo_selecionado.SelectedItem.ToString()) - 1].Dijkstra(1, 2, tipoPeso);

                    string x = "";

                    foreach (int ex in caminho)
                        x += ex + ", ";

                    MessageBox.Show(x);
                }

            }

            catch(NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
