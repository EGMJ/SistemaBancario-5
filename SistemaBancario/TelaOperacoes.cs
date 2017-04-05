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
    public partial class TelaOperacoes : Form 
    {
        public List<Conta> Contas { get; set; }

        public TelaOperacoes()
        {
            Contas = new List<Conta>();
            InitializeComponent();
        }

        private void TelaOperacoes_Load(object sender, EventArgs e)
        {
            carrega();
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            TelaSaque sa = new TelaSaque(this);
            sa.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TelaDepo de = new TelaDepo(this);
            de.ShowDialog();
            
            
        }

        public void carrega()
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.Rows.Clear();
            this.Contas.Clear();
            var stringConexao = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=bd;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection conexao = new SqlConnection(stringConexao);

            conexao.Open();

            SqlCommand comando = new SqlCommand("Select * from Contas", conexao);
            SqlDataReader reader = comando.ExecuteReader();
            while (reader.Read())
            {
                Conta conta = new Conta();
                conta.IdConta = Convert.ToInt32(reader["Id"]);
                conta.Numero = reader["Conta"].ToString();
                conta.Agencia = reader["Agencia"].ToString();
                conta.Saldo = Convert.ToDecimal(reader["Saldo"]);
                conta.Tipo = reader["Tipo"].ToString();
                conta.IdCliente = Convert.ToInt32(reader["IdCliente"]);
                Contas.Add(conta);
            }

            dataGridView1.DataSource = Contas;
            dataGridView1.ReadOnly = true;
            conexao.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            TelaSaldo sa = new TelaSaldo();
            sa.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TelaTran tt = new TelaTran(this);
            tt.ShowDialog();
        }
    }
}
