using Model;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using TesteApiTicket.Model;
using System.Threading;
using System.Threading.Tasks;

namespace TesteTicketApi
{
    [TestFixture]
    public class Tests
    {
        private HttpClient Usuarios;
        private static string Caminho = $"{AppDomain.CurrentDomain.BaseDirectory}/Sucesso.json";
        private static string CaminhoErro = $"{AppDomain.CurrentDomain.BaseDirectory}/Erros.json";
        [SetUp]
        public void Setup()
        {
           Usuarios = new HttpClient();
        }

        [Test]
        public async Task Test1Async()
        {
            try
            {
                var Resposta = await Usuarios.PostAsync("https://ticketdev4jobs.azurewebsites.net/api/usuarios", new StringContent(JsonConvert.SerializeObject(new Usuario()), Encoding.UTF8, "application/json")).ConfigureAwait(true);
                string Res = await Resposta.Content.ReadAsStringAsync().ConfigureAwait(true);
                File.AppendAllText(Caminho, Res);
                var Teste = JsonConvert.DeserializeObject<Retorno>(Res);
                Assert.IsFalse(Teste.Status);
            }
            catch(Exception e)
            {
                File.AppendAllText(CaminhoErro, e.Message);
            }
        }
        [Test]
        public async Task TesteLogin()
        {
            try
            {
                var Resposta = await Usuarios.PostAsync("https://ticketdev4jobs.azurewebsites.net/api/usuarios/Autenticar", new StringContent(JsonConvert.SerializeObject(new Usuario { Email = "andre@atendente.com", Senha = "senha1234" }), Encoding.UTF8, "application/json")).ConfigureAwait(true);
                string Res = await Resposta.Content.ReadAsStringAsync().ConfigureAwait(true);
                File.AppendAllText(Caminho, Res);
                var Teste = JsonConvert.DeserializeObject<Retorno>(Res);
                Assert.IsTrue(Teste.Status);
            }
            catch (Exception e)
            {
                File.AppendAllText(CaminhoErro, e.Message);
            }
        }
    }
}