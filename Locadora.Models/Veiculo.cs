using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Models
{
    public class Veiculo
    {
        public Veiculo(int categoriaID, string placa, string marca, string modelo, int ano, string statusVeiculo)
        {
            CategoriaID = categoriaID;
            Placa = placa;
            Marca = marca;
            Modelo = modelo;
            Ano = ano;
            StatusVeiculo = statusVeiculo;
        }

        public int VeiculoID {  get; private set; }
        public int CategoriaID {  get; private set; }
        public string Placa {  get; private set; }
        public string Marca { get; private set; }
        public string Modelo { get; private set; }
        public int Ano { get; private set; }
        public string StatusVeiculo { get; private set; }

        public void SetVeiculoID(int veiculoID)
        {
            VeiculoID = veiculoID;
        }

        public void SetStatusVeiculo(string statusVeiculo)

        {
            StatusVeiculo = statusVeiculo;
        }
        public override string ToString()
        {
            return $"Placa: {Placa} \nModelo: {Modelo} \nModelo: {Modelo} \nAno: {Ano} \nStatus: {StatusVeiculo}\n";
        }
    }
}
