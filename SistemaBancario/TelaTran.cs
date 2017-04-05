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
    public partial class TelaTran : Form
    {
        public List<Conta> Contas { get; set; } // cria a lista de contas do cliente a ser debitado
        public List<Conta> Contas2 { get; set; }// cria a lista de contas do cliente a ser creditado

        public TelaOperacoes tela; // variavel que vai receber a tela principal de operações

        public TelaTran()
        {
            InitializeComponent();
        }

        public TelaTran(TelaOperacoes te) // Construtor de Tela Trans que recebem a tela principal de operações
        {
            Contas = new List<Conta>(); // instancia a lista de contas do cliente a ser debitado
            Contas2 = new List<Conta>();// instancia a lista de contas do cliente a ser creditado
            tela = te;           //variavel tela recebe a tela principal de operações
            InitializeComponent();
        }

        private void TelaTran_Load(object sender, EventArgs e)
        {
            textBox3.Focus(); // coloca o foco na primeira textbox da tela
            textBox3.Select();// coloca o foco na primeira textbox da tela
            textBox4.Enabled = false; // desabilita a textbox para que não haja alteração
        }

        private void textBox3_Leave(object sender, EventArgs e) //quando sai do textbox da id do cliente
        {
            dataGridView1.DataSource = null; //esvazia-se o datagrid
            dataGridView1.Rows.Clear();//esvazia-se o datagrid
            Contas.Clear();//esvazia-se a lista de contas do cliente a ser debitado
            var stringConexao = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=bd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection conexao = new SqlConnection(stringConexao);

            conexao.Open();

            SqlCommand comando = new SqlCommand("SELECT CONTAS.ID, CONTAS.TIPO, CONTAS.AGENCIA, CONTAS.CONTA, CONTAS.SALDO, CLIENTES.NOME FROM CONTAS JOIN CLIENTES ON CONTAS.IdCliente = CLIENTES.IdCliente WHERE CONTAS.IdCliente = '" + textBox3.Text.ToString() + "' ", conexao);
            SqlDataReader reader = comando.ExecuteReader();
            while (reader.Read())
            {
                Conta conta = new Conta();
                conta.IdConta = Convert.ToInt32(reader["Id"]);

                conta.Tipo = reader["Tipo"].ToString();
                conta.Agencia = reader["Agencia"].ToString();
                conta.Numero = reader["Conta"].ToString();
                conta.Saldo = Convert.ToDecimal(reader["Saldo"]);
                textBox4.Text = reader["Nome"].ToString();


                Contas.Add(conta);
            }

            dataGridView1.DataSource = Contas;
            dataGridView1.Columns["IdConta"].Visible = false;
            dataGridView1.Columns["IdCliente"].Visible = false;

            textBox1.Clear();
            textBox2.Clear();
            textBox6.Clear();

            conexao.Close();
        }

        Conta linha = new Conta();
        Conta linha2 = new Conta();

        private void textBox10_Leave(object sender, EventArgs e)
        {
            dataGridView2.DataSource = null;
            dataGridView2.Rows.Clear();
            Contas2.Clear();
            var stringConexao = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=bd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection conexao = new SqlConnection(stringConexao);

            conexao.Open();

            SqlCommand comando = new SqlCommand("SELECT CONTAS.ID, CONTAS.TIPO, CONTAS.AGENCIA, CONTAS.CONTA, CONTAS.SALDO, CLIENTES.NOME FROM CONTAS JOIN CLIENTES ON CONTAS.IdCliente = CLIENTES.IdCliente WHERE CONTAS.IdCliente = '" + textBox10.Text.ToString() + "' ", conexao);
            SqlDataReader reader = comando.ExecuteReader();
            while (reader.Read())
            {
                Conta conta = new Conta();
                conta.IdConta = Convert.ToInt32(reader["Id"]);

                conta.Tipo = reader["Tipo"].ToString();
                conta.Agencia = reader["Agencia"].ToString();
                conta.Numero = reader["Conta"].ToString();
                conta.Saldo = Convert.ToDecimal(reader["Saldo"]);
                textBox9.Text = reader["Nome"].ToString();


                Contas2.Add(conta);
            }

            dataGridView2.DataSource = Contas2;
            dataGridView2.Columns["IdConta"].Visible = false;
            dataGridView2.Columns["IdCliente"].Visible = false;
            dataGridView2.Columns["Saldo"].Visible = false;
            textBox12.Clear();
            textBox11.Clear();
            

            conexao.Close();
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            linha = dataGridView1.SelectedRows[0].DataBoundItem as Conta;
            textBox1.Text = linha.Agencia;
            textBox2.Text = linha.Numero;
            textBox6.Text = linha.Saldo.ToString();
        }

        private void dataGridView2_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            linha2 = dataGridView2.SelectedRows[0].DataBoundItem as Conta;
            textBox12.Text = linha2.Agencia;
            textBox11.Text = linha2.Numero;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text) && !String.IsNullOrEmpty(textBox2.Text) && !String.IsNullOrEmpty(textBox4.Text) && !String.IsNullOrEmpty(textBox5.Text) && !String.IsNullOrEmpty(textBox6.Text) && !String.IsNullOrEmpty(textBox9.Text) && !String.IsNullOrEmpty(textBox11.Text) && !String.IsNullOrEmpty(textBox12.Text))
            {
                var stringConexao = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=bd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                SqlConnection conexao = new SqlConnection(stringConexao);

                conexao.Open();

                decimal vsaque;
                decimal saldod;
                decimal saldo2;
                saldod = linha.Saldo;
                saldo2 = linha2.Saldo;
                vsaque = Convert.ToDecimal(textBox5.Text);
                decimal valida;
                valida = saldod - vsaque;
                if (valida >= 0)
                {
                    decimal result2;
                    result2 = saldo2 + vsaque;

                    string re = result2.ToString(); ;
                    re = re.Replace(",", ".");

                    SqlCommand comando = new SqlCommand("UPDATE Contas SET Saldo = '" + re + "' WHERE Id = '" + linha2.IdConta + "';", conexao);
                    comando.ExecuteNonQuery();

                    string re2 = valida.ToString();
                    re2 = re2.Replace(",", ".");
                    SqlCommand comando2 = new SqlCommand("UPDATE Contas SET Saldo = '" + re2 + "' WHERE Id = '" + linha.IdConta + "';", conexao);
                    comando2.ExecuteNonQuery();
                    conexao.Close();


                    this.Close();
                    tela.carrega();

                }
                else
                {
                    MessageBox.Show("Saldo Insuficiente para Transferência");
                }
            }
            else
            {
                MessageBox.Show("Dados Incompletos");
            }
        }
    }
}
