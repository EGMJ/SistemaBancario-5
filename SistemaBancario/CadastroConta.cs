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
    public partial class CadastroConta : Form
    {
        

        public int contador;
        

        public CadastroConta()
        {
            
            InitializeComponent();
        }

        private void CadastroConta_Load(object sender, EventArgs e)
        {
            
            textBox4.Enabled = false; contador = 0;
            var stringConexao = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=bd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection conexao = new SqlConnection(stringConexao);

            conexao.Open();

            SqlCommand comando = new SqlCommand("SELECT * FROM CONTAS", conexao);
            comando.ExecuteNonQuery();
            contador = (Int32)comando.ExecuteScalar();
            //MessageBox.Show("Test"+count);
            conexao.Close();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox4.Text)&& !String.IsNullOrEmpty(textBox1.Text)&& !String.IsNullOrEmpty(textBox2.Text)&&(radioButton1.Checked==true||radioButton2.Checked==true) )
            {
                var stringConexao = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=bd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                SqlConnection conexao = new SqlConnection(stringConexao);

                conexao.Open();

                string tipo;
                if (radioButton1.Checked == true)
                {
                    tipo = "C";
                }
                else
                {
                    tipo = "P";
                }
                int aumenta;
                aumenta = contador + 1;

                SqlCommand comando = new SqlCommand("INSERT INTO CONTAS (Agencia, Tipo, Conta, Saldo, IdCliente) VALUES ('" + textBox1.Text + "', '" + tipo + "','" + textBox2.Text + "', '0', '" + textBox3.Text + "' );", conexao);

                try
                {
                    comando.ExecuteNonQuery();
                    this.Close();
                    MessageBox.Show("Cliente Cadastrado com sucesso");
                    Conta conta = new Conta();
                    conta.IdCliente = Convert.ToInt32(textBox3.Text);
                    conta.Numero = textBox2.Text;
                    conta.Agencia = textBox1.Text;
                    conta.Saldo = 0;
                    conta.Tipo = tipo;
                    

                }
                catch (SqlException ee)
                {
                    if (ee.Number == 2627) { MessageBox.Show("Dados Duplicados, tente novamente"); }
                    else
                    {


                        MessageBox.Show("Erro na Conexao");
                        
                    }

                }
                conexao.Close();
            }
            else
            {
                MessageBox.Show("Dados Incompletos");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CadastroCliente telaCliente = new CadastroCliente();
            telaCliente.ShowDialog();
            Cliente rec = telaCliente.envia_Cliente() as Cliente;
            textBox3.Text = rec.IdCliente.ToString();
            textBox4.Text = rec.Nome;
            
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            var stringConexao = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=bd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection conexao = new SqlConnection(stringConexao);

            conexao.Open();

            SqlCommand comando = new SqlCommand("SELECT NOME FROM CLIENTES WHERE IdCliente = '" + textBox3.Text.ToString()+"' ", conexao);
            SqlDataReader leitor = comando.ExecuteReader();
            while (leitor.Read())
            {
                textBox4.Text = leitor["Nome"].ToString();
            }


            textBox4.Enabled = false;
            conexao.Close();


        }
    }
}
