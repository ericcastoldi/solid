using System;
using System.Collections.Generic;

namespace Solid
{
    /// <summary>
    /// "Software entities (classes, modules, functions, etc.) should be open for extension, but closed for modification"
    /// </summary>
    public class OpenClosedViolationMadeRight
    {
        public class Compra
        {
            public Cliente Cliente { get; set; }

            public decimal ValorTotal { get; set; }

            public IList<Item> Itens { get; set; }
        }

        public class Cliente
        {
            public string Nome { get; set; }

            public DateTime DataNascimento { get; set; }

            public DateTime ClienteDesde { get; set; }
        }

        public class Item
        {
            public string Nome { get; set; }
            public decimal Preco { get; set; }
        }

        /// <summary>
        /// Implementação Simples, permite extensão por override (método AplicarDesconto é virtual).
        /// </summary>
        public class AplicacaoDescontoClienteAntigo : IAplicacaoDesconto
        {
            public virtual decimal AplicarDesconto(Compra compra)
            {
                var clienteAntigo = (DateTime.Today - compra.Cliente.ClienteDesde).Days > 1825;

                // Cliente a mais de 5 anos ganha 5% de desconto
                return clienteAntigo
                    ? compra.ValorTotal - (0.05m * compra.ValorTotal)
                    : compra.ValorTotal;
            }
        }

        /// <summary>
        /// Implementação de extensão via decorator, injetando uma segunda estratégia de aplicação de desconto via construtor;
        /// </summary>
        public class AplicacaoDescontoAniversario : IAplicacaoDesconto
        {
            private readonly IAplicacaoDesconto _aplicacaoDesconto;

            public AplicacaoDescontoAniversario()
                : this(null)
            {
            }

            public AplicacaoDescontoAniversario(IAplicacaoDesconto aplicacaoDesconto)
            {
                _aplicacaoDesconto = aplicacaoDesconto;
            }

            public decimal AplicarDesconto(Compra compra)
            {
                // Se recebemos uma outra estratégia de desconto,
                // aplicamos ela primeiro e depois aplicamos a estratégia de aniversário.
                var valor = _aplicacaoDesconto?.AplicarDesconto(compra)
                    ?? compra.ValorTotal; // Se não recebemos utilizamos o valor puro.

                var aniversario = compra.Cliente.DataNascimento.Date == DateTime.Today;

                // Cliente de aniversário ganha 10% de desconto
                return aniversario
                    ? valor - (0.1m * compra.ValorTotal)
                    : valor;
            }
        }

        /// <summary>
        /// Segundo exemplo de extensão via Decorator onde ao invés de combinar as duas estratégias, utilizamos ou uma ou outra.
        /// </summary>
        public class AplicacaoDescontoClienteAmigoPessoal : IAplicacaoDesconto
        {
            private readonly IAplicacaoDesconto _aplicacaoDesconto;

            public AplicacaoDescontoClienteAmigoPessoal(IAplicacaoDesconto aplicacaoDesconto)
            {
                _aplicacaoDesconto = aplicacaoDesconto;
            }

            public decimal AplicarDesconto(Compra compra)
            {
                return compra.Cliente.Nome == "Joesley Batista"
                    ? decimal.Zero
                    : _aplicacaoDesconto.AplicarDesconto(compra);
            }
        }

        public class ExemplosDeInstanciacaoDaAplicacaoDesconto
        {
            public void Instanciando()
            {
                // Aqui temos uma aplicação de desconto que só leva em consideração o tempo de casa do cliente.
                var descontoClienteAntigo = new AplicacaoDescontoClienteAntigo();

                // Aqui temos uma aplicação de desconto que só leva em consideração o aniversario do cliente.
                var descontoClienteAniversario = new AplicacaoDescontoAniversario();

                // Aqui temos uma aplicação de desconto que leva em consideração tempo de casa E aniversário
                var descontoClienteAntigoDeAniversario = new AplicacaoDescontoAniversario(new AplicacaoDescontoClienteAntigo());

                // Aqui temos uma aplicação de desconto que leva em consideração
                // tempo de casa, aniversário E se o cliente é um amigo pessoal do gerente
                var descontoClienteAmigo = new AplicacaoDescontoClienteAmigoPessoal(descontoClienteAntigoDeAniversario);

                // E assim conseguimos ESTENDER E COMBINAR os comportamentos sem alterar nenhuma das classes.
                // Ou seja, estão abertas a extensão (tanto por Decorator quanto por Herança) e fechadas a modificação,
                // já que as regras de desconto de cada classe não precisam ser alteradas em nenhum momento.
            }
        }

        public interface IAplicacaoDesconto
        {
            decimal AplicarDesconto(Compra compra);
        }
    }
}