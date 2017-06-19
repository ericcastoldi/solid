using System;
using System.Data;

namespace Solid
{
    /// <summary>
    /// A. High-level modules should not depend on low-level modules. Both should depend on abstractions.
    ///
    /// B. Abstractions should not depend on details.Details should depend on abstractions.
    /// </summary>
    public class DependencyInversionViolationMadeRight
    {
        // A - High-level modules should depend on abstractions.
        public interface ITransacaoFactory
        {
            // Violação Resolvida: Passamos a depender das abstrações IDbTransaction e IDbConnection

            IDbTransaction AbrirTransacao(IDbConnection conn);
        }

        public class TransacaoFactory : ITransacaoFactory
        {
            // Desta forma nossa factory funciona para qualquer implementação de IDbConnection
            public IDbTransaction AbrirTransacao(IDbConnection conn)
            {
                // Retorna uma nova transação
                throw new NotImplementedException();
            }
        }

        public class TransacaoComRetryFactory : ITransacaoFactory
        {
            private int _retries = 0;

            // Desta forma nossa factory funciona para qualquer implementação de IDbConnection
            public IDbTransaction AbrirTransacao(IDbConnection conn)
            {
                do
                {
                    try
                    {
                        // Abre uma nova transação
                    }
                    catch (Exception)
                    {
                        _retries++;
                    }
                } while (_retries <= 3);

                throw new NotImplementedException();
            }
        }

        public class ContextoTransacionado
        {
            // B. Details should depend on abstractions.
            public void ExecutarComandoTransacionado(string cmd, ITransacaoFactory transacaoFactory)
            {
                // Violação Resolvida: passamos a depender de uma abstração (ITransacaoFactory)

                // Agora conseguimos funcionar com qualquer implementação concreta de ITransacaoFactory
                using (var tx = transacaoFactory.AbrirTransacao(null))
                {
                    tx.Commit();
                }
            }
        }
    }
}