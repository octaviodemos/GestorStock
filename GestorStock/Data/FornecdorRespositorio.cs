using GestorStock.Models;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace GestorStock.Data
{
    public class FornecedorRepository
    {
        public List<Fornecedor> GetAll()
        {
            using var conn = Database.GetConnection();
            return conn.Query<Fornecedor>(@"
                SELECT 
                    id_fornecedor AS Id, 
                    nome AS Nome, 
                    cnpj AS CNPJ, 
                    telefone AS Telefone, 
                    email AS Email, 
                    id_endereco AS IdEndereco 
                FROM fornecedor
            ").ToList();
        }

        public void Add(Fornecedor f, IDbConnection connection, IDbTransaction transaction)
        {
            var sql = "INSERT INTO fornecedor (nome, cnpj, telefone, email, id_endereco) VALUES (@Nome, @CNPJ, @Telefone, @Email, @IdEndereco)";
            connection.Execute(sql, f, transaction: transaction);
        }

        public void Update(Fornecedor f, IDbConnection connection, IDbTransaction transaction)
        {
            var sql = @"
                UPDATE fornecedor SET 
                    nome = @Nome, 
                    cnpj = @CNPJ, 
                    telefone = @Telefone, 
                    email = @Email, 
                    id_endereco = @IdEndereco 
                WHERE id_fornecedor = @Id";
            connection.Execute(sql, f, transaction: transaction);
        }

        public void Delete(int id)
        {
            using var conn = Database.GetConnection();
            conn.Execute("DELETE FROM fornecedor WHERE id_fornecedor = @id", new { id });
        }
    }
}