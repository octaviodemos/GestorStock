using GestorStock.Data;
using GestorStock.Models;
using System.Windows;


namespace GestorStock
{

    public partial class FornecedorView : Window
    {
        private readonly FornecedorRepository _repo = new FornecedorRepository();

        public FornecedorView()
        {
            InitializeComponent();
            CarregarFornecedores();
        }

        private void CarregarFornecedores()
        {
            List<Fornecedor> fornecedores = _repo.GetAll();
            dgFornecedores.ItemsSource = fornecedores;
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            var form = new FornecedorForm();
            if (form.ShowDialog() == true)
            {
                CarregarFornecedores();
            }
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (dgFornecedores.SelectedItem is Fornecedor selecionado)
            {
                var form = new FornecedorForm(selecionado);
                if (form.ShowDialog() == true)
                {
                    CarregarFornecedores();
                }
            }
        }

        private void Remover_Click(object sender, RoutedEventArgs e)
        {
            if (dgFornecedores.SelectedItem is Fornecedor selecionado)
            {
                _repo.Delete(selecionado.Id);
                CarregarFornecedores();
            }
        }
    }
}

