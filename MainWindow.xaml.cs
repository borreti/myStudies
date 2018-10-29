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
using Grafos;
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

                while (!reader.EndOfStream)
                {
                    int x, y, direcao;
                    Peso peso;
                    string linha = reader.ReadLine();
                    string[] vetSplit = linha.Split(';');
                    x = int.Parse(vetSplit[0]) - 1;
                    y = int.Parse(vetSplit[1]) - 1;
                    peso = TratarPesos(vetSplit[3], vetSplit[4]);
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

                text_arq_nome.Clear();

                grafoDir = new Grafo_Dir(nVertices, lista);
                grafoUnd = new Grafo_Und(nVertices, lista);
                ListaGrafosDirigidos.Add(grafoDir);
                ListaGrafosNaoDirigido.Add(grafoUnd);

                nGrafos++;
                box_grafo_selecionado.Items.Add(nGrafos);
                MessageBox.Show("Novo grafo incluido na memória", "Arquivo foi lido com sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            catch(ArgumentException ex)
            {
                MessageBox.Show(ex.Message,"Erro ao tentar ler o arquivo", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            catch(FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Erro ao tentar ler o arquivo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Peso TratarPesos(string dist, string duracao_Voo)
        {
            string[] vetSplit = dist.Split(':');

            int distancia = int.Parse(duracao_Voo), duracaoVoo = 0;

            duracaoVoo += int.Parse(vetSplit[0]) * 60;
            duracaoVoo += int.Parse(vetSplit[1]);

            Peso p = new Peso(distancia, duracaoVoo);

            return p;
        }
    }
}
