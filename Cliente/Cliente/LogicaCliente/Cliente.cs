using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.LogicaCliente
{
    public class Cliente
    {
        public static bool EnviarMsg(string msg, string ip, int porta, out string erro)
        {
            erro = string.Empty;
            try
            {
                IPEndPoint ipCliente = new IPEndPoint(IPAddress.Parse(ip), porta);
                Socket socketCliente = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

                byte[] mensagem = PreparaMsg(msg);

                socketCliente.Connect(ipCliente);
                socketCliente.Send(mensagem, 0, mensagem.Length, 0);
                socketCliente.Close();

            }catch(Exception ex)
            {
                erro = ex.Message;
                return false;
            }
            return true;
        }

        private static byte[] PreparaMsg(string msg)
        {
            byte[] mensagem = Encoding.UTF8.GetBytes(msg);
            byte[] dadosEnvio = new byte[4 + mensagem.Length];
            byte[] tamanhoMensagem = BitConverter.GetBytes(mensagem.Length);
            tamanhoMensagem.CopyTo(dadosEnvio, 0);
            mensagem.CopyTo(dadosEnvio, 4);

            return dadosEnvio;
        }
    }
}
