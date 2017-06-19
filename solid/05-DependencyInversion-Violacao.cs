using System;
using System.Data.SqlClient;

namespace Solid
{
    /// <summary>
    /// A. High-level modules should not depend on low-level modules. Both should depend on abstractions.
    ///
    /// B. Abstractions should not depend on details.Details should depend on abstractions.
    /// </summary>
    public class DependencyInversionViolation
    {
        // A - High-level modules should depend on abstractions.
        public interface ITransacaoFactory
        {
            // Violação: Sempre que possível interfaces devem depender de interfaces. Nunca de tipos concretos.

            SqlTransaction AbrirTransacao(SqlConnection conn);
        }

        public class TransacaoFactory : ITransacaoFactory
        {
            // Desta forma nossa factory só funciona para SQL Server.
            public SqlTransaction AbrirTransacao(SqlConnection conn)
            {
                // Retorna uma nova transação de SQL Server.
                throw new NotImplementedException();
            }
        }

        public class ContextoTransacionado
        {
            // B. Details should depend on abstractions.
            public void ExecutarComandoTransacionado(string cmd, TransacaoFactory transacaoFactory)
            {
                // Violação: estamos dependendo de um tipo concreto (TransacaoFactory)
                // Dessa forma caso venha a existir uma implementação de ITransacaoFactory que tenha uma política de retry
                // não conseguimos invocar esse método utilizando essa nova implementação.
                using (var tx = transacaoFactory.AbrirTransacao(null))
                {
                    tx.Commit();
                }
            }
        }
    }
}