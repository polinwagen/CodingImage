using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
        bool pictlod = false;
        string filename;
        byte[] ByteArray;
        byte[] ByteArray2;
        long sizeofimage,sizeofimage2;


        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
          
        
            var openFileDialog1 = new OpenFileDialog();
            openFileDialog1.DefaultExt = "*.TIFF";
            openFileDialog1.Filter = "Файлы TIFF (*.tif)|*.tif";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {


                FileStream fstream = File.OpenRead(openFileDialog1.FileName);

                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                filename = openFileDialog1.FileName;
                ByteArray = new byte[fstream.Length];    //создали массив размеров с файл , загрузили туда байты
                fstream.Read(ByteArray, 0, ByteArray.Length);
                fstream.Dispose();
                pictlod = true;
            }
            else {
                pictlod = false;
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (pictlod == false)
            {

                MessageBox.Show("Необходимо загрузить изображение!");
                return;
            }



            string path = "";

            if (path.Length == 0)
            {
                SaveFileDialog save = new SaveFileDialog();

                save.DefaultExt = "*.POLPAL";
                save.Filter = "Файлы POLPAL (*.pol)|*.pol";

                if (save.ShowDialog() == DialogResult.OK)
                {
                    path = save.FileName; //тут просто диалоговое окно открывалось для названия файла

                }

                else
                {
                    return;
                }
            }
            


            FileStream fstream = new FileStream(path, FileMode.OpenOrCreate); //создали поток файла
            BinaryWriter compr = new BinaryWriter(fstream);



            

 
            long i = 0;
            int povt = 1;
            byte cv;

           sizeofimage = ByteArray.Length;


            while (i < sizeofimage-1) //открываем цикл ,будем все делать до тех пор пока массив не кончится
            {
               

                    povt = 1;

                if ((i + 3)> (sizeofimage - 1)) // если мы пришли к концу файла и у нас осталось
                                                     //2 байта,конечно мы их не будем сравнивать со следующими ,у нас конец массива
                {
                    if (ByteArray[i] < 0xC0)
                    {
                        compr.Write(ByteArray[i]);
                        compr.Write(ByteArray[i + 1]);         //кароч если байт меньше С0 то пишем его таким какой он есть  (неповторяющийся байт)               
                        break;
                    }

                    else
                    {


                        cv = Convert.ToByte(0xC1);
                        compr.Write(cv);
                        compr.Write(ByteArray[i]); // если байт больше С0,то пишем С1-значение байта повторителя+значение байта
                        compr.Write(ByteArray[i + 1]);
                        break;


                    }


                }

                    if (ByteArray[i] == ByteArray[i + 2] & ByteArray[i + 1] == ByteArray[i + 3]) //если байты повторяются
                    {

                        while (ByteArray[i] == ByteArray[i + 2] & ByteArray[i + 1] == ByteArray[i + 3])
                        {     //тут уже все интереснее,мы сравниваем байт со следующим байтом ,если одинаковые стакуем повторения

                            povt++;
                            i = i + 2;

                        

                            if (povt == 63) //помним ,что повторения нельзя превышать 64,настаковали 63,обнулили группу
                            {
                                cv = Convert.ToByte(povt + 0xC0);
                                compr.Write(cv);
                                compr.Write(ByteArray[i]);
                                compr.Write(ByteArray[i + 1]);
                                povt = 1;
                                i = i + 2;

                            }
                        }


                        cv = Convert.ToByte(povt + 0xC0); //тут все пишем норм ,если повторений не много (cv-счетчик)
                        compr.Write(cv);
                        compr.Write(ByteArray[i]);
                        compr.Write(ByteArray[i + 1]);
                        povt = 1;
                        i = i + 2;
                    }
                

                else //если байты одиночны
                {

                    if (ByteArray[i] < 0xC0)
                    {
                        compr.Write(ByteArray[i]);
                        compr.Write(ByteArray[i + 1]);         //кароч если байт меньше С0 то пишем его таким какой он есть  (неповторяющийся байт)               
                        i = i + 2;
                        povt = 1;
                    }

                    else
                    {


                        cv = Convert.ToByte(0xC1);
                        compr.Write(cv);
                        compr.Write(ByteArray[i]); // если байт больше С0,то пишем С1-значение байта повторителя+значение байта
                        compr.Write(ByteArray[i + 1]);
                        i = i + 2;
                        povt = 1;


                    }
                }
                
            }

            fstream.Close();
            compr.Close();


        }


        
        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var openFileDialog1 = new OpenFileDialog();
            openFileDialog1.DefaultExt = "*.POLPAL";
            openFileDialog1.Filter = "Файлы POLPAL (*.pol)|*.pol";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {


                FileStream fstream = File.OpenRead(openFileDialog1.FileName);
                filename = openFileDialog1.FileName;
                ByteArray2 = new byte[fstream.Length];    //создали массив размеров с файл , загрузили туда байты
                fstream.Read(ByteArray2, 0, ByteArray2.Length);
                fstream.Dispose();
                pictlod = true;
            }
            else
            {
                return;
            }



                string path = "";

                if (path.Length == 0)
                {
                    SaveFileDialog save = new SaveFileDialog();

                    save.DefaultExt = "*.TIFF";
                    save.Filter = "Файлы TIFF (*.tif)|*.tif";

                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        path = save.FileName; //тут просто диалоговое окно открывалось для названия файла

                    }

                    else
                    {
                        return;
                    }
                }



                FileStream fstream2 = new FileStream(path, FileMode.OpenOrCreate); //создали поток файла
                BinaryWriter compr2 = new BinaryWriter(fstream2);


                long i = 0;
                int povt = 0;

                sizeofimage2 = ByteArray2.Length;
                byte[] BufArray = new byte[2] ; //2 элемента 

                while (i < sizeofimage2 - 1) //открываем цикл ,будем все делать до тех пор пока массив не кончится
                {



                    
                        if (ByteArray2[i] < 0xC0)
                        {
                            compr2.Write(ByteArray2[i]);
                            compr2.Write(ByteArray2[i + 1]);         //кароч если байт меньше С0 то пишем его таким какой он есть  (неповторяющийся байт)               
                            i = i + 2;
                        }

                        else
                        {

                            povt = ByteArray2[i] - 0xC0;

                              BufArray[0] = ByteArray2[i + 1];
                              BufArray[1] = ByteArray2[i + 2];


                        for (int j = povt; j >= 1; j--)
                        {

                            compr2.Write(BufArray[0]);
                            compr2.Write(BufArray[1]);

                        }

                    BufArray[0] = 0;
                    BufArray[1] = 0;

                        povt = 0;
                        i = i + 3;

                        }


                }


                fstream2.Close();
                compr2.Close();



            pictureBox2.Image = Image.FromFile(path);
            
            FileStream fstream3 = File.OpenRead(path); //осторожно тут жесть
            ByteArray2 = new byte[fstream3.Length];
            sizeofimage2 = ByteArray2.Length;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
                pictlod = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            byte[] ByteSrav;
            byte[] ByteSrav2;
            long sizeofimage1;
            long sizeofimage3;

            
                var openFileDialog1 = new OpenFileDialog();
                openFileDialog1.DefaultExt = "*.TIFF";
                openFileDialog1.Filter = "Файлы TIFF (*.tif)|*.tif";

                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {


                    FileStream fstream = File.OpenRead(openFileDialog1.FileName);

                    pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                    filename = openFileDialog1.FileName;
                    ByteSrav = new byte[fstream.Length];
                    sizeofimage1 = ByteSrav.Length;
                     

                }
                else
                {
                    return;
                }
            

            var openFileDialog2 = new OpenFileDialog();
            openFileDialog2.DefaultExt = "*.TIFF";
            openFileDialog2.Filter = "Файлы TIFF (*.tif)|*.tif";

            if (openFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {


                FileStream fstream = File.OpenRead(openFileDialog2.FileName);

                pictureBox2.Image = Image.FromFile(openFileDialog2.FileName);
                filename = openFileDialog2.FileName;
                ByteSrav2 = new byte[fstream.Length];
                sizeofimage3 = ByteSrav2.Length;
            }
            else
            {
                return;
            }






            if (sizeofimage1 == sizeofimage3)
                MessageBox.Show("Размер файлов одинаков!");
            else
                MessageBox.Show("Размер файлов не совпадает!");



        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            
            if (pictureBox1.Image != null & pictureBox2.Image != null)
            {
                if (sizeofimage == sizeofimage2)
                    MessageBox.Show("Размер файлов одинаков!");
                else
                    MessageBox.Show("Размер файлов не совпадает!");
            }
            else
            {

                if (pictureBox1.Image == null & pictureBox1.Image == null)
                    MessageBox.Show("Загрузите изображения !");
                else

                if (pictureBox1.Image == null)
                    MessageBox.Show("Загрузите исходное изображение!");
                else
                     if (pictureBox2.Image == null)
                    MessageBox.Show("Загрузите изображение для декодирования!");
              
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null)
            {
                pictureBox2.Image.Dispose();
                pictureBox2.Image = null;
                
            }
        }
    }
    }

