using System;
using System.Collections.Generic;

namespace Solid
{
    /// <summary>
    /// "Software entities (classes, modules, functions, etc.) should be open for extension, but closed for modification"
    /// </summary>
    public class OpenClosedViolation
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

        public class ServicoDesconto
        {
            public decimal AplicarDesconto(Compra compra)
            {
                // Se surgirem novos tipos de desconto, preciso alterar meu serviço de desconto para tratar.
                // Ou seja, minha classe não está fechada para alterações, sempre que surgir um novo tipo de desconto devo alterar minha classe.

                var aniversario = compra.Cliente.DataNascimento.Date == DateTime.Today;
                if (aniversario)
                {
                    // Cliente de aniversário ganha 10% de desconto
                    return compra.ValorTotal - (0.1m * compra.ValorTotal);
                }

                var clienteAntigo = (DateTime.Today - compra.Cliente.ClienteDesde).Days > 1825;
                if (clienteAntigo)
                {
                    // Cliente a mais de 5 anos ganha 5% de desconto
                    return compra.ValorTotal - (0.05m * compra.ValorTotal);
                }

                return compra.ValorTotal;
            }
        }
    }
}