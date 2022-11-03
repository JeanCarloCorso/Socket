using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Servidor.LogicaServidor
{
    public class Servidor : BaseNotify
    {
        private IPAddress ip;
        private int porta;
        private Socket socketServidor = null;

        public Servidor()
        {
            mensagem = String.Empty;
            string logErro = string.Empty;
            this.porta = 1197;

            string strHostName = string.Empty;
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            ip = ipEntry.AddressList[6];

            mensagem += "Servidor: " + ipEntry.HostName + Environment.NewLine;
            mensagem += "IP: " + ip.ToString() + Environment.NewLine;
            
            if (InicializaServidor())
            {
                Ouvir();
            }
            
        }

        public string mensagem { get; set; }

        public bool InicializaServidor()
        {
            try
            {
                IPEndPoint ipServidor = new IPEndPoint(ip, porta);
                this.socketServidor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                this.socketServidor.Bind(ipServidor);
            }
            catch (Exception ex)
            {
                mensagem += "Erro ao iniciar o servidor!" + Environment.NewLine + ex.Message;
                return false;
            }
            return true;
        }

        private bool Ouvir()
        {
            try
            {
                socketServidor.Listen(100);
                Socket clienteSock = socketServidor.Accept();
                clienteSock.ReceiveBufferSize = 16384;

                byte[] dadosCliente = new byte[1024 * 50000];

                int tamanhoBytesRecebidos = clienteSock.Receive(dadosCliente, dadosCliente.Length, 0);
                int tamanhoMsg = BitConverter.ToInt32(dadosCliente, 0);
                mensagem += Encoding.UTF8.GetString(dadosCliente, 4, tamanhoMsg);
            }
            catch (Exception ex)
            {
                mensagem += "Erro no processamento da mensagem!" + Environment.NewLine + ex.Message;
                return false;
            }
            return false;
        }
    }
}
