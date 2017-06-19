using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace Solid
{
    /// <summary>
    ///  "A class should have only one reason to change."
    /// </summary>
    public class SingleResponsibilityViolation
    {
        public class Usuario
        {
            public long Id { get; set; }

            public string Nome { get; set; }

            public string Email { get; set; }

            public DateTime DataNascimento { get; set; }

            public string Facebook { get; set; }
        }

        public class ImportacaoUsuarioFacebook // O próprio nome já deixa explícito que é uma importação E que depende do Facebook
        {
            private const string URL = "https://graph.facebook.com/search?q={0}&type=user";

            public void Importar(string email)
            {
                // Se a URL do facebook mudar preciso alterar essa classe...
                var url = string.Format(URL, email);

                // Se o Web Service deixar de ser REST para ser SOAP preciso alterar essa classe...
                var client = new HttpClient();
                var response = client.GetAsync(url).Result;
                var responseContent = response.Content.ReadAsStringAsync().Result;

                // Se o formato retornado pelo Web Service mudar de JSON para XML preciso alterar essa classe...
                var usuario = JsonConvert.DeserializeObject<Usuario>(responseContent);

                // Se mudar a forma de persistência preciso alterar essa classe...
                Save(usuario);
            }

            public void Save(Usuario usuario)
            {
                // Aqui faríamos um roundtrip para um SQL Server para salvar o usuario.
            }
        }
    }
}