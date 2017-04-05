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
    public partial class TelaSaque : Form
    {
        public List<Conta> Contas { get; set; }
        public TelaOperacoes tela;
        public TelaSaque()
        {
            Contas = new List<Conta>();
            InitializeComponent();
        }
        public TelaSaque(TelaOperacoes te)
        {
            Contas = new List<Conta>();
            tela = te;
            InitializeComponent();
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            Contas.Clear();
            var stringConexao = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BD;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
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
        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            linha = dataGridView1.SelectedRows[0].DataBoundItem as Conta;
            textBox1.Text = linha.Agencia;
            textBox2.Text = linha.Numero;
            textBox6.Text = linha.Saldo.ToString();
        }

        private void TelaSaque_Load(object sender, EventArgs e)
        {
            textBox3.Focus();
            textBox3.Select();
            textBox4.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text) && !String.IsNullOrEmpty(textBox2.Text) && !String.IsNullOrEmpty(textBox4.Text) && !String.IsNullOrEmpty(textBox5.Text) && !String.IsNullOrEmpty(textBox6.Text))
            {
                var stringConexao = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=bd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                SqlConnection conexao = new SqlConnection(stringConexao);

                conexao.Open();

                decimal vsaque;
                decimal saldod;
                saldod = linha.Saldo;
                vsaque = Convert.ToDecimal(textBox5.Text);

                decimal valida;
                if (linha.Tipo.Equals("P"))
                {
                    if (vsaque <= 1000.00m)
                    {
                        valida = saldod - (vsaque + 0.10m);
                        if (valida >= 0)
                        {

                            string re = valida.ToString(); ;
                            re = re.Replace(",", ".");
                            

                            SqlCommand comando = new SqlCommand("UPDATE Contas SET Saldo = '" + re + "' WHERE Id = '" + linha.IdConta + "';", conexao);
                            comando.ExecuteNonQuery();
                            string ddd = String.Format("{0:C}", vsaque);
                            MessageBox.Show("Saque de " + ddd + " Efetuado Com Sucesso");

                            conexao.Close();


                            this.Close();
                            tela.carrega();
                        }
                        else
                        {
                            MessageBox.Show("Saldo Insuficiente para esta Operação");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Valor Máximo para Saque para Este tipo de Conta é de R$ 1000,00");
                    }

                }


                if (linha.Tipo.Equals("C"))
                {
                    valida = saldod - (vsaque + 0.20m);
                    if (valida >= 0)
                    {

                        string re = valida.ToString(); ;
                        re = re.Replace(",", ".");
                        MessageBox.Show(re);

                        SqlCommand comando = new SqlCommand("UPDATE Contas SET Saldo = '" + re + "' WHERE Id = '" + linha.IdConta + "';", conexao);
                        comando.ExecuteNonQuery();
                        string ddd = String.Format("{0:C}", vsaque);
                        MessageBox.Show("Saque de " + ddd + " Efetuado Com Sucesso");

                        conexao.Close();


                        this.Close();
                        tela.carrega();
                    }
                    else
                    {
                        MessageBox.Show("Saldo Insuficiente para esta Operação");
                    }
                }

            }
            else
            {
                MessageBox.Show("Dados Incompletos");
            }
        }
            
        }
    }

