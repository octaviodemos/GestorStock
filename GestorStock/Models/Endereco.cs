namespace GestorStock.Models
{
    public class Endereco
    {
        public int IdEndereco { get; set; }
        public string CEP { get; set; }
        public string Cidade { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string UF { get; set; }
    }
}