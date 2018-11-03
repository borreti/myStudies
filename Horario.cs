using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_Grafos
{
    class Horario
    {
        private int hora;
        private int minuto;

        public int Hora
        {
            get
            {
                return hora;
            }

            set
            {
                if (value < 0)
                    hora = 0;

                else
                    hora = value;
            }
        }

        public int Minuto
        {
            get
            {
                return minuto;
            }

            set
            {
                minuto += value;

                if (minuto > 59)
                {
                    int horasExtras = minuto / 60;
                    int minutosRestantes = minuto % 60;

                    Hora += horasExtras;

                    minuto = minutosRestantes;

                    while (hora > 23)
                    {
                        int restoHoras = hora - 24;
                        hora = restoHoras;
                    }
                }
            }
        }

        public Horario(int Hora, int Minuto)
        {
            this.Hora = Hora;
            this.Minuto = Minuto;
        }

        public Horario(int Minuto)
        {
            this.Minuto = Minuto;
        }

        public string RetornarHora()
        {
            string value = Hora + ":" + Minuto;
            return value;
        }

        public int HorasParaMinutos()
        {
            return Hora * 60;
        }

        public int ValorEmMinutos()
        {
            int minutos = Minuto + (Hora * 60);
            return minutos;
        }

        //método feito para auxiliar o Dijkstra no cálculo de tempo de viagem total
        //retorna um valor em minutos
        public static int CalcularTempoEmEspera(List<Horario> lista, Horario horario)
        {
            //percorre toda a lista de horários buscando horários que estejam acima do horário atual
            //se não houver horários para hoje (lista.Count == 0), deve ser buscado o menor horário da lista
            List<Horario> HorariosHoje = new List<Horario>();

            for (int u = 0; u < lista.Count; u++)
            {
                int val = lista[u].CompararHorarios(horario);

                if (val == 1)
                    HorariosHoje.Add(lista[u]);
            }

            Horario menorHorario = new Horario(23, 59);

            if (HorariosHoje.Count > 0)
            {
                for(int u = 0; u < HorariosHoje.Count; u++)
                {
                    if (HorariosHoje[u].CompararHorarios(menorHorario) == -1)
                        menorHorario = HorariosHoje[u];
                }

                return menorHorario.ValorEmMinutos() - horario.ValorEmMinutos();
            }

            else
            {
                for (int u = 0; u < lista.Count; u++)
                {
                    if (lista[u].CompararHorarios(menorHorario) == -1)
                        menorHorario = lista[u];
                }

                return horario.ValorEmMinutos() - menorHorario.ValorEmMinutos();
            }
        }

        //se o valor da instância for maior que o parametro, retorna 1, se for igual, retorna 0, e se for menor, retorna -1
        public int CompararHorarios(Horario horarioParaComparar)
        {
            if (hora > horarioParaComparar.hora)
            {
                return 1;
            }

            else if (hora == horarioParaComparar.hora)
            {
                if (minuto > horarioParaComparar.minuto)
                    return 1;

                else if (minuto == horarioParaComparar.minuto)
                    return 0;

                else
                    return -1;
            }

            else
            {
                return -1;
            }
        }
    }
}
