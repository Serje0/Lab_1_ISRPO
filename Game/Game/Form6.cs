using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Game
{
    public partial class Form6 : Form
    {
        Thread th; //Создание потока, для открытия других форм после закрытия этой формы
        bool clickBt = false; //Подверждение нажатия кнопки, оно необходимо для предоварщения првторного запроса выхода в событие Form6_FormClosing
        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            //Записывает рекорд и последние набранные очки от предыдущей работы приложения.
            label3.Text = Properties.Settings.Default.BestRecord.ToString();
            label5.Text = Properties.Settings.Default.LastScored.ToString();
            //Выравнивание компонентов по центру
            label1.Location = new Point( (this.Size.Width/2) - (label1.Size.Width/2) , 30);
            label2.Location = new Point((this.Size.Width / 2) - (label2.Size.Width / 2), 160);
            label3.Location = new Point((this.Size.Width / 2) - (label3.Size.Width / 2), 200);
            label4.Location = new Point((this.Size.Width / 2) - (label4.Size.Width / 2), 265);
            label5.Location = new Point((this.Size.Width / 2) - (label5.Size.Width / 2), 305);
            button1.Location = new Point((this.Size.Width / 2) - (button1.Size.Width / 2), 400);
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
        private void Form6_FormClosing(object sender, FormClosingEventArgs e)
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
