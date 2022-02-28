using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using parserDecimal.Parser;
using System.IO;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace the_Golden_Section_Search_Method
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string func = ""; //функция
     

        private void Form1_Load(object sender, EventArgs e)
        {
           MessageBox.Show("Рекомендуется сначало проверить \r\n" +
                    "интервал [a] и [b] на УНИМОДАЛЬНОСТЬ.  \r\n" +
                    "Чтобы найти правильное решение \r\n" +
                    "\r\n" +
                    "Вы сможете получить более подробную информацию \r\n" +
                    "нажав на кнопку ПОМОЩЬ", 
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Computer computer = new Computer();
            Stopwatch swatch = new Stopwatch();

            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            label12.Text = "F(x*) - F(x* - tol)";
            label13.Text = "F(x*) - F(x* + tol)";

            textBox1.ReadOnly = false;
            textBox2.ReadOnly = false;

            decimal x1 = 0, x2, a, b, tol, YF1, YF2, F1, F2, r,FF;
            int max, k = 0;
            try
            {
                a = decimal.Parse(textBox1.Text);
                b = decimal.Parse(textBox2.Text);
                tol = Convert.ToDecimal(Convert.ToDouble(textBox4.Text));
                max = int.Parse(textBox5.Text);
            }
            catch
            {
                MessageBox.Show("Проверьте вводные данные.",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                
                func = comboBox1.Text;
                func = func.ToLower();
                a = decimal.Parse(textBox1.Text);
                b = decimal.Parse(textBox2.Text);
                FF = computer.Compute(func, x1);

            }
            catch (Exception error)
            {


                string s = "Ссылка на объект не указывает на экземпляр объекта";
                if (s != error.Message)
                {
                    MessageBox.Show("При написании функции допущена ошибка!",
                 "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
           
            if (max <= 0)
            {
                MessageBox.Show("Количество итераций должно быть не меньше 1",
                 "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (tol <= 0)
            {
                MessageBox.Show("Погрешность должно быть больше нуля",
                    "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {

            }
            try
            {        
                func = comboBox1.Text;
                func = func.ToLower();
                string str1 = "ln", str2 = "log";
                a = decimal.Parse(textBox1.Text);
                b = decimal.Parse(textBox2.Text);
                if ((func.Contains(str1) || func.Contains(str2)) && (a <= 0 || b <= 0))
                {
                    MessageBox.Show("Для данной функции (содержащий ln или log)\r\n" +
                           "значение отрезков a или b должны быть больше 0 \r\n",
                "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
              //  FF = computer.Compute(func, x);

            }
            catch (Exception error)
            {
            

                string s = "Ссылка на объект не указывает на экземпляр объекта";
                if (s != error.Message)
                {
                    MessageBox.Show("При написании функции допущена ошибка!",
                 "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

           
            swatch.Start(); //начало подсчета времени
            if (radioButton1.Checked) //Переключатель на минимум (Выбран по умолчанию)
            {



                r = Convert.ToDecimal((Math.Sqrt(5) - 1) / 2);
                x1 = a + (1 - r) * (b - a);
                YF1 = aziretParser.ParserDecimal.Compute(func, x1);
                x2 = a + r * (b - a);
                YF2 = aziretParser.ParserDecimal.Compute(func, x2);

                do
                {
                    k = k + 1;
                    progressBar1.Visible = true;
                    progressBar1.Maximum = k + 1;
                    progressBar1.Value = k;
                    if (YF1 >= YF2)
                    {
                        a = x1;
                        x1 = x2;
                        YF1 = YF2;
                        x2 = a + r * (b - a);
                        YF2 = aziretParser.ParserDecimal.Compute(func, x2);
                    }
                    else
                    {
                        b = x2;
                        x2 = x1;
                        YF2 = YF1;
                        x1 = a + (1 - r) * (b - a);
                        YF1 = aziretParser.ParserDecimal.Compute(func, x1);
                    }

                }
                while (Math.Abs(b - a) > tol);

                swatch.Stop();
                progressBar1.Visible = false;
                progressBar1.Value = 0;
                textBox10.Text = (swatch.Elapsed).ToString();
                textBox6.Text = x1.ToString();
                textBox7.Text = YF1.ToString();
                textBox8.Text = Math.Abs(b - a).ToString("0E0");
                textBox9.Text = k.ToString();

                F1 = aziretParser.ParserDecimal.Compute(func, x1 - tol);
                textBox12.Text = F1.ToString();
                //MessageBox.Show("x1 - tol = " + F1.ToString("0E0"));
                F2 = aziretParser.ParserDecimal.Compute(func, x1 + tol);
                textBox13.Text = F2.ToString();

                decimal a1, b1, c, a2, b2;
                a1 = decimal.Parse(textBox7.Text);
                b1 = decimal.Parse(textBox12.Text);
                c = decimal.Parse(textBox13.Text);

                a2 = a1 - b1;
                label12.Text = "F(x*) - F(x* - tol) = " + a2.ToString();
                b2 = a1 - c;
                label13.Text = "F(x*) - F(x* + tol) = " + b2.ToString();

                if (YF1 <= F1 && YF1 <= F2)
                {
                    textBox11.BackColor = textBox11.BackColor;
                    textBox11.ForeColor = Color.Green;
                    textBox11.Text = "\r\n" +
                        "\r\n" +
                        "The result x* is the minimizer of this function because \r\n" +
                        "F(x*) <= F(x*–Tolerance) \r\n" +
                        "And \r\n" +
                        "F(x*) <= F(x*+Tolerance) \r\n";
                   
                }

                else if (YF1 <= F1 && YF1 >= F2)
                {
                    textBox11.BackColor = textBox11.BackColor;
                    textBox11.ForeColor = Color.DarkRed;
                    textBox11.Text = "\r\n" +
                        "\r\n" +
                        "The result x* is not the minimizer of this function because \r\n" +
                        "F(x*) <= F(x*–Tolerance) \r\n" +
                        "And \r\n" +
                        "F(x*) >= F(x*+Tolerance) \r\n";
                    return;
                }

                else if (YF1 >= F1 && YF1 <= F2)
                {
                    textBox11.BackColor = textBox11.BackColor;
                    textBox11.ForeColor = Color.DarkRed;
                    textBox11.Text = "\r\n" +
                        "\r\n" +
                        "The result x* is not the minimizer of this function because \r\n" +
                        "F(x*) >= F(x*–Tolerance) \r\n" +
                        "And \r\n" +
                        "F(x*) <= F(x*+Tolerance) \r\n";
                    return;
                }
            }
            else if (radioButton2.Checked) // Переключатель на максимум
            {
                swatch.Start(); //начало подсчета времени

                r = Convert.ToDecimal((Math.Sqrt(5) - 1) / 2);
                x1 = a + (1 - r) * (b - a);
                YF1 = aziretParser.ParserDecimal.Compute(func, x1);
                x2 = a + r * (b - a);
                YF2 = aziretParser.ParserDecimal.Compute(func, x2);

                do
                {
                    k = k + 1;
                    progressBar1.Visible = true;
                    progressBar1.Maximum = k + 1;
                    progressBar1.Value = k;
                    if (YF1 <= YF2)
                    {
                        a = x1;
                        x1 = x2;
                        YF1 = YF2;
                        x2 = a + r * (b - a);
                        YF2 = aziretParser.ParserDecimal.Compute(func, x2);
                    }
                    else
                    {
                        b = x2;
                        x2 = x1;
                        YF2 = YF1;
                        x1 = a + (1 - r) * (b - a);
                        YF1 = aziretParser.ParserDecimal.Compute(func, x1);
                    }
                }
                while (Math.Abs(b - a) > tol);

                swatch.Stop();
                progressBar1.Visible = false;
                progressBar1.Value = 0;
                textBox10.Text = (swatch.Elapsed).ToString();
                textBox6.Text = x1.ToString();
                textBox7.Text = YF1.ToString();
                textBox8.Text = Math.Abs(b - a).ToString("0E0");
                textBox9.Text = k.ToString();

                F1 = aziretParser.ParserDecimal.Compute(func, x1 - tol);
                textBox12.Text = F1.ToString();
                F2 = aziretParser.ParserDecimal.Compute(func, x1 + tol);
                textBox13.Text = F2.ToString();

                decimal a1, b1, c, a2, b2;
                a1 = decimal.Parse(textBox7.Text);
                b1 = decimal.Parse(textBox12.Text);
                c = decimal.Parse(textBox13.Text);

                a2 = a1 - b1;
                label12.Text = "F(x*) - F(x* - tol) = " + a2.ToString();
                b2 = a1 - c;
                label13.Text = "F(x*) - F(x* + tol) = " + b2.ToString();

                if (YF1 >= F1 && YF1 >= F2)
                {
                    textBox11.BackColor = textBox11.BackColor;
                    textBox11.ForeColor = Color.Green;
                    textBox11.Text = "\r\n" +
                        "\r\n" +
                       "The result x* is the maximizer of this function because \r\n" +
                       "F(x*) >= F(x*–Tolerance) \r\n" +
                       "And \r\n" +
                       "F(x*) >= F(x*+Tolerance) \r\n";
                    return;
                }
                else if (YF1 <= F1 && YF1 <= F2)
                {
                    textBox11.BackColor = textBox11.BackColor;
                    textBox11.ForeColor = Color.Green;
                    textBox11.Text = "\r\n" +
                        "\r\n" +
                       "The result x* is the maximizer of this function because \r\n" +
                       "F(x*) <= F(x*–Tolerance) \r\n" +
                       "And \r\n" +
                       "F(x*) <= F(x*+Tolerance) \r\n";
                    return;
                }
               else if (YF1 <= F1 && YF1 >= F2)
                {
                    textBox11.BackColor = textBox11.BackColor;
                    textBox11.ForeColor = Color.DarkRed;
                    textBox11.Text = "\r\n" +
                      "\r\n" +
                  "The result x* is not the maximizer of this function because \r\n" +
                  "F(x*) <= F(x*–Tolerance) \r\n" +
                  "And \r\n" +
                  "F(x*) >= F(x*+Tolerance) \r\n";
                }
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            label12.Text = "F(x*) - F(x* - tol)";
            label13.Text = "F(x*) - F(x* + tol)";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string mySheet = Path.Combine(System.Windows.Forms.Application.StartupPath, "Grafic.xlsx");

            Excel.Application ExcelApp = new Excel.Application();
            Workbook wb = ExcelApp.Workbooks.Open(mySheet);
            Worksheet ws = (Worksheet)wb.ActiveSheet;

            ExcelApp.Visible = true;

            func = comboBox1.Text;
            ws.Cells[2, 2] = func;
            func = func.Replace("exp", "!");
            func = func.Replace("x", "D4");
            func = "=" + func.Replace("!", "exp");
            ws.Cells[4, 9] = textBox1.Text;
            ws.Cells[4, 10] = textBox2.Text;
            ws.Range["E4", "E10003"].Value = func;
        }


        private void button5_Click(object sender, EventArgs e)
        {
            string mySheet = Path.Combine(System.Windows.Forms.Application.StartupPath, "Grafic.xlsx");
            Excel.Application ExcelApp = new Excel.Application();
            Workbook wb = ExcelApp.Workbooks.Open(mySheet);

            Worksheet sh = (Worksheet)wb.ActiveSheet;

            Excel.Range cell = sh.Cells[4, 9] as Excel.Range;
            string value = cell.Value2.ToString();
            textBox1.Text = value;
            textBox1.ReadOnly = true;

            Excel.Range cell2 = sh.Cells[4, 10] as Excel.Range;
            string value2 = cell2.Value2.ToString();
            textBox2.Text = value2;
            textBox2.ReadOnly = true;

            wb.Close();

            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            label12.Text = "F(x*) - F(x* - tol)";
            label13.Text = "F(x*) - F(x* + tol)";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Help frm2 = new Help(); 
            frm2.Show();  // открываем форму-2
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

    }
}

       


