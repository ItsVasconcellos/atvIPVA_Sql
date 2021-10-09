using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atv_Avaliativa_FernandoVasconcellosPauloEduardo
{
    public partial class Form1 : Form
    {
        MySqlConnection con;
        Double valorImposto;
        Double valorVeiculo;
        double aux;
        int formaPag;

        public Form1()
        {
            InitializeComponent();
            try
            {
                con = new MySqlConnection("server=143.106.241.3;port=3306;User ID=cl200128;database=cl200128;password=cl*10092004; SslMode=none");
            }
            catch
            {
                MessageBox.Show("Falha na Conexão");
            }
            label5.Text = DateTime.Now.ToString("T");
            textBox1.Text = DateTime.Today.ToString("dd/MM/yyy");

            this.WindowState = FormWindowState.Maximized;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label5.Text = DateTime.Now.ToString("T");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != null)
            {
                valorVeiculo = float.Parse(textBox3.Text);
                valorImposto = 0.04 * valorVeiculo;
                textBox5.Text = valorImposto.ToString();
            }
            else
            {
                MessageBox.Show("Insira o valor do carro!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            aux = 0;
            valorImposto = 0;
            valorVeiculo = 0;
            radioButton1.Checked = false;
            radioButton2.Checked = false;

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (valorImposto != 0)
            {
                aux = valorImposto - 0.10 * valorImposto;
                textBox6.Text = aux.ToString();
                formaPag = 1;
            }
            else
            {
                MessageBox.Show("Calcule primeiro o valor de imposto!");
            }
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (valorImposto != 0)
            {
                textBox7.Text = (valorImposto / 3).ToString();
                formaPag = 2;
            }
            else
            {
                MessageBox.Show("Calcule primeiro o valor de imposto!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != null && textBox3 != null)
            {
                if (radioButton1.Checked || radioButton2.Checked)
                {
                    con.Open();
                    if (radioButton1.Checked)
                    {
                        valorImposto = aux;
                    }
                    try
                    {
                        MySqlCommand insert = new MySqlCommand("insert into Veiculo(placaVeiculo,valor_veículo,valor_imposto,forma_pagto) values ('" + textBox2.Text + "','" + valorVeiculo + "','" + valorImposto + "','" + formaPag + "')", con);
                        insert.ExecuteNonQuery();
                        MessageBox.Show("Dados gravados");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Calcule primeiro o valor de imposto e escolha sua forma de pagamento!");
            }
        }
    }
}
