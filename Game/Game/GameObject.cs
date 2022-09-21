using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Game
{
    public class Player
    {
        public Point point;             //Положение ракеты в 2D-пространстве
        public Size size;               //Размеры ракеты
        public Region reg;              //Занимаемая ракетой область в пространстве
        public Pen laser_pen;           //Свойство лазера
        public Bullets[] bullets;       //Массив объекта лазера
        public int manybullet;          //Количество лазеров
        public int health;              //Здоровье ракеты
        public bool live;               //Состояния ракеты: жив (true) или мертв (false)

        //Задать свойства (параметры) ракеты
        public void New_player(Form1 F)     
        {
            size = F.imageP.Size;
            point.X = 0;
            point.Y = 0;
            Rectangle rec = new Rectangle(point, size);
            reg = new Region(rec);
            laser_pen = new Pen(new HatchBrush(HatchStyle.DashedUpwardDiagonal, F.bc.PlayerLaserColor, F.bc.PlayerLaserColor), 3);
            health = 6;
            live = true;
        }

        //Отображение ракеты на поле битвы.
        public void Show_player(Form1 F, int x, int y) 
        {
            F.g.ResetClip();
            F.g.FillRegion(new SolidBrush(F.BackColor), reg);
            point.X = (F.klava) ? x : x - size.Width / 2;
            point.Y = y;
            Rectangle rec = new Rectangle(point, size);
            reg = new Region(rec);
            F.g.DrawImage(F.imageP, point);
            F.g.ExcludeClip(reg);
        }

        //Действия после получения урона
        public void Damage_player(Form1 F, Point point, Size size, Bullets bullet = null) 
        {
            //Прямоугольник объекта, т.е. лазер или летающая тарелка
            Rectangle r = new Rectangle(point.X, point.Y, size.Width, size.Height);
            //Если прямоугольник пересекает область ракеты, то уменьшает здоровье ракеты.
            if (reg.IsVisible(r, F.g))
            {
                health--;
                //Если здоровье равно нулю, то игра завершена
                if (health == 0)
                {
                    live = false;
                }
                if (bullet != null)
                {
                    bullet.delete = true;
                }
            }
        }
    }

    public class Bugs
    {
        public Point point;                 //Положение летающей тарелки в 2D-пространстве
        public Size size;                   //Размеры летающей тарелки
        int veloX;                          //Скорость смещения летающей тарелки по X
        int veloY;                          //Скорость_падения летающей тарелки по Y
        public HatchBrush br;               //Кисть для покраски летающей тарелки
        public Region reg;   //Занимаемая летающей тарелкой область в пространстве
        public bool life;         //Состояния летающей тарелки: жив (true) или мертв (false)

        //Задать свойства (параметры) летающей тарелки
        public void New_bug(Form1 F, int rch) 
        {
            reg = new Region();
            life = true;
            Random rv = new Random(rch);
            point.X = rv.Next(10, F.Width - 40);
            point.Y = rv.Next(10, F.Height / 5);
            size.Width = rv.Next(20, 50);
            size.Height = size.Width * 2 / 3;
            veloX = rv.Next(7) - 3;
            veloY = rv.Next(3, 10);
            br = F.bc.New_br(rch);
            reg = Form_bug();
        }

        //Создать форму летающей тарелки
        public Region Form_bug()    
        {
            Point pt = new Point();
            Size st = new Size();
            pt.X = point.X;
            pt.Y = point.Y + size.Height / 4;
            st.Width = size.Width;
            st.Height = size.Height / 2;
            Rectangle rec = new Rectangle(pt, st);
            GraphicsPath path1 = new GraphicsPath();
            path1.AddEllipse(rec);
            Region reg = new Region(path1);
            rec.X = point.X + size.Width / 4;
            rec.Y = point.Y;
            rec.Width = size.Width / 2;
            rec.Height = size.Height;
            path1.AddEllipse(rec);
            reg.Union(path1);
            return reg;
        }

        //Перемещение летающей тарелки
        public void Move_bug() 
        {
            point.X += veloX;
            point.Y += veloY;
            reg = Form_bug();
        }
    }

    public class Enemies
    {
        public int N;                //Актуальное количество летающих тарелок на экране
        public Bugs[] bugs;          //Массив  объектов летающих тарелок
        public int manybullet;       //Количество лазеров у летающей тарелок
        public Bullets[] bullets;    //Массив объекта лазера
        public Pen laser_pen;        //Свойство лазера

        //Инициализация объектов летающих тарелок
        public void New_Enemies(Form1 F)    
        {
            N = 20;
            Array.Resize(ref bugs, N);
            for (int j = 0; j < N; j++)
            {
                bugs[j] = new Bugs();
            }
            laser_pen = new Pen(new HatchBrush(HatchStyle.DashedUpwardDiagonal, F.bc.EnemiesLaserColor, F.bc.EnemiesLaserColor), 3);
        }

        //Передвижения летающих тарелок.
        public void Show_bugs(Form1 F) 
        {
            for (int j = 0; j < N; j++)
            {                
                F.g.FillRegion(new HatchBrush(HatchStyle.DashedUpwardDiagonal, F.bc.FonColor), bugs[j].reg);
                bugs[j].Move_bug();
                Killed_bugs(F, F.player.point, F.player.size);
                //Летающая тарелка находящаяся за пределами экрана уничтожается.
                if (bugs[j].point.Y > F.Height || bugs[j].point.X > F.Width || bugs[j].point.X + bugs[j].size.Width < 0)
                {
                    N--;
                    for (int i = j; i < N; i++)
                    {
                        bugs[i] = bugs[i + 1];
                    }
                    Array.Resize(ref bugs, N);
                }
                else
                {
                    F.g.FillRegion(bugs[j].br, bugs[j].reg);
                }
            }
        }

        //Добавление одной серии (20 шт.) летающих тарелок.
        public void Enemy(Form1 F) 
        {
            int N0 = N - 20;
            int rch;
            Random rnd = new Random();
            for (int j = N0; j < N; j++)
            {
                bugs[j] = new Bugs();
                rch = rnd.Next();
                bugs[j].New_bug(F, rch);
                F.g.FillRegion(bugs[j].br, bugs[j].reg);
            }
        }

        //Определение и удаление сбитой летающей тарелки
        public void Killed_bugs(Form1 F, Point point, Size size, Bullets bullet = null) 
        {
            Rectangle r;
            for (int j = 0; j < N; j++)
            {         
                //Прямоугольник объекта, т.е. лазер или ракета
                r = new Rectangle(point.X, point.Y, size.Width, size.Height);
                //Если прямоугольник пересекает область летающей тарелки, то уничтожается летающая тарелка.
                if (bugs[j].reg.IsVisible(r, F.g))
                {
                    F.g.FillRegion(bugs[j].br, bugs[j].reg);
                    bugs[j].life = false;
                    F.g.FillRegion(new HatchBrush(HatchStyle.DashedUpwardDiagonal, F.bc.FonColor), bugs[j].reg);
                    //Если лазер попадает в летающую тарелку, то он исчезает.
                    if (bullet != null)
                    {
                        bullet.delete = true;
                    }
                    else //Если ракета сталкивается с летающей тарлекой, то здоровье ракеты уменьшается.
                    {
                        F.player.Damage_player(F, bugs[j].point, bugs[j].size);
                    }

                    F.Result += 1;

                    N--;
                    for (int i = j; i < N; i++)
                    {
                        bugs[i] = bugs[i + 1];
                    }
                    Array.Resize(ref bugs, N);

                    break;
                }
            }
        }
    }

    public class BrushColor
    {
        public Color FonColor;             // цвет фона
        public Color PlayerLaserColor;     // цвет лазера ракеты
        public Color EnemiesLaserColor;    // цвет лазера летающей тарелки
        public Color DashBug;              // цвет штриховки летающей тарелки

        //Конструктор (настройка цветов)
        public BrushColor() 
        {
            FonColor = Color.Black;
            PlayerLaserColor = Color.Blue;
            EnemiesLaserColor = Color.Red;
            DashBug = Color.Blue;
        }

        //Кисть для задания цвета летающей тарелки
        public HatchBrush New_br(int rch) 
        {
            return new HatchBrush(HatchStyle.DashedUpwardDiagonal, DashBug, RandomColor(rch));
        }

        //Генератор случайного цвета (rch - слчайное число)
        public Color RandomColor(int rch)      
        {
            int r, g, b;
            byte[] bytes1 = new byte[3];        // массив 3 цветов
            Random rnd1 = new Random(rch);
            rnd1.NextBytes(bytes1);             // генерация в массив
            r = Convert.ToInt16(bytes1[0]);
            g = Convert.ToInt16(bytes1[1]);
            b = Convert.ToInt16(bytes1[2]);
            return Color.FromArgb(r, g, b);     // возврат цвета
        }
    }

    public class Bullets
    {
        public Point point1;            //Положение начальной точки лазера в 2D-пространстве
        public Point point2;            //Положение конечной точки лазера в 2D-пространстве
        public bool delete;             //Состояние лазера, если true, то лазер исчезает
        public Size size;               //Размер лазера

        //Задать свойства (параметры) лазера
        public void new_bullet(Form1 F, int x, int y) 
        {
            size.Width = 3;
            size.Height = 35;
            point1.X = x;
            point1.Y = y;
            point2.X = x;
            point2.Y = y + size.Height;
            F.g.DrawLine(F.player.laser_pen, point1.X, point1.Y, point2.X, point2.Y);
            delete = false;
        }

        //Передвижение лазера
        public void Show_bullet(Form1 F, int num, int veloY, Bullets[] bullets, int manybullet, Pen laser_pen, string clas)
        {
            Pen deleteLine = new Pen(new HatchBrush(HatchStyle.DashedUpwardDiagonal, F.bc.FonColor, F.bc.FonColor), 3);
            F.g.DrawLine(deleteLine, point1, point2);
            point1.Y += veloY;
            point2.Y += veloY;
            //Если лазеры находится за пределмаи экрана, то они исчезают
            if(point2.Y < 0)
            {
                F.player.manybullet--;
                delete_bullet(F, bullets, F.player.manybullet, num, "Player");
            }
            else if (point1.Y > F.Height)
            {
                F.nlo.manybullet--;
                delete_bullet(F, bullets, F.nlo.manybullet, num, "Enemies");
            }

            F.g.DrawLine(laser_pen, point1, point2);

            //Контроль на предмет попадания в объект
            if (clas == "Player")
            {
                F.nlo.Killed_bugs(F, point2, size, this);
            }
            else if (clas == "Enemies")
            {
                F.player.Damage_player(F, point2, size, this);
            }

            //Если есть уничтоженные лазеры, то эти лазеры исчезают с экрана
            if (delete)
            {
                if (clas == "Player")
                {
                    F.player.manybullet--;
                    delete_bullet(F, bullets, F.player.manybullet, num, "Player");
                }
                else if (clas == "Enemies")
                {
                    F.nlo.manybullet--;
                    delete_bullet(F, bullets, F.nlo.manybullet, num, "Enemies");
                }
            }
        }

        //Удаление лазера.
        public void delete_bullet(Form1 F, Bullets[] bullets, int manybullet, int num, string clas)
        {
            //Окрашивает лазер в цвет фона.
            Pen deleteLine = new Pen(new HatchBrush(HatchStyle.DashedUpwardDiagonal, F.bc.FonColor, F.bc.FonColor), 3);
            F.g.DrawLine(deleteLine, point1, point2);

            Bullets[] newbullets = new Bullets[manybullet];
            //Изменения массива объекта лазера.
            for (int i = num; i < manybullet; i++)
            {
                if (clas == "Player")
                {
                    F.player.bullets[i] = F.player.bullets[i + 1];
                }
                else if (clas == "Enemies")
                {
                    F.nlo.bullets[i] = F.nlo.bullets[i + 1];
                }
            }
        }
    }
}
