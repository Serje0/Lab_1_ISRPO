using System;
using System.Drawing;
using System.Windows.Forms;

namespace Game
{
    public partial class Form1 : Form
    {
        public Player player = new Player();     //Ракета, которая сбивает летающую тарелку (объект)
        public Bitmap imageP;                    //Изображения ракеты
        public int Result = 0;                   //Количество сбитых летающих тарелок (счет игры)
        public Graphics g;                       //Холст для битвы
        public BrushColor bc = new BrushColor(); //Набор кистей и цветов
        public Enemies nlo = new Enemies();      //Все летающие тарелки
        public bool klava = Properties.Settings.Default.ControlKlava; //Способ управления: true - через клавиатуру, false - через мышь
        public bool start = false;               //Переменные для появления рисунка ракеты

        public Form1()
        {
            InitializeComponent();         
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            g = this.CreateGraphics();          //Инициализация холста
            BackColor = bc.FonColor;            //Цвет фона
            imageP = new Bitmap(imageList1.Images[0], 50, 100); //Рисунок ракеты
            player.New_player(this);            //Инициализация ракеты
            nlo = new Enemies();                //Инициализация летающей тарелки
            nlo.New_Enemies(this);              //Инициализация летающих тарелок, как объектов           
            nlo.Enemy(this);                    //Генерация одной серии летающих тарелок
            //Запускает счетчики
            timer1.Start();
            timer2.Start();
            timer3.Start();
            //Вырванивает картинку.
            pictureBox1.Location = new Point( this.Size.Width - (32 * player.health + 15), 2);
            Cursor.Hide(); //Скрыть курсор.
            //Изменить положение курсора.
            if (!klava)
            {
                Cursor.Position = new Point(this.Width / 2, (this.Height * 3) / 4);
            }
        }

        //Событие для счетчика. По умолчанию стоит интервал в 20. Каждую 0,02 секнуду летающие тарелки движутся.
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Отображение рисунка ракеты после откртия формы
            if (!start)
            {
                player.Show_player(this, this.Width / 2, (this.Height * 3) / 4);
            }


            nlo.Show_bugs(this);
            label1.Text = "Очки: " + Result.ToString();
            pictureBox1.Width = 32 * player.health;
            //Действия после уничтожения ракеты.         
            if (!player.live)
            {
                //Для начала сохранить набранные очки. Если набранные очки превышают рекорд, то рекорд обновляется и сохраняется.
                bool exit = Properties.Settings.Default.Exit;
                Properties.Settings.Default.LastScored = Result;
                if(Properties.Settings.Default.BestRecord < Result)
                {
                    Properties.Settings.Default.BestRecord = Result;
                }
                Properties.Settings.Default.F2TextButton1 = "Играть";
                Properties.Settings.Default.ControlKlava = true;
                Properties.Settings.Default.Exit = false;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Exit = exit;
                Properties.Settings.Default.F2TextButton1 = "Продолжить";
                Properties.Settings.Default.ControlKlava = klava;
                Cursor.Show(); //Отображает курсор
                //Оставнавливются счетчики
                timer1.Stop();
                timer2.Stop();
                timer3.Stop();
                timer4.Stop();
                //Отображается итоговая форма (form3)
                Form3 F = new Form3();
                F.ShowDialog();
            }            
        }

        //Событие для счетчика. По умолчанию стоит интервал в 3000.
        //При завершении предыдущего интревала, следующий интервал уменьшается на 100, пока не дойдет до 2500.
        //Каждые следующий интервал, добавляет еще 1 серию летающих тарелок, т.е. еще 20 летающих тарелок.
        private void timer2_Tick(object sender, EventArgs e)
        {
            nlo.N += 20;
            Array.Resize(ref nlo.bugs, nlo.N);
            if (timer2.Interval > 2500)
            {
                timer2.Interval -= 100;
            }
            nlo.Enemy(this);          
        }

        //Событие для счетчика. По умолчанию стоит интервал в 500. Каждые 0,5 секунду запускает лазеры.
        private void timer3_Tick(object sender, EventArgs e)
        {
            //Добваяет лазер от ракеты
            player.manybullet++;
            Array.Resize(ref player.bullets, player.manybullet);
            player.bullets[player.manybullet - 1] = new Bullets();
            player.bullets[player.manybullet - 1].new_bullet(this, player.point.X + player.size.Width / 2, player.point.Y);

            //Добваяет лазеры от летающей тарелки
            Random rnd = new Random();
            int num1 = rnd.Next(1, nlo.bugs.Length - 1);
            nlo.manybullet++;
            Array.Resize(ref nlo.bullets, nlo.manybullet);
            nlo.bullets[nlo.manybullet - 1] = new Bullets();
            nlo.bullets[nlo.manybullet - 1].new_bullet(this, nlo.bugs[num1].point.X + nlo.bugs[num1].size.Width / 2, nlo.bugs[num1].point.Y);

            int num2 = rnd.Next(1, nlo.bugs.Length - 1);
            nlo.manybullet++;
            Array.Resize(ref nlo.bullets, nlo.manybullet);
            nlo.bullets[nlo.manybullet - 1] = new Bullets();
            nlo.bullets[nlo.manybullet - 1].new_bullet(this, nlo.bugs[num2].point.X + nlo.bugs[num2].size.Width / 2, nlo.bugs[num2].point.Y);

            if (timer2.Interval <= 2500)
            {
                int num3 = rnd.Next(1, nlo.bugs.Length - 1);
                nlo.manybullet++;
                Array.Resize(ref nlo.bullets, nlo.manybullet);
                nlo.bullets[nlo.manybullet - 1] = new Bullets();
                nlo.bullets[nlo.manybullet - 1].new_bullet(this, nlo.bugs[num3].point.X + nlo.bugs[num3].size.Width / 2, nlo.bugs[num3].point.Y);
            }

            //Запускает счетчик для передвижения лазера
            if (!start)
            {
                timer4.Start();
                start = true;
            }
        }

        //Событие для счетчика. По умолчанию стоит интервал в 25. Каждые 0,025 секунды лазеры движутся
        private void timer4_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < player.manybullet; i++)
            {
                player.bullets[i].Show_bullet(this, i, -15, player.bullets, player.manybullet, player.laser_pen, "Player");
            }
            for (int i = 0; i < nlo.manybullet; i++)
            {
                nlo.bullets[i].Show_bullet(this, i, 15, nlo.bullets, nlo.manybullet, nlo.laser_pen, "Enemies");
            }
        }

        //Событие нажатия клавиш. Этот события отвечает за управление ракетой, если способ управления был выбран через клавиатуру.
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (klava)
            {
                if (e.KeyCode == Keys.Up && player.point.Y - 15 > 0)
                {
                    player.Show_player(this, player.point.X, player.point.Y - 15);
                }
                if (e.KeyCode == Keys.Down && player.point.Y + player.size.Height + 10 < this.Height)
                {
                    player.Show_player(this, player.point.X, player.point.Y + 15);
                }
                if (e.KeyCode == Keys.Left && player.point.X - 15 > 0)
                {
                    player.Show_player(this, player.point.X - 15, player.point.Y);
                }
                if (e.KeyCode == Keys.Right && player.point.X + player.size.Width + 10 < this.Width)
                {
                    player.Show_player(this, player.point.X + 15, player.point.Y);
                }
            }
        }

        //Событие движения мыши. Этот события отвечает за управление ракетой, если способ управления был выбран через мышь.
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {          
            if (!klava)
            {
                player.Show_player(this, e.X, e.Y);
            }
        }

        //Событие отпускания клавиши. Это событие имеет возможность поставить паузу, нажимая Escape
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Cursor.Show(); //Отображает курсор
                //Останавливают счетчики
                timer1.Stop();
                timer2.Stop();
                timer3.Stop();
                timer4.Stop();
                //Открытие формы главного меню (form2)
                Form2 F = new Form2();
                F.ShowDialog(); 
                //Если в главном меню, пользователь выбрал продолжить игру, то игра воозбновляется.
                Cursor.Position = new Point(player.point.X, player.point.Y);
                timer1.Start();
                timer2.Start();
                timer3.Start();
                timer4.Start();
                Cursor.Hide(); //Скрывает курсор     
            }
        }

        //Событие процесса закрытия формы. Если вручную закрыть программа (например, Alt+F4) и подтвердить выход, то закрывается программа
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (player.live && !Properties.Settings.Default.Exit)
            {
                Cursor.Show();
                timer1.Stop();
                timer2.Stop();
                timer3.Stop();
                timer4.Stop();

                if (MessageBox.Show("Вы уверены хотите выйти? Иначе набранные очки потеряют!", "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    e.Cancel = true;
                    Cursor.Position = new Point(player.point.X, player.point.Y);
                    timer1.Start();
                    timer2.Start();
                    timer3.Start();
                    timer4.Start();
                    Cursor.Hide();
                }
            }           
        }
    }
}