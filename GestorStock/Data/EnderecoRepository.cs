using GestorStock.Models;
using Dapper;
using System.Data;

namespace GestorStock.Data
{
    public class EnderecoRepository
    {
        public int Add(Endereco endereco, IDbConnection connection, IDbTransaction transaction)
        {
            var sql = @"
                INSERT INTO endereco (cep, cidade, rua, numero, uf) 
                VALUES (@CEP, @Cidade, @Rua, @Numero, @UF)
                RETURNING id_endereco;";
            return connection.ExecuteScalar<int>(sql, endereco, transaction: transaction);
        }

        public void Update(Endereco endereco, IDbConnection connection, IDbTransaction transaction)
        {
            var sql = @"
                UPDATE endereco SET
                    cep = @CEP,
                    cidade = @Cidade,
                    rua = @Rua,
                    numero = @Numero,
                    uf = @UF
                WHERE id_endereco = @IdEndereco;";
            connection.Execute(sql, endereco, transaction: transaction);
        }

        public Endereco GetById(int id)
        {
            using var conn = Database.GetConnection();
            var sql = @"
                SELECT 
                    id_endereco AS IdEndereco,
                    cep AS CEP,
                    cidade AS Cidade,
                    rua AS Rua,
                    numero AS Numero,
                    uf AS UF
                FROM endereco WHERE id_endereco = @id;";
            return conn.QueryFirstOrDefault<Endereco>(sql, new { id });
        }
    }
}