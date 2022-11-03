using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cliente
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            bool status = true;
            string erro = string.Empty;
            try
            {
                int numeroPorta = int.Parse(porta.Text);
            }
            catch 
            {
                MessageBox.Show("A porta deve ser um numero!");
                porta.Text = string.Empty;
                return;
            }


            status = LogicaCliente.Cliente.EnviarMsg(msg.Text, ip.Text, int.Parse(porta.Text), out erro);
            
            if (status)
            {
                MessageBox.Show("Mensagem enviada com sucesso!");
            }
            else
            {
                MessageBox.Show("Houve um erro durante o envio!" + Environment.NewLine + erro);
            }
            
        }
    }
}
