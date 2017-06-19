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
    public class LiskovViolationMadeRight
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

        public class AnalistaAntigoProgramadorDelphi : IAnalistaSistemas
        {
            public string Code(Especificacao spec, IEnumerable<Coffee> xicarasCafe)
            {
                return "bla bla bla Unit, bla bla bla arquivo *.pas, bla bla bla COM PLUS...";
            }

            public Especificacao AnalisarRequisitos(IEnumerable<IRequisito> requisitos)
            {
                return new Especificacao("Adoro fazer especificacao e tbm sei programar, sou o pacote completo ;)");
            }
        }

        public class EmpresaFicticia : ISoftwareHouse
        {
            public void DesenvolverSistema(IProgramador programador, IEnumerable<Especificacao> specs)
            {
                // Nesse caso como o "AnalistaAntigoProgramadorDelphi" também sabe cumprir a função de desenvolvedor como era esperado,
                // podemos passar a spec e uma xícara de café para ele que ele dá conta de realizar o trabalho, novamente, como era esperado.

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