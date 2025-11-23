using Locadora.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Validadacoes;
using Locadora.Controller;

namespace Utils.Menus
{
    public class ClienteMenu
    {
        public ClienteController Controller = new ClienteController();

        private void InsertService()
        {
            string? name = Validar.ValidarInputString("Nome: ");
            if (name == null) return;

            string? email = Validar.ValidarInputString("Email: ");
            if (email == null) return;

            string? phone = Validar.ValidarInputOpcional("Telefone (opcional): ");

            Cliente customer = new Cliente(name, email, phone);

            string? docType = Validar.ValidarInputString("Tipo de documento | RG | CPF | CNH | : ");
            if (docType == null) return;

            string? docNumber = Validar.ValidarInputString("Numero do documento: ");
            if (docNumber == null) return;

            DateOnly dismissalDate = Validar.ValidarInputDateOnly("Data de Emissão [dd/MM/yyyy]: ");

            DateOnly expirationDate = Validar.ValidarInputDateOnly("Data de Validade [dd/MM/yyyy]: ");

            Documento document = new Documento(docType, docNumber, dismissalDate, expirationDate);

            try
            {
                Controller.AdicionarCliente(customer, document);
                Console.WriteLine("\n   >>>   Cliente inserido com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void SelectAllService()
        {
            Console.Clear();
            Console.WriteLine();

            try
            {
                var list = Controller.ListarClientes();

                foreach (var customer in list)
                {
                    Console.WriteLine(customer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void UpdatePhoneService()
        {
            string? email = Validar.ValidarInputString("Informe o email para busca do cliente: ");
            if (email == null) return;

            try
            {
                var vlr = Controller.BuscarClientePorEmail(email);
                if (vlr is null)
                {
                    Console.WriteLine("\nNão existe cliente com esse email cadastrado!");
                    return;
                }

                Console.WriteLine("\n\n               =-=-=   >  Cliente  <   =-=-=\n");
                Console.WriteLine(vlr + "\n");

                string? phone = Validar.ValidarInputString("Informe o telefone atualizado: ");
                if (phone == null) return;

                Controller.AtualizarTelefoneCliente(phone, email);

                Console.WriteLine("\n >>>  Telefone atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void UpdateDocumentService()
        {
            string? email = Validar.ValidarInputString("Informe o email para busca do cliente: ");
            if (email == null) return;


            try
            {
                var vlr = Controller.BuscarClientePorEmail(email);
                if (vlr is null)
                {
                    Console.WriteLine("\nNão existe cliente com esse email cadastrado!");
                    return;
                }

                Console.WriteLine("\n\n               =-=-=   >  Cliente  <   =-=-=\n");
                Console.WriteLine(vlr + "\n");

                Console.WriteLine(" > Preencha os campos atualizados do documento do cliente\n");

                string? docType = Validar.ValidarInputString("Tipo de documento | RG | CPF | CNH | : ");
                if (docType == null) return;

                string? docNumber = Validar.ValidarInputString("Numero do documento: ");
                if (docNumber == null) return;

                DateOnly dismissalDate = Validar.ValidarInputDateOnly("Data de Emissão [dd/MM/yyyy]: ");

                DateOnly expirationDate = Validar.ValidarInputDateOnly("Data de Validade [dd/MM/yyyy]: ");

                Documento document = new Documento(docType, docNumber, dismissalDate, expirationDate);

                Controller.AtualizarDocumentoCliente(email, document);

                Console.WriteLine("\n >>>  Documento atualizado com sucesso!\n");
                vlr = Controller.BuscarClientePorEmail(email);
                Console.WriteLine(vlr.Documento.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void DeleteService()
        {
            string? email = Validar.ValidarInputString("Informe o email para busca do cliente: ");
            if (email == null) return;

            try
            {
                var vlr = Controller.BuscarClientePorEmail(email);
                if (vlr is null)
                {
                    Console.WriteLine("\nNão existe cliente com esse email cadastrado!");
                    return;
                }

                Console.WriteLine("\n=-=-=-=   >   Cliente   <   =-=-=-=\n");
                Console.WriteLine(vlr + "\n");

                Console.Write("Tem certeza que deseja deletar o cliente? [S/N]: ");
                string res = Console.ReadLine()!.ToUpper();

                while (res is not "S" && res is not "N")
                {
                    Console.Write("Error! Informe apenas [S/N] pra continuar: ");
                    res = Console.ReadLine()!.ToUpper();
                }

                if (res == "N")
                {
                    Console.WriteLine("\nEncerrando a operação de deletar...");
                    return;
                }

                Controller.DeletarCliente(email);
                Console.WriteLine("\n >>>  Cliente deletado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public void MenuCliente()
        {
            int opcao = 0;
            do
            {
                Console.Clear();
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-|");
                Console.WriteLine(" |                   >      Cliente      <                   |");
                Console.WriteLine(" |-----------------------------------------------------------|");
                Console.WriteLine(" | [ 1 ] Cadastrar Cliente    |   [ 2 ] Exibir Clientes      |");
                Console.WriteLine(" | [ 3 ] Atualizar Telefone   |   [ 4 ] Atualizar Documento  |");
                Console.WriteLine(" | [ 5 ] Deletar Cliente      |   [ 6 ] Voltar               |");
                Console.WriteLine(" |-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-|");
                Console.WriteLine();
                Console.Write("  >>> Informe o menu desejado: ");
                string entrada = Console.ReadLine()!;
                bool conversao = int.TryParse(entrada, out opcao);
                Console.WriteLine("---------------------------------------");

                switch (opcao)
                {
                    case 1:
                        InsertService();
                        break;
                    case 2:
                        SelectAllService();
                        break;
                    case 3:
                        UpdatePhoneService();
                        break;
                    case 4:
                        UpdateDocumentService();
                        break;
                    case 5:
                        DeleteService();
                        break;
                    case 6:
                        return;
                    default:
                        Console.WriteLine("\nOpção Inválida. Tente novamente.");
                        break;
                }

                Console.Write("\n  >  Pressione qualquer Tecla para prosseguir ");
                Console.ReadLine();

            } while (true);
        }
    }
}
