using System;
using System.Collections.Generic;

namespace Solid
{
    /// <summary>
    /// No client should be forced to depend on methods it does not use.
    /// </summary>
    public class InterfaceSegregationViolation
    {
        // Exemplo de Violação na implementação de um contrato.
        public interface IAnimal
        {
            void Voar();

            void Comer();

            void Nadar();
        }

        public class Pato : IAnimal
        {
            public void Voar()
            {
                Console.WriteLine("Flap flap.");
            }

            public void Comer()
            {
                Console.WriteLine("Chomp chomp.");
            }

            public void Nadar()
            {
                Console.WriteLine("Swim swim.");
            }
        }

        public class Peixe : IAnimal
        {
            public void Voar()
            {
                // Por que preciso depender dessa operação se não realizo ela?
                throw new NotSupportedException();
            }

            public void Comer()
            {
                Console.WriteLine("Chomp chomp.");
            }

            public void Nadar()
            {
                Console.WriteLine("Swim swim.");
            }
        }

        #region Exemplos do dia a dia

        public class ExemploSimples
        {
            // Exemplo de Violação na implementação de um "cliente"

            public void PercorrerLista(IList<string> lista)
            {
                // Aqui estamos só percorrendo a lista, por que preciso depender de operações como Add, Contains, IndexOf?
                // Nesse caso um IEnumerable<string> resolveria.
                foreach (var str in lista)
                {
                    Console.WriteLine(str.ToUpper());
                }
            }
        }

        public class RelatorioNotas
        {
            private IRepository _repository;

            public RelatorioNotas(IRepository repository)
            {
                _repository = repository;
            }

            public IReport<NotaFiscal> GerarRelatorio()
            {
                // Só uso operações de leitura, porque preciso depender de operações de escrita (Save e Delete)?
                var notas = _repository.GetAll<NotaFiscal>();
                return new Report<NotaFiscal>(notas);
            }
        }

        public interface IRepository
        {
            void Save<T>(T entity);

            T Get<T>(long id);

            IEnumerable<T> GetAll<T>();

            void Delete<T>(T entity);
        }

        public interface IReport<T>
        {
            IEnumerable<T> Data { get; }
        }

        public class NotaFiscal { }

        public class Report<T> : IReport<T>
        {
            public Report(IEnumerable<T> data)
            {
                Data = data;
            }

            public IEnumerable<T> Data { get; private set; }
        }

        #endregion Exemplos do dia a dia
    }
}