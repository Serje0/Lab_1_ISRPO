using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Game
{
    public partial class Form5 : Form
    {
        Thread th; //Создание потока, для открытия других форм после закрытия этой формы
        bool clickBt = false; //Подверждение нажатия кнопки, оно необходимо для предоварщения првторного запроса выхода в событие Form5_FormClosing
        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            //Выравнивание компонентов по центру
            label1.Location = new Point((this.Size.Width / 2) - (label1.Size.Width / 2), 30);
            label2.Location = new Point((this.Size.Width / 2) - (label2.Size.Width / 2), 110);
            button1.Location = new Point((this.Size.Width / 2) - (button1.Size.Width / 2), 450);
        }

        //Событие нажатия кнопки "Назад". После нажатия кнопки, открывается форма главного меню (form2), а эта форма закрывается
        private void button1_Click(object sender, EventArgs e)
        {
            clickBt = true;
            this.Close();
            th = new Thread(open);
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }

        public void open(Object obj) //Открытие формы главного меню (form2)
        {
            Application.Run(new Form2());
        }

        //Событие процесса закрытия формы. Если вручную закрыть программа (например, Alt+F4) и подтвердить выход, то закрывается программа
        private void Form5_FormClosing(object sender, FormClosingEventArgs e)
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
