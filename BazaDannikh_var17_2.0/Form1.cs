using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BazaDannikh_var17_2._0
{
    public partial class Form1 : Form
    {
        String path = "base.txt";
        int RowCount = 0;
        //задаем все необходимые переменные
        double sigma_F, omega_FT, K_FV, omega_FV, sigmaF_dopusk;// формульные
        double Y_F, sigma1_FP, K_Fbeta, g0, N_F0, N_FE, F_t; // табличные
        double Y_b = 1, Y_beta = 1;//
        double b_omega, m, v, a_w, u, mf;// вводимые как и табличные, но без таблиц
        double delta_f = 0.011;
        int HB, zv, x, n;

        private void button2_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            textBox1.Text = Convert.ToString(rnd.Next(0, 11));
            if (rnd.Next(0, 2) == 1)
            {
                Нереверсивная.Checked = true;
                radioButton1.Checked = false;
            }
            else {
                Нереверсивная.Checked = false;
                radioButton1.Checked = true;
            }
            textBox2.Text = Convert.ToString(rnd.Next(210, 500));
            textBox3.Text = Convert.ToString(rnd.Next(1,23));
            textBox13.Text = Convert.ToString(rnd.Next(1, 9));
            textBox4.Text = Convert.ToString(rnd.Next(0, 7));
            textBox5.Text = Convert.ToString(rnd.Next(0, 1500) / 100f);
            textBox6.Text = Convert.ToString(rnd.Next(5, 9));
            textBox7.Text = Convert.ToString(rnd.Next(0, 1000) / 100f);
            textBox8.Text = Convert.ToString(rnd.Next(250, 2500) / 100f);
            textBox9.Text = Convert.ToString(rnd.Next(4000, 35000) / 100f);
            textBox10.Text = Convert.ToString(rnd.Next(0, 1000) / 100f);
            textBox11.Text = Convert.ToString(rnd.Next(0, 1000) / 100f);
            textBox12.Text = Convert.ToString(rnd.Next(100000, 10000000));
            int sw = rnd.Next(0, 5);
            switch (sw)
            {
                case 1: radioButton2.Checked = true; break;
                case 2: radioButton3.Checked = true; break;
                case 3: radioButton4.Checked = true; break;
                case 4: radioButton5.Checked = true; break;
                    //default: MessageBox.Show("Random has been broken down :C", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (rnd.Next(0, 2) == 1)
            {
                radioButton6.Checked = true;
                radioButton7.Checked = false;
            }
            else
            {
                radioButton7.Checked = true;
                radioButton6.Checked = false;
            }
        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //формулы
            //sigmaF_dopusk = Convert.ToDouble(textBox1.Text);// принимаем значения
            //HB            = Convert.ToDouble(textBox2.Text);
            //Y_F           = Convert.ToDouble(textBox3.Text);
            //K_Fbeta       = Convert.ToDouble(textBox4.Text);
            //m             = Convert.ToDouble(textBox5.Text);
            //b_omega       = Convert.ToDouble(textBox6.Text);
            //n             = Convert.ToDouble(textBox7.Text);
            //v             = Convert.ToDouble(textBox8.Text);
            //a_w           = Convert.ToDouble(textBox8.Text);
            //u             = Convert.ToDouble(textBox9.Text);
            //N_F0          = Convert.ToDouble(textBox10.Text);
            //N_FE          = Convert.ToDouble(textBox11.Text);
            //F_t
            int for_sigmaF_dopusk = Convert.ToInt32(textBox1.Text);// for search in tabl6.5
            if (revers_or_not == 0)
            {
                sigmaF_dopusk = tab6dot5[for_sigmaF_dopusk, 0];
                N_F0 = tab6dot5[for_sigmaF_dopusk, 2];
            }
            else
            {
                sigmaF_dopusk = tab6dot5[for_sigmaF_dopusk, 1];
                N_F0 = tab6dot5[for_sigmaF_dopusk, 2];
            }


            HB = Convert.ToInt32(textBox2.Text);
            if (HB >= 350)
            { mf = 9; }
            else
            { mf = 6; }


            zv = Convert.ToInt32(textBox3.Text);//  таблица 6.7 Y_F
            x = Convert.ToInt32(textBox13.Text);
            Y_F = tab6dot7[zv, x];

            double z1_v = 1;
            int for_z1_v = 0;
            for_z1_v = Convert.ToInt32(textBox4.Text);// psi(stroka) таблица 6.3 
            if (for_K_Fbeta == 1)
            {
                if (HB >= 350)
                { z1_v = tab6dot3a[for_z1_v, 1]; }
                else
                { z1_v = tab6dot3a[for_z1_v, 2]; }
            }
            else if (for_K_Fbeta == 2)
            {
                if (HB >= 350)
                { z1_v = tab6dot3b[for_z1_v, 1]; }
                else
                { z1_v = tab6dot3b[for_z1_v, 2]; }
            }
            else if (for_K_Fbeta == 3)
            {
                if (HB >= 350)
                { z1_v = tab6dot3c[for_z1_v, 1]; }
                else
                { z1_v = tab6dot3c[for_z1_v, 2]; }
            }
            else if (for_K_Fbeta == 4)
            {
                if (HB >= 350)
                { z1_v = tab6dot3d[for_z1_v, 1]; }
                else
                { z1_v = tab6dot3d[for_z1_v, 2]; }
            }

            K_Fbeta = z1_v;

            int j = 0;// получаем g0 из таблицы 6.13
            n = Convert.ToInt32(textBox6.Text);//4-9
            m = Convert.ToDouble(textBox5.Text);
            if (n == tab6dot13[0, j])
            {
                if (m < 3.5)
                {
                    g0 = tab6dot13[1, j];
                }
                else if (m > 3.5 && m <= 10)
                {
                    g0 = tab6dot13[2, j];
                }
                else if (m > 10)
                {
                    g0 = tab6dot13[2, j];
                }

            }
            else
            {
                while (n != tab6dot13[0, j])
                    j++;
            }

            j = 0;//g0 = tab6dot13[m, n];
            b_omega = Convert.ToDouble(textBox7.Text);
            v = Convert.ToDouble(textBox8.Text);
            a_w = Convert.ToDouble(textBox9.Text);
            u = Convert.ToDouble(textBox10.Text);

            omega_FV = delta_f * g0 * v * Math.Pow((a_w / u), 0.5);// w_t 5 formula



            //K_Fbeta = Convert.ToDouble(textBox11.Text);
            F_t = Convert.ToDouble(textBox11.Text);
            double K_Fa = 1;

            K_FV = 1 + omega_FV * b_omega / (F_t * K_Fa * K_Fbeta);// 4 formula

            omega_FT = (F_t / b_omega) * K_Fa * K_Fbeta * K_FV;//3 formula


            N_FE = Convert.ToDouble(textBox12.Text); // должно вводиться несколько сот тысяч или пару миллионов
            if (N_FE > N_F0)
            {
                sigma1_FP = sigmaF_dopusk * 1;

            }

            else if (N_FE <= N_F0)
            {
                sigma1_FP = sigmaF_dopusk * Math.Pow((N_F0 / N_FE), (1 / mf));// 2 formula

                if ((Math.Pow(((float) N_F0 / (float) N_FE), (1f / mf)) > 1.63) && (mf == 9))
                {
                    sigma1_FP = sigmaF_dopusk * 1.63;
                }
                else if ((Math.Pow(((float) N_F0 / (float) N_FE), (1f / mf)) > 2.08) && (mf == 6))
                {
                    sigma1_FP = sigmaF_dopusk * 2.08;
                }
                else
                {
                    sigma1_FP = sigmaF_dopusk * Math.Pow(((float) N_F0 / (float) N_FE), (1f / mf));
                }

            }

            Y_b = 1;
            Y_beta = 1;
            sigma_F = Y_F * Y_b * Y_beta / m;// конечный результат
            if (sigma_F <= sigma1_FP)
            {
                // здесь выводиться результат должен всех вычислений , т.е. omega_FV, K_FV, omega_FT, sigma_F, sigma_1_FP, но самое главное это sigma_F
                MessageBox.Show("ω_FV: " + omega_FV + "\nK_FV: " + K_FV + "\nω_FT: " + omega_FT + "\nσ_F: " + sigma_F + "\nσ_1_FP: " + sigma1_FP + "\n\nРезультат σ_F: " + sigma_F, "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgOut.Rows.Add();
                dgOut.Rows[RowCount-1].Cells[0].Value = omega_FV;
                dgOut.Rows[RowCount-1].Cells[1].Value = K_FV;
                dgOut.Rows[RowCount-1].Cells[2].Value = omega_FT;
                dgOut.Rows[RowCount-1].Cells[3].Value = sigma_F;
                dgOut.Rows[RowCount-1].Cells[4].Value = sigma1_FP;
                RowCount++;
                File.AppendAllText(path, omega_FV + "|" + K_FV + "|" + omega_FT + "|" + sigma_F + "|" + sigma1_FP + "\n");
            }
            else
            {
                //Show.Message("Errors!"); все ошибки с вычислениями или недопустимый коэффициент
            }
        }

        private void dgOut_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public int revers_or_not=1, for_K_Fbeta = 1;

        private void Label19_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }



        // radiobutton1
        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            revers_or_not = 1;
        }
        private void Нереверсивная_CheckedChanged(object sender, EventArgs e)
        {
            revers_or_not = 0;
        }



        //drugoi radiobutton
        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            for_K_Fbeta = 1;
        }
        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            for_K_Fbeta = 2;
        }
        private void RadioButton4_CheckedChanged(object sender, EventArgs e)
        {
            for_K_Fbeta = 3;
        }
        private void RadioButton5_CheckedChanged(object sender, EventArgs e)
        {
            for_K_Fbeta = 4;
        }
        //drugoi radiobutton


        //delta про модификацию головки зубьев
        private void RadioButton6_CheckedChanged(object sender, EventArgs e)
        {
            delta_f = 0.011;
        }

        private void RadioButton7_CheckedChanged(object sender, EventArgs e)
        {
            delta_f = 0.016;
        }
        //delta про модификацию головки зубьев  


        // таблицы 6.7, 6.5, 6.3, 6.13, 6.14

        //таблица 6.3
        public double[,] tab6dot7 = new double[23, 9]{
    { 0, 0.7, 0.5, 0.3, 0.1, 0, -0.1, -0.3, -0.5 },
    { 14, 3.12, 3.42, 3.78, 0, 0, 0, 0, 0 },
    { 16, 3.15, 3.40, 3.72, 0, 0, 0, 0, 0 },
    { 17, 3.16, 3.40, 3.67, 4.03, 4.26, 0, 0, 0 },
    { 18, 3.17, 3.39, 3.64, 3.97, 4.20, 0, 0, 0 },
    { 19, 3.18, 3.39, 3.62, 3.92, 4.11, 4.32, 0, 0 },
    { 20, 3.19, 3.39, 3.61, 3.89, 4.08, 4.28, 0, 0 },
    { 21, 3.20, 3.39, 3.60, 3.85, 4.01, 4.22, 0, 0 },
    { 22, 3.21, 3.39, 3.59, 3.82, 4.00, 4.20, 0, 0 },
    { 24, 3.23, 3.39, 3.58, 3.79, 3.92, 4.10, 0, 0 },
    { 25, 3.24, 3.39, 3.57, 3.77, 3.90, 4.05, 4.28, 0 },
    { 28, 3.27, 3.40, 3.56, 3.72, 3.82, 3.95, 4.22, 0 },
    { 30, 3.28, 3.40, 3.54, 3.70, 3.80, 3.90, 4.14, 0 },
    { 32, 3.29, 3.41, 3.54, 3.69, 3.78, 3.87, 4.08, 4.45 },
    { 37, 3.32, 3.42, 3.53, 3.64, 3.71, 3.80, 3.96, 4.20 },
    { 40, 3.33, 3.42, 3.53, 3.63, 3.70, 3.77, 3.92, 4.13 },
    { 45, 3.36, 3.43, 3.52, 3.62, 3.68, 3.72, 3.86, 4.02 },
    { 50, 3.38, 3.44, 3.52, 3.60, 3.65, 3.70, 3.81, 3.96 },
    { 60, 3.41, 3.47, 3.53, 3.59, 3.62, 3.67, 3.74, 3.84 },
    { 80, 3.45, 3.50, 3.54, 3.58, 3.61, 3.62, 3.68, 3.73 },
    { 100, 3.49, 3.52, 3.55, 3.58, 3.60, 3.61, 3.65, 3.68 },
    { 150, 0, 0, 0, 0, 3.60, 3.63, 3.63, 3.63 },
    { 500, 0, 0, 0, 0, 3.63, 0, 0, 0 }
};



        public double[,] tab6dot3a = new double[8, 3] {
    { 0.2, 1.0, 1.0 },
    { 0.4, 1.03, 1.01 },
    { 0.6, 1.05, 1.02 },
    { 0.8, 1.08, 1.05 },
    { 1.0, 1.15, 1.08 },
    { 1.2, 1.18, 1.10 },
    { 1.4, 1.25, 1.13 },
    { 1.6, 1.30, 1.16 }
};

        private void Label6_Click(object sender, EventArgs e)
        {

        }

        public double[,] tab6dot3b = new double[8, 3] {
    { 0.2, 1.02, 1.01 },
    { 0.4, 1.04, 1.04 },
    { 0.6, 1.13, 1.07 },
    { 0.8, 1.20, 1.11 },
    { 1.0, 1.27, 1.15 },
    { 1.2, 1.37, 1.20 },
    { 1.4, 1.50, 1.25 },
    { 1.6, 1.60, 1.32 }
};

        public double[,] tab6dot3c = new double[8, 3] {
    { 0.2, 1.1, 1.05 },
    { 0.4, 1.20, 1.12 },
    { 0.6, 1.30, 1.17 },
    { 0.8, 1.44, 1.23 },
    { 1.0, 1.57, 1.32 },
    { 1.2, 1.72, 1.40 },
    { 1.4, 1.85, 1.50 },
    { 1.6, 0, 1.60 }
};

        public double[,] tab6dot3d = new double[8, 3] {
    { 0.2, 1.25, 1.13 },
    { 0.4, 1.55, 1.28 },
    { 0.6, 1.90, 1.50 },
    { 0.8, 2.30, 1.70 },
    { 1.0, 0, 0 },
    { 1.2, 0, 0 },
    { 1.4, 0, 0 },
    { 1.6, 0, 0 }
};
        //таблица 6.3

        //таблица 6.13
        public double[,] tab6dot13 = new double[4, 5] {
    { 5, 6, 7, 8, 9 },
    { 28, 38, 47, 56, 73 },
    { 31, 42, 53, 61, 82 },
    { 37, 48, 64, 73, 100 }
};
        //таблица 6.13

        //таблица 6.5
        public double[,] tab6dot5 = new double[12, 3]{
    {195, 130, 4000000 },//HB 240...290 HRC 40...50
    {210, 160, 4000000 },//HRC 40...50
    {230, 180, 4000000 },//HB 240...280 HRC 40...52
    {220, 165, 4000000 },//HRC 45...50
    {200, 130, 4000000 },//HB 210...230
    {230, 150, 4000000 },//HB 240...280
    {230, 170, 4000000 },//HRC 48...52
    {270, 200, 4000000 },//HRC 48...55
    {320, 240, 4000000 },//HRC 52...56 HB 260...300
    {280, 210, 4000000 },//HRC52..62 HRC 26..35
    {330, 250, 4000000 },//HRC56..62 HRC 30..40
    {300, 220, 4000000 },//HRC56..62 HRC 30..40
};
        //таблица 6.5

        // таблицы 6.7, 6.5, 6.3, 6.13, 6.14
        //double findKFB(double bw, double dw1, int HB)
        //{
        //    int i = 0, j = 0;
        //    double result;
        //    double psi = bw / dw1;
        //
        //    if (psi > 1.6 || psi < 0.2)
        //    {
        //        result = 0;
        //        goto vixod1;
        //    }
        //
        //    if (HB >= 350)
        //        j = 1;
        //    else j = 2;
        //
        //    if (FLRadio == 1)
        //    {
        //        while (tab6dot3a[i, 0] < psi)
        //            i++;
        //        result = tab6dot3a[i, j];
        //
        //    }
        //    else
        //    {
        //        while (tab6dot3d[i, 0] < psi)
        //            i++;
        //        result = tab6dot3d[i, j];
        //    }
        //    vixod1:
        //    return result;
        //}
        //
        //double findKFB(double bw, float dw1, int HB, double Ldop)
        //{
        //    int i = 0, j = 0;
        //    double result;
        //    double psi = bw / dw1;
        //
        //    if (psi > 1.6 || psi < 0.2)
        //    {
        //        result = 0;
        //        goto vixod2;
        //    }
        //
        //
        //    if (HB >= 350)
        //        j = 1;
        //    else j = 2;
        //
        //    if (Ldop <= 6)
        //    {
        //        while (tab6dot3b[i, 0] < psi)
        //            i++;
        //        result = tab6dot3b[i, j];
        //
        //    }
        //    else
        //    {
        //        while (tab6dot3c[i, 0] < psi)
        //            i++;
        //        result = tab6dot3c[i, j];
        //    }
        //    vixod2:
        //    return result;
        //}
        //
        //double findg0(double m, int n)
        //{
        //    int i = 0, j = 0;
        //    //double nn = IntToDouble(n);
        //    if (m < 3.5)
        //        i = 1;
        //    else if (m < 10)
        //        i = 2;
        //    else i = 3;
        //
        //    while (tab6dot13[i, j] < n)
        //        j++;
        //    return tab6dot13[i, j];
        //}
        //
        //
        //double findYF(int zv, double x)
        //{
        //    int i = 1, j = 1;
        //    while (tab6dot7[i, 0] < zv)
        //        i++;
        //    while (tab6dot7[0, j] > x)
        //        j++;
        //
        //    return tab6dot7[i, j];
        //
        //}

        //формулы
        private void Button1_Click(object sender, EventArgs e)//кнопка для вычислений
        {

        }


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(path))
            {
                if (File.ReadAllText(path) != "")
                {
                    using (StreamReader sr = File.OpenText(path))
                    {
                        string s;
                        while ((s = sr.ReadLine()) != null)
                        {
                            dgOut.Rows.Add();
                            RowCount++;
                            String buf = "";
                            int stringCount = 0;
                            for (int i = 0; i < s.Length; i++)
                            {
                                if (s[i] == '|')
                                {
                                    dgOut.Rows[RowCount - 1].Cells[stringCount].Value = buf;
                                    buf = "";
                                    stringCount++;
                                    continue;
                                }
                                buf += s[i];
                            }
                        }
                    }
                    dgOut.RowCount = RowCount;
                }
                else {
                    RowCount++;
                }
            }
            else { 
                File.Create(path);
                RowCount++;
                dgOut.RowCount = RowCount;
            }
        }
    }
}
