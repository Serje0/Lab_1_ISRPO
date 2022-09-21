using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Game
{
    public partial class Form2 : Form
    {
        Thread th; //Создание потока, для открытия других форм после закрытия этой формы
        bool clickBt = false; //Подверждение нажатия кнопки, оно необходимо для предоварщения првторного запроса выхода в событие Form2_FormClosing
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //Записывает текст первой кнопки в зависимости от ситуации
            button1.Text = Properties.Settings.Default.F2TextButton1;
            //Выравнивание компонентов по центру
            label1.Location = new Point((this.Size.Width / 2) - (label1.Size.Width / 2), 30);
            button1.Location = new Point((this.Size.Width / 2) - (button1.Size.Width / 2), 130);
            button2.Location = new Point((this.Size.Width / 2) - (button2.Size.Width / 2), 204);
            button3.Location = new Point((this.Size.Width / 2) - (button3.Size.Width / 2), 278);
            button4.Location = new Point((this.Size.Width / 2) - (button4.Size.Width / 2), 352);
            button5.Location = new Point((this.Size.Width / 2) - (button5.Size.Width / 2), 426);
            button6.Location = new Point((this.Size.Width / 2) - (button6.Size.Width / 2), 500);
            //Если открыта игровая форма(form1), то некоторые кнопки выключены
            Form1 F = (Form1)Application.OpenForms["Form1"];
            if (F != null)
            {
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
            }
        }

        //Событие нажатия кнопки "Играть" или "Продолжить"
        private void button1_Click(object sender, EventArgs e) 
        {
            clickBt = true;
            Form1 F = new Form1();
            if (button1.Text == "Играть") //Октрывает игровую форму(form1)
            {
                Properties.Settings.Default.F2TextButton1 = "Продолжить";
                this.Close();
                th = new Thread(openForm1);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();

            }
            else if(button1.Text == "Продолжить") //В режиме паузы, закрывает форму
            {
                this.Close();
            }
        }

        //Событие нажатия кнопки "Рекорды". После нажатия кнопки, открывает форму для просмотра рекорда (form6), а эту форму закрывает
        private void button2_Click(object sender, EventArgs e) 
        {
            clickBt = true;
            this.Close();
            th = new Thread(openForm6);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        //Событие нажатия кнопки "Управление". После нажатия кнопки, открывает форму для изменения способа управления (form4), а эту форму закрывает
        private void button3_Click(object sender, EventArgs e)
        {
            clickBt = true;
            this.Close();
            th = new Thread(openForm4);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        //Событие нажатия кнопки "Инструкция". После нажатия кнопки, открывает форму для просмотра инструкции (form5), а эту форму закрывает
        private void button4_Click(object sender, EventArgs e)
        {
            clickBt = true;
            this.Close();
            th = new Thread(openForm5);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        //Событие нажатия кнопки "Справка". После нажатия кнопки, открывает форму для просмотра справки (form7), а эту форму закрывает
        private void button5_Click(object sender, EventArgs e) 
        {
            clickBt = true;
            this.Close();
            th = new Thread(openForm7);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        //Событие нажатия кнопки "Выход". После нажатия кнопки запрашивает подверждения. Если игрок подверждает выход, то программа закрывается
        private void button6_Click(object sender, EventArgs e)
        {
            clickBt = true;
            string text = "Вы уверены хотите выйти?";
            if (button1.Text == "Продолжить")
            {
                text += " Иначе набранные очки потеряют!";
            }
            if (MessageBox.Show(text, "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                Properties.Settings.Default.F2TextButton1 = "Играть";
                Properties.Settings.Default.Exit = true;
                Application.Exit();
            }
        }

        public void openForm1(Object obj) //Открытие игровой формы (form1)
        {
            Application.Run(new Form1());
        }

        public void openForm6(Object obj) //Открытие формы для просмотра рекородов (form6)
        {
            Application.Run(new Form6());
        }

        public void openForm4(Object obj) //Открытие формы для изменения способа управления (form4)
        {
            Application.Run(new Form4());
        }

        public void openForm5(Object obj) //Открытие формы для просмотра инструкции (form5)
        {
            Application.Run(new Form5());
        }

        public void openForm7(Object obj) //Открытие формы для просмотра справки (form7)
        {
            Application.Run(new Form7());
        }

        //Событие процесса закрытия формы. Если вручную закрыть программа (например, Alt+F4) и подтвердить выход, то закрывается программа
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1 F = (Form1)Application.OpenForms["Form1"];
            if (!clickBt && F == null)
            {
                if (MessageBox.Show("Вы уверены хотите выйти?", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
            else if (!clickBt && F != null && !Properties.Settings.Default.Exit)
            {
                if (MessageBox.Show("Вы уверены хотите выйти? Иначе набранные очки потеряют!", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    Properties.Settings.Default.Exit = true;
                    Application.Exit();
                }
            }
        }
    }
}
