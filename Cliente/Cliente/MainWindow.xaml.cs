using Cliente.LogicaCliente;
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
        private MainWindowsVM vm;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            bool status = true;
            string erro = string.Empty;
            int numeroPorta;
            try
            {
                numeroPorta = int.Parse(porta.Text);
            }
            catch 
            {
                MessageBox.Show("A porta deve ser um numero!");
                porta.Text = string.Empty;
                return;
            }

            status = vm.EnviarMsg(msg.Text, ip.Text, numeroPorta, out erro);
            
            if (status)
            {
                msg.Text = string.Empty;
            }
            else
            {
                MessageBox.Show("Houve um erro durante o envio!" + Environment.NewLine + erro);
            }
            
        }

        private void Entrar_Click(object sender, RoutedEventArgs e)
        {
            vm = new MainWindowsVM(usuario.Text);
            DataContext = vm;
            vm.telaUsuario = "Collapsed";
            vm.Notifica("telaUsuario");
        }
    }
}
