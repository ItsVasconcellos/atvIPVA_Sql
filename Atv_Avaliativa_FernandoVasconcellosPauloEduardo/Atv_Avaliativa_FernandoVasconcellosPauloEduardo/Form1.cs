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
            if (textBox3.Text != "")
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
            textBox5.Text = "";
            maskedTextBox2.Text = "";
            maskedTextBox3.Text = "";
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
                maskedTextBox2.Text = aux.ToString();
                maskedTextBox3.Text = "";
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
                maskedTextBox3.Text = (valorImposto / 3).ToString();
                maskedTextBox3.Text = String.Format("{0:N2}", double.Parse(maskedTextBox3.Text));
                maskedTextBox2.Text = "";
                formaPag = 2;
            }
            else
            {
                MessageBox.Show("Calcule primeiro o valor de imposto!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && textBox3.Text != "")
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
                        MySqlCommand insert = new MySqlCommand("insert into Veiculo(placaVeiculo,valor_veiculo,valor_imposto,forma_pagto) values ('" + textBox2.Text + "','" + valorVeiculo + "','" + valorImposto + "','" + formaPag + "')", con);
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
                    int teste = ((int)result["qntd_parcela_paga"]);
                    if (teste == 0)
                    {
                        textBox9.Text = "Sua dívida já está quitada!";
                    }
                    else
                    {
                        textBox9.Text = result["qntd_parcela_paga"].ToString();
                    }
                }
                if(textBox9.Text == "")
                {
                    try
                    {
                        result.Close();
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
            if(textBox9.Text != "" && textBox8.Text != "")
            {
                try
                {
                    con.Open();
                    MySqlCommand consulta = new MySqlCommand("Select placaVeiculo from Veiculo where placaVeiculo=" + textBox8.Text, con);
                    MySqlDataReader checagem = consulta.ExecuteReader();
                    if (checagem.Read())
                    {
                        int Parcelas = int.Parse(textBox9.Text);
                        if (Parcelas > 0)
                        {
                            try
                            {
                                Parcelas--;
                                MySqlCommand alter = new MySqlCommand("Update Pagamento set qntd_parcela_paga=" + Parcelas + " where placa=" + textBox8.Text, con);
                                alter.ExecuteNonQuery();
                                if (Parcelas == 0)
                                {
                                    textBox9.Text = "Dívida quitada";
                                }
                                else
                                {
                                    textBox9.Text = Parcelas.ToString();
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                        }
                        else
                        {
                            MessageBox.Show("Dívida já quitada");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Seu veículo ainda não foi registrado!");
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
            else
            {
                MessageBox.Show("Consulte a quantidade de parcelas a pagar primeiro!");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                MySqlCommand search = new MySqlCommand("Select forma_pagto from Veiculo where placaVeiculo=" + textBox10.Text, con);
                MySqlDataReader formaPagto = search.ExecuteReader();
                if(formaPagto.Read())
                {
                    int formaPagamento = (int)formaPagto["forma_pagto"];
                    textBox11.Text = formaPagto["forma_pagto"].ToString();
                    formaPagto.Close();
                    MySqlCommand parcelasPagar = new MySqlCommand("Select qntd_parcela_paga from Pagamento where placa=" + textBox10.Text, con);
                    MySqlDataReader totalParcelas = parcelasPagar.ExecuteReader();
                    if (totalParcelas.Read())
                    {
                        int parc = (int)totalParcelas["qntd_parcela_paga"];
                        totalParcelas.Close();
                        MySqlCommand valor = new MySqlCommand("Select valor_imposto from Veiculo where placaVeiculo=" + textBox10.Text, con);
                        MySqlDataReader valorV = valor.ExecuteReader();
                        if (valorV.Read())
                        {
                            float valorVeiculo = (float)valorV["valor_imposto"];
                            if (formaPagamento == 1)
                            {
                                if (parc == 0)
                                    textBox12.Text = (valorVeiculo-10/100*valorVeiculo).ToString();
                                if (parc == 1)
                                    textBox12.Text = "0";
                            }
                            if (formaPagamento == 2)
                            {
                                if (parc == 0)
                                    textBox12.Text = valorVeiculo.ToString();
                                if (parc == 1)
                                    textBox12.Text = (2*valorVeiculo/3).ToString();
                                if (parc == 2)
                                    textBox12.Text = (valorVeiculo/3).ToString();
                                if (parc == 3)
                                    textBox12.Text = "0";
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            if (formaPagamento == 1)
                            {
                                MySqlCommand addingPag = new MySqlCommand("Insert into Pagamento(placa,qntd_parcela_paga) values ('" + textBox10.Text + "','" + 1 + "')", con);
                                addingPag.ExecuteNonQuery();
                                textBox9.Text = formaPagamento.ToString();
                            }
                            if (formaPagamento == 2)
                            {
                                MySqlCommand addingPag2 = new MySqlCommand("Insert into Pagamento(placa,qntd_parcela_paga) values ('" + textBox10.Text + "','" + 3 + "')", con);
                                addingPag2.ExecuteNonQuery();
                                textBox9.Text = 3.ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Veículo não registrado!");
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
    }
}
