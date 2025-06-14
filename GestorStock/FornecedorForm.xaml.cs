using GestorStock.Data;
using GestorStock.Models;
using System;
using System.Data;
using System.Windows;

namespace GestorStock
{
    public partial class FornecedorForm : Window
    {
        // Repositórios para acesso ao banco de dados
        private readonly FornecedorRepository _fornecedorRepo = new FornecedorRepository();
        private readonly EnderecoRepository _enderecoRepo = new EnderecoRepository();

        // Propriedades para guardar os dados do fornecedor e endereço
        public Fornecedor Fornecedor { get; private set; }
        private Endereco Endereco { get; set; }

        // Variável para controlar se o formulário está em modo de edição ou adição
        private bool isEditMode;

        // Construtor da janela
        public FornecedorForm(Fornecedor fornecedor = null)
        {
            InitializeComponent();

            if (fornecedor != null) // Entra aqui se estiver no MODO EDIÇÃO
            {
                isEditMode = true;
                Fornecedor = fornecedor;

                // Busca o endereço correspondente no banco
                Endereco = _enderecoRepo.GetById(fornecedor.IdEndereco);
                if (Endereco == null)
                {
                    Endereco = new Endereco(); // Garante que o objeto não seja nulo
                }

                // Preenche todos os campos da tela com os dados existentes
                txtNome.Text = Fornecedor.Nome;
                txtCNPJ.Text = Fornecedor.CNPJ;
                txtTelefone.Text = Fornecedor.Telefone;
                txtEmail.Text = Fornecedor.Email;

                txtEnderecoId.Text = Endereco.IdEndereco.ToString();
                txtCEP.Text = Endereco.CEP;
                txtCidade.Text = Endereco.Cidade;
                txtRua.Text = Endereco.Rua;
                txtNumero.Text = Endereco.Numero;
                txtUF.Text = Endereco.UF;
            }
            else // Entra aqui se estiver no MODO ADIÇÃO
            {
                isEditMode = false;
                Fornecedor = new Fornecedor();
                Endereco = new Endereco();

                // Informa ao usuário que o ID será gerado depois
                txtEnderecoId.Text = "(Será gerado ao salvar)";
            }
        }

        // Método acionado pelo clique no botão "Salvar"
        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            // Pega os valores dos campos da tela e atualiza os objetos
            Endereco.CEP = txtCEP.Text;
            Endereco.Cidade = txtCidade.Text;
            Endereco.Rua = txtRua.Text;
            Endereco.Numero = txtNumero.Text;
            Endereco.UF = txtUF.Text;

            Fornecedor.Nome = txtNome.Text;
            Fornecedor.CNPJ = txtCNPJ.Text;
            Fornecedor.Telefone = txtTelefone.Text;
            Fornecedor.Email = txtEmail.Text;

            // Inicia a conexão e a transação com o banco de dados
            using var conn = Database.GetConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                if (isEditMode) // Se for edição, atualiza os registros existentes
                {
                    _enderecoRepo.Update(Endereco, conn, transaction);
                    _fornecedorRepo.Update(Fornecedor, conn, transaction);
                }
                else // Se for adição, cria novos registros
                {
                    // Cria o endereço primeiro para obter o ID
                    int novoEnderecoId = _enderecoRepo.Add(Endereco, conn, transaction);
                    Fornecedor.IdEndereco = novoEnderecoId; // Associa o novo ID ao fornecedor

                    // Cria o fornecedor
                    _fornecedorRepo.Add(Fornecedor, conn, transaction);
                }

                // Se todas as operações deram certo, confirma a transação
                transaction.Commit();

                DialogResult = true; // Informa à janela anterior que a operação foi um sucesso
                Close(); // Fecha o formulário
            }
            catch (Exception ex)
            {
                // Se qualquer erro ocorreu, desfaz todas as operações
                transaction.Rollback();
                MessageBox.Show($"Ocorreu um erro ao salvar os dados. Nenhuma alteração foi feita.\n\nErro: {ex.Message}", "Erro de Banco de Dados", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Método acionado pelo clique no botão "Cancelar"
        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            // Informa à janela anterior que a operação foi cancelada
            DialogResult = false;
            Close(); // Fecha o formulário
        }
    }
}