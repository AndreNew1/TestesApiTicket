using Model;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using TesteApiTicket.Model;
using System.Threading.Tasks;

namespace TesteTicketApi
{
    [TestFixture]
    public class Tests
    {
        private HttpClient Usuarios;
        private string TokenAtendente;
        private string TokenCliente;
        private static readonly string CaminhoErro = $"{AppDomain.CurrentDomain.BaseDirectory}/Excecoes";
        
        [SetUp]
        public void Setup()
        {
           Usuarios = new HttpClient();
           TokenCliente = "342C9C18-3D3A-493E-9687-F47C86DA20F1";
           TokenAtendente = "B5A424D5-BC47-4926-8290-676EDED3FDCB";
        }

        [Test]
        [Parallelizable]
        public async Task TesteRegistro()
        {
            try
            {
                var Resposta = await Usuarios.PostAsync("https://ticketdev4jobs.azurewebsites.net/api/usuarios", new StringContent(JsonConvert.SerializeObject(new Usuario()), Encoding.UTF8, "application/json"));
                string Res = await Resposta.Content.ReadAsStringAsync();
                var Teste = JsonConvert.DeserializeObject<Retorno>(Res);
                Assert.IsFalse(Teste.Status);

                Resposta = await Usuarios.PostAsync("https://ticketdev4jobs.azurewebsites.net/api/usuarios", new StringContent(JsonConvert.SerializeObject(new Usuario {Nome="An",Senha="andre" }), Encoding.UTF8, "application/json"));
                Res = await Resposta.Content.ReadAsStringAsync();
                Teste = JsonConvert.DeserializeObject<Retorno>(Res);
                Assert.IsFalse(Teste.Status);
            }
            catch(Exception e)
            {
                File.AppendAllText(CaminhoErro, e.Message);
            }
        }

        [Test]
        [Parallelizable]
        public async Task TesteLogin()
        {
            try
            {
                var Resposta = await Usuarios.PostAsync("https://ticketdev4jobs.azurewebsites.net/api/usuarios/Autenticar", new StringContent(JsonConvert.SerializeObject(new Usuario { Email = "andre@atendente.com", Senha = "senha1234" }), Encoding.UTF8, "application/json"));
                string Res = await Resposta.Content.ReadAsStringAsync();
                var Teste = JsonConvert.DeserializeObject<Retorno>(Res);
                Assert.IsTrue(Teste.Status);
            }
            catch (Exception e)
            {
                File.AppendAllText(CaminhoErro, e.Message);
            }
        }

        [Test]
        [Parallelizable]
        public async Task TesteCadastroTicket()
        {
            try
            {
                Usuarios.DefaultRequestHeaders.TryAddWithoutValidation("autorToken", TokenCliente);
                var Resposta = await Usuarios.PostAsync("https://ticketdev4jobs.azurewebsites.net/api/Tickets", new StringContent(JsonConvert.SerializeObject(new Ticket()), Encoding.UTF8, "application/json"));
                string Res = await Resposta.Content.ReadAsStringAsync();
                var Teste = JsonConvert.DeserializeObject<Retorno>(Res);
                Assert.IsFalse(Teste.Status);
                
                Resposta = await Usuarios.PostAsync("https://ticketdev4jobs.azurewebsites.net/api/Tickets", new StringContent(JsonConvert.SerializeObject(new Ticket { Titulo = "" , Mensagem = "" }), Encoding.UTF8, "application/json"));
                Res = await Resposta.Content.ReadAsStringAsync();
                Teste = JsonConvert.DeserializeObject<Retorno>(Res);
                Assert.IsFalse(Teste.Status);


                Usuarios.DefaultRequestHeaders.TryAddWithoutValidation("autorToken", TokenAtendente);
                Resposta = await Usuarios.PostAsync("https://ticketdev4jobs.azurewebsites.net/api/Tickets", new StringContent(JsonConvert.SerializeObject(new Usuario { Email = "andre@atendente.com", Senha = "senha1234" }), Encoding.UTF8, "application/json"));
                Res = await Resposta.Content.ReadAsStringAsync();
                Teste = JsonConvert.DeserializeObject<Retorno>(Res);
                Assert.IsFalse(Teste.Status);
            }
            catch(Exception e)
            {
                File.AppendAllText(CaminhoErro, e.Message);
            }
        }


    }
}