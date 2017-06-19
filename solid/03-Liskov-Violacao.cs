using System;
using System.Collections.Generic;
using System.Linq;

namespace Solid
{
    /// <summary>
    /// If S is a subtype of T, then objects of type T may be replaced with objects of type S without
    /// altering any of the desirable properties of T (correctness, task performed, etc.).
    ///
    /// It is a semantic rather than merely syntactic relation because
    /// it intends to guarantee semantic interoperability of types in a hierarchy
    /// </summary>
    public class LiskovViolation
    {
        public interface IProgramador
        {
            string Code(Especificacao spec, IEnumerable<Coffee> xicarasCafe);
        }

        public interface IAnalistaSistemas : IProgramador
        {
            Especificacao AnalisarRequisitos(IEnumerable<IRequisito> requisitos);
        }

        public class ProgramadorCSharp : IProgramador
        {
            public string Code(Especificacao spec, IEnumerable<Coffee> xicarasCafe)
            {
                return
                    @"public class Program
                      {
                          static void Main(string[] args)
                          {
                                Console.WriteLine(""M$ Rules!"");
                          }
                      }";
            }
        }

        public class AnalistaVelhaco : IAnalistaSistemas
        {
            public string Code(Especificacao spec, IEnumerable<Coffee> xicarasCafe)
            {
                // Aqui estamos violando o princípio de Liskov.
                // Se no contrato (interface) de Analista de Sistemas estamos derivando de Programador,
                // esperamos que esse analista saiba programar.
                throw new NotSupportedException("Ah, nem gosto de programar =/");
            }

            public Especificacao AnalisarRequisitos(IEnumerable<IRequisito> requisitos)
            {
                return new Especificacao("Adoro fazer especificacao, yay o/");
            }
        }

        public class EmpresaFicticia : ISoftwareHouse
        {
            public void DesenvolverSistema(IProgramador programador, IEnumerable<Especificacao> specs)
            {
                // Devido à violação do princípio de Liskov, se passarmos um "AnalistaVelhaco" aqui será lançada uma exception
                // pois ele não cumpre funcionalmente o contrato de desenvolvedor, já que lança exceção quando pedem pra ele desenvolver.

                specs
                    .ToList()
                    .ForEach(spec => programador.Code(spec, new[] { new Coffee() }));
            }
        }

        public interface ISoftwareHouse
        {
            void DesenvolverSistema(IProgramador programador, IEnumerable<Especificacao> specs);
        }

        public class Coffee
        { }

        public interface IRequisito { }

        public class Especificacao
        {
            public Especificacao(string texto)
            {
                Texto = texto;
            }

            public string Texto { get; private set; }
        }
    }
}