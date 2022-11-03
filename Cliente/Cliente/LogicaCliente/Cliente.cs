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

                socketCliente.Connect(ipCliente);
                socketCliente.Send(PreparaMsg(msg));
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
            return Encoding.ASCII.GetBytes(msg);
        }
    }
}
