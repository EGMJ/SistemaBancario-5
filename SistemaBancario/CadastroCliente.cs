using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaBancario
{
    public partial class CadastroCliente : Form
    {
        public CadastroCliente()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox2.Text))
            {


                var stringConexao = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=bd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                SqlConnection conexao = new SqlConnection(stringConexao);

                conexao.Open();

                SqlCommand comando = new SqlCommand("INSERT INTO Clientes (Nome) VALUES ('" + textBox2.Text + "' );", conexao);

                try
                {
                    comando.ExecuteNonQuery();
                    this.Close();
                    MessageBox.Show("Cliente Cadastrado com sucesso");


                }
                catch (Exception ee)
                {

                    MessageBox.Show("Erro na Conexao");
                    MessageBox.Show(ee.ToString());

                }
                conexao.Close();
            }
            else
            {
                MessageBox.Show("Dados Incompletos");
            }
        }

        private void CadastroCliente_Load(object sender, EventArgs e)
        {
            var stringConexao = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=bd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection conexao = new SqlConnection(stringConexao);

            conexao.Open();
            SqlCommand comando = new SqlCommand("SELECT COUNT(*) FROM Clientes ;", conexao);
            Int32 cont = (Int32)comando.ExecuteScalar();
            cont += 1;
            textBox1.Text = cont.ToString();
            textBox1.Enabled = false;
            conexao.Close();

        }
        public object envia_Cliente()
        {
            Cliente teste = new Cliente();
            teste.IdCliente = Convert.ToInt32(textBox1.Text);
            teste.Nome = textBox2.Text;
            return teste;
        }

    }
}
