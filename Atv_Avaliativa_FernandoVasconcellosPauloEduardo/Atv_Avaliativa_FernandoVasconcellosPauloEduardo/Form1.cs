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
            maskedTextBox1.Text = DateTime.Today.ToString();

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

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {


                con.Open();
                MySqlCommand busca = new MySqlCommand("Select qntd_parcela_paga from Pagamento where placa=" + textBox8.Text, con);
                MySqlDataReader result = busca.ExecuteReader();

                if (result.Read())
                {
                    int teste = ((int)result["qtde_parcela_paga"]);
                    if (teste != 0)
                    {
                        textBox9.Text = "Sua dívida já está quitada!";
                    }
                    textBox9.Text = result["qtde_parcela_paga"].ToString();
                    
                }
                if(textBox9.Text == "")
                {
                    try
                    {
                        MySqlCommand buscaFormapag = new MySqlCommand("Select forma_pagto from Veiculo where placaVeiculo=" + textBox8.Text, con);
                        MySqlDataReader resultForma = buscaFormapag.ExecuteReader();
                        if (resultForma.Read())
                        {
                            
                            int qntdParcelas = 0;
                            qntdParcelas = (int)resultForma["forma_pagto"];
                            resultForma.Close();
                                if (qntdParcelas == 1)
                                {
                                    MySqlCommand addingPag = new MySqlCommand("Insert into Pagamento(placa,qntd_parcela_paga) values ('" + textBox8.Text + "','" + 1 +"')", con);
                                    addingPag.ExecuteNonQuery();
                                    textBox9.Text = qntdParcelas.ToString();
                                }
                                if (qntdParcelas == 2)
                                {
                                    MySqlCommand addingPag2 = new MySqlCommand("Insert into Pagamento(placa,qntd_parcela_paga) values ('" + textBox8.Text + "','" + 3 + "')", con);
                                    result.Close();
                                    addingPag2.ExecuteNonQuery();
                                    textBox9.Text = 3.ToString();
                                }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }

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

        private void button5_Click(object sender, EventArgs e)
        {
            if(textBox9.Text != null && textBox8.Text != null)
            {
                try
                {
                    con.Open();
                    int Parcelas = int.Parse(textBox9.Text);
                    if (Parcelas > 0)
                    {
                        Parcelas--;
                        MySqlCommand alter = new MySqlCommand("Update Pagamento set qntd_parcela_paga='" + Parcelas + "' where placa=" + textBox8.Text, con);
                        if (Parcelas == 0)
                        {
                            textBox9.Text = "Dívida quitada";
                        }
                        else
                        { 
                            textBox9.Text = Parcelas.ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Dívida já quitada");
                    }
                }
                catch 
                {
                    MessageBox.Show("Você ainda não consultou o veículo!")
                }
                finally
                {
                    con.Close();
                }
            }
            else
            {
                MessageBox.Show("Consulte a quantidade de parcelas a pagar primeiro!");
            }
        }
    }
}
