using System;
using System.Collections.Generic;

namespace Solid
{
    /// <summary>
    /// No client should be forced to depend on methods it does not use.
    /// </summary>
    public class InterfaceSegregationViolationMadeRight
    {
        // Correção da Violação na implementação de um contrato.
        public interface IAnimal
        {
            void Comer();
        }

        public interface IAnimaisAquaticos : IAnimal
        {
            void Nadar();
        }

        public interface IAves : IAnimal
        {
            void Voar();
        }

        public class Pato : IAnimaisAquaticos, IAves
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

        public class Peixe : IAnimaisAquaticos
        {
            // Segregando as interfaces não preciso mais implementar um método que não suporto
            //public void Voar()
            //{
            //    throw new NotSupportedException();
            //}

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
            // Correção de violação na implementação de um "cliente"

            public void PercorrerLista(IEnumerable<string> enumeravel)
            {
                // Alterando para IEnumerable não precisei alterar nem uma linha do método e agora posso passar
                // arrays, listas, queriables, collections, etc
                foreach (var str in enumeravel)
                {
                    Console.WriteLine(str.ToUpper());
                }
            }
        }

        public class RelatorioNotas
        {
            private IReadOnlyRepository _repository;

            public RelatorioNotas(IReadOnlyRepository repository)
            {
                _repository = repository;
            }

            public IReport<NotaFiscal> GerarRelatorio()
            {
                // Agora dependo de um repositório somente leitura e estou seguro de que
                // caso qualquer operação de escrita sofra modificações não sofrerei de efeitos colaterais.
                var notas = _repository.GetAll<NotaFiscal>();
                return new Report<NotaFiscal>(notas);
            }
        }

        public interface IReadOnlyRepository
        {
            T Get<T>(long id);

            IEnumerable<T> GetAll<T>();
        }

        public interface IOperableRepository
        {
            void Save<T>(T entity);

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