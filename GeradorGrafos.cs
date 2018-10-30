using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Trabalho_Grafos
{
    class GeradorGrafos
    {
        private Peso TratarPesos(string dist, string duracao_Voo)
        {
            string[] vetSplit = dist.Split(':');

            int distancia = int.Parse(duracao_Voo), duracaoVoo = 0;

            duracaoVoo += int.Parse(vetSplit[0]) * 60;
            duracaoVoo += int.Parse(vetSplit[1]);

            Peso p = new Peso(distancia, duracaoVoo);

            return p;
        }

        public Grafo_Und MontagemGrafoUnd(string arqName)
        {
            Grafo_Und grafo;
            StreamReader reader = new StreamReader(arqName);
            int nVertices = int.Parse(reader.ReadLine());
            List<ParOrdenado> lista = new List<ParOrdenado>();
            while (!reader.EndOfStream)
            {
                int x, y;
                Peso peso;
                string linha = reader.ReadLine();
                string[] vetSplit = linha.Split(';');
                x = int.Parse(vetSplit[0]) - 1;
                y = int.Parse(vetSplit[1]) - 1;
                peso = TratarPesos(vetSplit[3], vetSplit[4]);
                ParOrdenado par = new ParOrdenado(x, y, peso);
                lista.Add(par);
            }
            reader.Close();
            return grafo = new Grafo_Und(nVertices, lista);
        }

        public Grafo_Dir MontagemGrafoDir(string arqName)
        {
            Grafo_Dir grafo;

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

            return grafo = new Grafo_Dir(nVertices, lista);
        }
    }
}
