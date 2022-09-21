using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Game
{
    public partial class Form3 : Form
    {
        Thread th; //Создание потока, для открытия других форм после закрытия этой формы
        bool clickBt = false; //Подверждение нажатия кнопки, оно необходимо для предоварщения првторного запроса выхода в событие Form3_FormClosing
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            //Записывает рекорд и последние набранные очки от предыдущей работы приложения
            label3.Text = Properties.Settings.Default.BestRecord.ToString();
            label5.Text = Properties.Settings.Default.LastScored.ToString();
            //Выравнивание компонентов
            label1.Location = new Point((this.Size.Width / 2) - (label1.Size.Width / 2), 30);
            label2.Location = new Point((this.Size.Width / 2) - (label2.Size.Width / 2), 140);
            label3.Location = new Point((this.Size.Width / 2) - (label3.Size.Width / 2), 200);
            label4.Location = new Point((this.Size.Width / 2) - (label4.Size.Width / 2), 265);
            label5.Location = new Point((this.Size.Width / 2) - (label5.Size.Width / 2), 320);
            button1.Location = new Point(30, 400);
            button2.Location = new Point(this.Size.Width - button2.Size.Width - 30, 400);
        }

        //Событие нажатия кнопки "Снова сыграть". После нажатия кнопки, открывается игровая форма (form1), а эта форма закрывается
        private void button1_Click(object sender, EventArgs e)
        {
            clickBt = true;
            Form1 F = (Form1)Application.OpenForms["Form1"];
            F.Close();

            this.Close();
            th = new Thread(openForm1);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        //Событие нажатия кнопки "В меню". После нажатия кнопки, открывается форма главного меню (form2), а эта форма закрывается
        private void button2_Click(object sender, EventArgs e)
        {
            clickBt = true;
            Form1 F = (Form1)Application.OpenForms["Form1"];
            F.Close();

            Properties.Settings.Default.F2TextButton1 = "Играть";
            this.Close();
            th = new Thread(openForm2);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }
        public void openForm1(Object obj) //Открытие игровой формы (form1)
        {
            Application.Run(new Form1());
        }

        public void openForm2(Object obj) //Открытие формы главного меню (form2)
        {
            Application.Run(new Form2());
        }

        //Событие процесса закрытия формы. Если вручную закрыть программа (например, Alt+F4) и подтвердить выход, то закрывается программа
        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!clickBt)
            {
                if (MessageBox.Show("Вы уверены хотите выйти?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
