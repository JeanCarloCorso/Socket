using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Servidor
{
    public class MainWindowVM : BaseNotify
    {
        public string mensagesTelaServidor { get; set; }
        public string corStatusServidor { get; set; }
        public string corBoaoServidor { get; set; }
        public string textoBotaoServidor { get; set; }

        private bool statusServidor = false;
        private IPHostEntry dadosDeRede;
        private int porta;
        private Socket socketServidor = null;

        public MainWindowVM()
        {
            this.porta = 1197;
            this.dadosDeRede = Dns.GetHostEntry(Dns.GetHostName());
            MudaStatusServidor(false);

        }

        public async Task<bool> SubirServidor()
        {
            if (!statusServidor)
            {
                if (await InicializaServidor())
                {
                    EscreverNaTela("Esperando conexões........");
                    do
                    {
                        await Ouvir();
                    } while (statusServidor);
                }
            }
            else
            {
                Dispose();
            }
            return true;
        }

        private Task<bool> InicializaServidor()
        {
            EscreverNaTela("Iniciando Servidor......");
            try
            {
                IPEndPoint ipServidor = new IPEndPoint(dadosDeRede.AddressList[6], porta);
                this.socketServidor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                this.socketServidor.Bind(ipServidor);
                EscreverNaTela("+------------------------------------------------------+");
                EscreverNaTela("|  Servidor: " + dadosDeRede.HostName);
                EscreverNaTela("|  IP: " + dadosDeRede.AddressList[6].ToString());
                EscreverNaTela("|  Porta: " + porta);
                EscreverNaTela("+------------------------------------------------------+");

                MudaStatusServidor(true);
            }
            catch (Exception ex)
            {
                EscreverNaTela("Erro ao iniciar o servidor!" + Environment.NewLine + ex.Message);
                MudaStatusServidor(false);
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

        private async Task<bool> Ouvir()
        {
            try
            {
                socketServidor.Listen(100);
                Socket clienteSock = null;
                await Task.Run(() => { clienteSock = socketServidor.Accept(); });
                
                clienteSock.ReceiveBufferSize = 16384;

                byte[] dadosCliente = new byte[1024 * 50000];

                int tamanhoBytesRecebidos = clienteSock.Receive(dadosCliente, dadosCliente.Length, 0);
                int tamanhoMsg = BitConverter.ToInt32(dadosCliente, 0);
                EscreverNaTela(Encoding.UTF8.GetString(dadosCliente, 4, tamanhoMsg));
            }
            catch (Exception ex)
            {
                EscreverNaTela("Erro no processamento da mensagem!" + Environment.NewLine + ex.Message);
                MudaStatusServidor(false);
                return await Task.FromResult(false);
            }
            return await Task.FromResult(false);
        }

        private void MudaStatusServidor(bool status)
        {
            statusServidor = status;
            if (!status)
            {
                corStatusServidor = "red";
                corBoaoServidor = "#FF369A14";
                textoBotaoServidor = "LIGAR SERVIDOR";
            }
            else
            {
                corStatusServidor = "#FF369A14";
                corBoaoServidor = "#C4302B";
                textoBotaoServidor = "DESLIGAR SERVIDOR";
            }
            Notifica("corStatusServidor");
            Notifica("corBoaoServidor");
            Notifica("textoBotaoServidor");
        }
        private void EscreverNaTela(string msg)
        {
            mensagesTelaServidor += msg + Environment.NewLine;
            Notifica("mensagesTelaServidor");
        }

        private void Dispose()
        {
            socketServidor.Close();
        }
    }
}
