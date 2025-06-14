using GestorStock.Data;
using GestorStock.Models;
using System;
using System.Data;
using System.Windows;

namespace GestorStock
{
    public partial class FornecedorForm : Window
    {
        private readonly FornecedorRepository _fornecedorRepo = new FornecedorRepository();
        private readonly EnderecoRepository _enderecoRepo = new EnderecoRepository();

        public Fornecedor Fornecedor { get; private set; }
        private Endereco Endereco { get; set; }

        private bool isEditMode;

        public FornecedorForm(Fornecedor fornecedor = null)
        {
            InitializeComponent();

            if (fornecedor != null) // Modo Edição
            {
                isEditMode = true;
                Fornecedor = fornecedor;

                Endereco = _enderecoRepo.GetById(fornecedor.IdEndereco);
                if (Endereco == null)
                {
                    Endereco = new Endereco();
                }

                // Preenche os campos da tela (fornecedor)
                txtNome.Text = Fornecedor.Nome;
                txtCNPJ.Text = Fornecedor.CNPJ;
                txtTelefone.Text = Fornecedor.Telefone;
                txtEmail.Text = Fornecedor.Email;

                // Preenche os campos da tela (endereço)
                txtEnderecoId.Text = Endereco.IdEndereco.ToString();
                txtCEP.Text = Endereco.CEP;
                txtCidade.Text = Endereco.Cidade;
                txtRua.Text = Endereco.Rua;
                txtNumero.Text = Endereco.Numero;
                txtUF.Text = Endereco.UF;
            }
            else // Modo Adição
            {
                isEditMode = false;
                Fornecedor = new Fornecedor();
                Endereco = new Endereco();

                txtEnderecoId.Text = "(Será gerado ao salvar)";
            }
        }

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            // Povoa os objetos com os dados atualizados da tela
            Endereco.CEP = txtCEP.Text;
            Endereco.Cidade = txtCidade.Text;
            Endereco.Rua = txtRua.Text;
            Endereco.Numero = txtNumero.Text;
            Endereco.UF = txtUF.Text;

            Fornecedor.Nome = txtNome.Text;
            Fornecedor.CNPJ = txtCNPJ.Text;
            Fornecedor.Telefone = txtTelefone.Text;
            Fornecedor.Email = txtEmail.Text;

            // Inicia conexão e transação para garantir a integridade dos dados
            using var conn = Database.GetConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                if (isEditMode)
                {
                    _enderecoRepo.Update(Endereco, conn, transaction);
                    _fornecedorRepo.Update(Fornecedor, conn, transaction);
                }
                else
                {
                    int novoEnderecoId = _enderecoRepo.Add(Endereco, conn, transaction);
                    Fornecedor.IdEndereco = novoEnderecoId;
                    _fornecedorRepo.Add(Fornecedor, conn, transaction);
                }

                transaction.Commit();

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show($"Ocorreu um erro ao salvar os dados. Nenhuma alteração foi feita.\n\nErro: {ex.Message}", "Erro de Banco de Dados", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}