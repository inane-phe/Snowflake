using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snowflake
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static Graphics g;
        static Pen pen3, pen1, pen2;
        int ro = 0;
        PointF point1 = new PointF(240, 120);
        PointF point2 = new PointF(720, 120);
        PointF point3 = new PointF(480, 440);
        
        private float rotationAngle = 0.2f; // 20% 的旋轉角度
        /// <summary>
        /// Построение фрактала
        /// </summary>
        /// <param name="p1">Первая точка</param>
        /// <param name="p2">Вторая точка</param>
        /// <param name="p3">3 точка для координации</param>
        /// <param name="lvl">Колличество итераций</param>
        /// <returns></returns>
        static int Fractal(PointF p1, PointF p2, PointF p3, int lvl)
        {

            if (lvl > 0)
            {
                //средняя треть отрезка
                var p4 = new PointF((p2.X + 2 * p1.X) / 3, (p2.Y + 2 * p1.Y) / 3);
                var p5 = new PointF((2 * p2.X + p1.X) / 3, (p1.Y + 2 * p2.Y) / 3);
                //координаты вершины угла
                var ps = new PointF((p2.X + p1.X) / 2, (p2.Y + p1.Y) / 2);          //邊的中點
                var pn = new PointF((4 * ps.X - p3.X) / 3, (4 * ps.Y - p3.Y) / 3);  //突出的角
                //рисуем его
                g.DrawLine(pen1, p4, pn);
                g.DrawLine(pen2, p5, pn);
                g.DrawLine(pen3, p4, p5);


                //рекурсивно вызываем функцию нужное число раз
                Fractal(p4, pn, p5, lvl - 1);
                Fractal(pn, p5, p4, lvl - 1);
                Fractal(p1, p4, new PointF((2 * p1.X + p3.X) / 3, (2 * p1.Y + p3.Y) / 3), lvl - 1);
                Fractal(p5, p2, new PointF((2 * p2.X + p3.X) / 3, (2 * p2.Y + p3.Y) / 3), lvl - 1);

            }
            return lvl;
        }

        private void Draw(int lvl)
        {
            SetOption();
            DrawRectangle(ref g);
            AllFractal(lvl);
        }

        private void AllFractal(int lvl)
        {
            Fractal(point1, point2, point3, lvl);
            Fractal(point2, point3, point1, lvl);
            Fractal(point3, point1, point2, lvl);
        }

        private void SetOption()
        {
            SetPens();
            ClearPicBox();
        }

        private void SetPens()
        {
            pen1 = new Pen(Color.Blue, trackBar2.Value);
            pen2 = new Pen(Color.Orange, trackBar2.Value);
            pen3 = new Pen(Color.White, trackBar2.Value+1);
            
        }

        private void ClearPicBox()
        {
            g = picBox.CreateGraphics();
            g.Clear(Color.White);
        }
        private void DrawRectangle(ref Graphics g)
        {
            g.DrawLine(pen1, point1, point2);
            g.DrawLine(pen1, point2, point3);
            g.DrawLine(pen1, point3, point1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Draw(Convert.ToInt32(counter1.Value));
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            ro = ro + 20;
            Rotate(ro);
        }
        private void Rotate(double angle)
        {
            // 旋转角度
            double angleInRadians = angle * Math.PI / 180.0;

            // 获取当前图形的中心点
            PointF center = new PointF((point1.X + point2.X + point3.X) / 3, (point1.Y + point2.Y + point3.Y) / 3);

            // 旋转每个顶点
            point1 = RotatePoint(point1, center, angleInRadians);
            point2 = RotatePoint(point2, center, angleInRadians);
            point3 = RotatePoint(point3, center, angleInRadians);

            // 清空图形并重新绘制
            ClearPicBox();
            DrawRectangle(ref g);
            AllFractal(Convert.ToInt32(counter1.Value));
            picBox.Update();
        }

        private PointF RotatePoint(PointF point, PointF center, double angle)
        {
            float x = center.X + (float)((point.X - center.X) * Math.Cos(angle) - (point.Y - center.Y) * Math.Sin(angle));
            float y = center.Y + (float)((point.X - center.X) * Math.Sin(angle) + (point.Y - center.Y) * Math.Cos(angle));

            return new PointF(x, y);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            ResetRotation();
        }
        private void ResetRotation()
        {
            point1 = new PointF(240, 120);
            point2 = new PointF(720, 120);
            point3 = new PointF(480, 440);

            // 清空图形并重新绘制
            ClearPicBox();
            DrawRectangle(ref g);
            AllFractal(Convert.ToInt32(counter1.Value));
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

            label3.Text = trackBar1.Value.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 100/trackBar1.Value;
            ro = ro + 20;
            Rotate(ro);
            

        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label4.Text = trackBar2.Value.ToString();
        }       
    }
}
