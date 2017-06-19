using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace Solid
{
    /// <summary>
    ///  "A class should have only one reason to change."
    /// </summary>
    public class SingleResponsibilityViolationMadeRight
    {
        public class Usuario
        {
            public long Id { get; set; }

            public string Nome { get; set; }

            public string Email { get; set; }

            public DateTime DataNascimento { get; set; }

            public string Facebook { get; set; }
        }

        public class ImportacaoUsuario
        {
            // Pode ser qualquer provider... facebook, twitter, AD, SAP...
            private readonly IUsuarioProvider _usuarioProvider;

            // Pode ser qualquer repositorio... arquivo, cache em memória, banco relacional, nosql...
            private readonly IRepository _repository;

            public ImportacaoUsuario(IUsuarioProvider usuarioProvider, IRepository repository)
            {
                _usuarioProvider = usuarioProvider;
                _repository = repository;
            }

            public void Importar(string email)
            {
                // Não instancio mais objetos concretos dentro do meu método, aumentando a testabilidade...
                var usuario = _usuarioProvider.Obter(email);
                _repository.Save(usuario);
            }
        }

        public class UsuarioFacebookProvider : IUsuarioProvider
        {
            private const string URL = "https://graph.facebook.com/search?q={0}&type=user";

            private readonly IRestClient _client;
            private readonly IDeserializer _deserializer;

            public UsuarioFacebookProvider(IRestClient client, IDeserializer deserializer)
            {
                _client = client;
                _deserializer = deserializer;
            }

            public Usuario Obter(string email)
            {
                // Se a URL do Facebook mudar tenho que alterar essa classe, o que faz sentido já que é um provedor de usuarios do facebook...
                var url = string.Format(URL, email);

                // Se o formato mudar de Json para XML posso injetar um IDeserializer que trate XML e não preciso mudar o interior dessa classe...
                return _deserializer.Deserializar<Usuario>(_client.Get(url));
            }
        }

        public class JsonDeserializer : IDeserializer
        {
            public T Deserializar<T>(string documento)
            {
                return JsonConvert.DeserializeObject<T>(documento);
            }
        }

        public class XmlDeserializer : IDeserializer
        {
            public T Deserializar<T>(string documento)
            {
                // Aqui implementariamos a deserialização do XML
                throw new NotImplementedException();
            }
        }

        public class RestClient : IRestClient
        {
            public string Get(string url)
            {
                var client = new HttpClient();
                var response = client.GetAsync(url).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public class Repository : IRepository
        {
            public void Save<T>(T obj)
            {
            }
        }

        public interface IRestClient
        {
            string Get(string url);
        }

        public interface IDeserializer
        {
            T Deserializar<T>(string document);
        }

        public interface IUsuarioProvider
        {
            Usuario Obter(string email);
        }

        public interface IRepository
        {
            void Save<T>(T obj);
        }
    }
}