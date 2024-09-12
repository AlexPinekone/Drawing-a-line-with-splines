using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Grafica
{
    public partial class Form1 : Form
    {
        //Variables necesarias
        //Lo que dicen lol
        private bool estaArrastrando1 = false;
        private bool estaArrastrando2 = false;
        private bool estaArrastrando3 = false;
        private bool estaArrastrando4 = false;
        //Los dos objetos para mover
        private Rectangle movi1;
        private Rectangle movi2;
        private Rectangle moviI;
        private Rectangle moviF;
        //La posicion de esos dos objetos 
        private Point posicionI1, posicionI2, posicionI3, posicionI4;
        //Saber si esta una curva activa
        private bool hayCurva;
        //Los valores de la curva deben ser globales para cargarla cada vez que se actualice todo
        private int xI, yI, xF, yF, x1, y1, x2, y2;


        //Se crea la ventana y se limpia el Panel
        public Form1()
        {
            InitializeComponent();
            LimpiarPanel();
        }
        //Se limpia el panel y se crean dos lineas 
        private void LimpiarPanel()
        {
            panelGrafica.Invalidate();
            panelGrafica.Update();
            //Las lineas del mapa cartesiano
            generaLinea(150, 0, 150, 275, false);
            generaLinea(0, 137, 300, 137, false);


        }
        
        //Al presionar el boton de linea
        private void button1_Click(object sender, EventArgs e)
        {
            //limpia la ventana

            movi1.X = movi1.Y = movi2.X = movi2.Y = 1000;
            
            LimpiarPanel();
            try
            {
                //Define las cuatro variables de los dos puntos
                int xInicial, yInicial, xFinal, yFinal;
                //Los convierte de texto a enteros
                xInicial = int.Parse(x1Linea.Text);
                yInicial = int.Parse(y1Linea.Text);
                xFinal = int.Parse(x2Linea.Text);
                yFinal = int.Parse(y2Linea.Text);
                //Llama al metodo que lo dibuja
                generaLinea(xInicial, yInicial, xFinal, yFinal, true);
            }catch(Exception eme)
            {
                MessageBox.Show("valores no validos");
            }
        }
        //Eso.. la genera... la linea
        private void generaLinea(int xl1,int yl1, int xl2, int yl2,bool col)
        {
            //Acomoda los puntos al centro del plano cartesiano(las "y" se restan porque
            //normalmente van a la inversa al graficar)
            //El IF es para saber si esta dibujando una linea, o el plano mismo
            if (col == true)
            {
                xl1 = xl1 + 150;
                yl1 = 137 - yl1;
                xl2 = xl2 + 150;
                yl2 = 137 - yl2;
            }
            //Cosa para dibujar
            Graphics g = panelGrafica.CreateGraphics();
            //DDA
            int dx= xl2 - xl1;
            int dy= yl2 - yl1;
            //Toma el mas grande
            int pasos = Math.Max(Math.Abs(dx), Math.Abs(dy)); 
            //Calcula los incrementos
            float incrementX = (float)dx / pasos; 
            float incrementY = (float)dy / pasos; 
            //A lo que se le va a ir sumando sin afectar los originales
            float x = xl1;
            float y = yl1;
            //Ciclo que hace cosas de ciclo
            for (int i = 0; i <= pasos; i++)
            {
                //Con el if cambia el color del plano cartesiano, y la linea
                if(col==true)
                    g.FillRectangle(Brushes.Purple, x, y, 1, 1); 
                else
                    g.FillRectangle(Brushes.Black, x, y, 1, 1);
                x += incrementX;
                y += incrementY;
            }
        
        }

        //Boton de la curva
        private void bCurva_Click(object sender, EventArgs e)
        {
            //Limpia el panel
            LimpiarPanel();
            //Asigna todos los valores globales
            try { 
            xI = int.Parse(xiCurva.Text);
            yI = int.Parse(yiCurva.Text);
            x1 = int.Parse(x1Curva.Text);
            y1 = int.Parse(y1Curva.Text);
            x2 = int.Parse(x2Curva.Text);
            y2 = int.Parse(y2Curva.Text);
            xF = int.Parse(xfCurva.Text);
            yF = int.Parse(yfCurva.Text);
            //Asigna donde deben estar los dos objetos que se mueven
            movi1 = new Rectangle(x1+150, 137-y1, 10, 10);
            movi2 = new Rectangle(x2+150, 137-y2, 10, 10);
                moviI = new Rectangle(xI + 150, 137 - yI, 10, 10);
                moviF = new Rectangle(xF + 150, 137 - yF, 10, 10);
                //Llama al metodo que genera la curva
                generaCurva(xI,yI,x1,y1,x2,y2,xF,yF);
            }catch(Exception eme)
            {
                MessageBox.Show("valores no validos");
            }

}
        //Pasa los valores así(de uno por uno) para no modificar los valores globales por error
        private void generaCurva(int xcI,int ycI,int xc1,int yc1,int xc2,int yc2,int xcF,int ycF)
        {
            //Actualiza el panel
            panelGrafica.Paint += panelGrafica_Paint;
            LimpiarPanel();
            //Acomoda todo al plano cartesiano
            xcI = xcI + 150;
            ycI = 137 - ycI;
            xc1 = xc1 + 150;
            yc1 = 137 - yc1;
            xc2 = xc2 + 150;
            yc2 = 137 - yc2;
            xcF = xcF + 150;
            ycF = 137 - ycF;
            //Varibles para generar la curva como el tiempo, y que tanto aumenta
            double t;
            double incremento = 0.001;
            double fx, fy;
            Graphics g = panelGrafica.CreateGraphics();
            //Cada tiempo dibuja un punto siguiendo las formaulas de Bezier
            for (t = 0; t < 1; t += incremento)
            {
                //Para la x
                fx = (Math.Pow(1 - t, 3) * xcI) + 
                    (3*t*(Math.Pow(1-t,2))*xc1) + 
                    (3*(Math.Pow(t,2))*(1-t)*xc2) + 
                    (Math.Pow(t,3)*xcF);
                //Para la y
                fy = (Math.Pow(1 - t, 3) * ycI) +
                    (3 * t * (Math.Pow(1 - t, 2)) * yc1) +
                    (3 * (Math.Pow(t, 2)) * (1 - t) * yc2) +
                    (Math.Pow(t, 3) * ycF);
                //También se podría agregar una para z si es necesario

                //Dibuja un rectangulito que compone la linea
                g.FillRectangle(Brushes.Purple,(float)fx, (float)fy, 1, 1);
            }

        }

        
        //Vuelve a pintar los rectangulos moviles
        private void panelGrafica_Paint(object sender, PaintEventArgs e)
        {

                e.Graphics.FillRectangle(Brushes.Blue, movi1);
                e.Graphics.FillRectangle(Brushes.Blue, movi2);
            e.Graphics.FillRectangle(Brushes.Blue, moviI);
            e.Graphics.FillRectangle(Brushes.Blue, moviF);



        }
        //Revisa si estas presionando sobre uno de los dos rectangulos y activa la variable
        // de que lo estan arrastrando al pobre
        private void panelGrafica_MouseDown(object sender, MouseEventArgs e)
        {
            if (movi1.Contains(e.Location))
            {
                estaArrastrando1 = true;
                posicionI1 = e.Location;
            }
            if (movi2.Contains(e.Location))
            {
                estaArrastrando2 = true;
                posicionI2 = e.Location;
            }
            if(moviI.Contains(e.Location))
            {
                estaArrastrando3 = true;
                posicionI3 = e.Location;
            }
            if (moviF.Contains(e.Location))
            {
                estaArrastrando4 = true;
                posicionI4 = e.Location;
            }
        }
        //Cuando lo suelte
        private void panelGrafica_MouseUp(object sender, MouseEventArgs e)
        {
            estaArrastrando1 = false;
            estaArrastrando2 = false;
            estaArrastrando3 = false;
            estaArrastrando4 = false;
        }
        //Checa si esta arrastrando uno u otro
        private void panelGrafica_MouseMove(object sender, MouseEventArgs e)
        {
            if (estaArrastrando1)
            {
                // Calcula a donde se movio y se lo suma a la posicion del rectangulo
                int deltaX = e.X - posicionI1.X;
                int deltaY = e.Y - posicionI1.Y;
                movi1.X += deltaX;
                movi1.Y += deltaY;

                //AAAAAAAAAAAA
                // Cambia la posicion, puede que aqui haya un error por sumar un numero muy grande o
                //algo asi
                posicionI1 = e.Location;

                //Cambia los valores en los textBox
                x1Curva.Text = "" + (movi1.X-150);
                y1Curva.Text = "" + (137-movi1.Y);
                x1 = movi1.X-150;
                y1 = 137-movi1.Y;
                // Vuelve a dibujar la linea (dibuja tambien las otras cosas)
                generaCurva(xI, yI, movi1.X-150,137- movi1.Y, x2, y2, xF, yF);
               
            }

            if (estaArrastrando2)
            {
                // Calcula a donde se movio y se lo suma a la posicion del rectangulo
                int deltaX = e.X - posicionI2.X;
                int deltaY = e.Y - posicionI2.Y;
                movi2.X += deltaX;
                movi2.Y += deltaY;

                //POR AQUI!!?
                // Cambia la posicion, puede que aqui haya un error por sumar un numero muy grande o
                //algo asi
                posicionI2 = e.Location;
                //Cambia los valores en los textBox
                x2Curva.Text = ""+(movi2.X-150);
                y2Curva.Text = "" + (137-movi2.Y);
                x2 = movi2.X - 150;
                y2 = 137 - movi2.Y;
                // Vuelve a dibujar la linea (dibuja tambien las otras cosas)
                generaCurva(xI, yI, x1, y1, movi2.X-150, 137-movi2.Y, xF, yF);

            }

            if (estaArrastrando3)
            {
                // Calcula a donde se movio y se lo suma a la posicion del rectangulo
                int deltaX = e.X - posicionI3.X;
                int deltaY = e.Y - posicionI3.Y;
                moviI.X += deltaX;
                moviI.Y += deltaY;

                //POR AQUI!!?
                // Cambia la posicion, puede que aqui haya un error por sumar un numero muy grande o
                //algo asi
                posicionI3 = e.Location;
                //Cambia los valores en los textBox
                xiCurva.Text = "" + (moviI.X - 150);
                yiCurva.Text = "" + (137 - moviI.Y);
                xI = moviI.X - 150;
                yI = 137 - moviI.Y;
                // Vuelve a dibujar la linea (dibuja tambien las otras cosas)
                generaCurva(xI, yI, x1, y1, x2, y2, xF, yF);

            }
            if (estaArrastrando4)
            {
                // Calcula a donde se movio y se lo suma a la posicion del rectangulo
                int deltaX = e.X - posicionI4.X;
                int deltaY = e.Y - posicionI4.Y;
                moviF.X += deltaX;
                moviF.Y += deltaY;

                //POR AQUI!!?
                // Cambia la posicion, puede que aqui haya un error por sumar un numero muy grande o
                //algo asi
                posicionI4 = e.Location;
                //Cambia los valores en los textBox
                xfCurva.Text = "" + (moviF.X - 150);
                yfCurva.Text = "" + (137 - moviF.Y);
                xF = moviF.X - 150;
                yF = 137 - moviF.Y;
                // Vuelve a dibujar la linea (dibuja tambien las otras cosas)
                generaCurva(xI, yI, x1, y1, x2, y2, xF, yF);

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


    }
}
