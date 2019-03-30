using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace EncodForm
{
    static class RLE
    {
        public static int indx = 0;
        public static int ct = 0;
        public static sbyte currentSymb = 0;
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //Image image = Image.FromFile("C:/Users/tolik/source/repos/EncodTestRLE/horse.bmp");
            //System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            ///image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            // string str = "";
            //byte[] b = memoryStream.ToArray();
            /* char[] ch = { 'a', 'a', 'a', 'l', 'l', 'p', 't', 't' };
             for (int i = 0; i < ch.Length; i++)
             {
                 Console.WriteLine(Convert.ToByte(ch[i]));
             }*/
            //  sbyte[] b = { 1, 1, 1, 9,9 ,2,4,4,5,6,3,2,9,6,6,6,7,7 };
            sbyte[] b = { 1, 1, 1, 4, 4, 6, 7, 9, 4, 4, 5, 6, 5, 7 };
            currentSymb = b[0];
            List<sbyte> encbb = new List<sbyte>();
            List<sbyte> testb = new List<sbyte>();
            int[,] BIT = new int[b.Length, 8];
            //   BIT = toBit(b);
            //    Encoder(encb,BIT);
            encbb = Encoder(b);
            testb = Decoder(encbb);
            for (int i = 0; i < b.Length; i++)
            {
                Console.Write(b[i]);
            }
            Console.WriteLine();
            for (int i = 0; i < testb.Count; i++)
            {
                Console.Write(testb[i]);
            }
            Console.WriteLine();
            for (int i = 0; i < encbb.Count; i++)
            {
                Console.Write(encbb[i].ToString());
            }
            Console.WriteLine();
            BIT = toBit(encbb);
            Console.WriteLine();
            Console.ReadLine();
        }
        public static List<sbyte> Encoder(sbyte[] b)
        {
            int counter = 0;
            byte controll = 1;
            currentSymb = b[0];
            bool repeat = false;
            bool isp = false;
            int k = 1;
            bool isCount = false;
            List<sbyte> encb = new List<sbyte>();
            for (int i = 0; i < b.Length; i++)
            {
                if (i == 0)
                {
                    if (!isCount)
                    {
                        encb.Add(Convert.ToSByte(-1));
                        isCount = true;
                    }
                }
                if (b[i] == currentSymb)
                {
                    k = 1;
                    currentSymb = b[i];
                    if (encb[encb.Count - 1] == 127)
                    {
                        encb.Add(currentSymb);
                        encb.Add(Convert.ToSByte(0));



                    }
                    if (repeat == true)
                    {
                        if (encb[encb.Count - 1] != 0)
                            encb.Add(Convert.ToSByte(0));
                    }
                    if (i == b.Length - 1)
                    {
                        encb[encb.Count - 1]++;
                    }
                    encb[encb.Count - 1]++;
                    repeat = false;
                }
                else
                {
                    if (repeat == false)
                    {
                        encb[encb.Count - 1]++;
                        encb.Add(currentSymb);
                        encb.Add(Convert.ToSByte(0));
                        currentSymb = b[i];
                        repeat = true;
                    }
                    else
                    {

                        encb.Add(currentSymb);
                        encb[encb.Count - k - 1]--;
                        currentSymb = b[i];
                        if (i == b.Length - 1)
                        {
                            encb[encb.Count - k - 1]--;
                        }
                        k++;
                        repeat = true;
                    }
                }
            }
            encb.Add(currentSymb);
            return encb;



        }
        public static List<sbyte> Decoder(List<sbyte> enb)
        {
            List<sbyte> b = new List<sbyte>();
            int count = 0;
            for (int i = 0; i < enb.Count; i++)
            {
                if (enb[i] > 0)
                {
                    for (int j = 0; j < enb[i]; j++)
                    {
                        b.Add(enb[i + 1]);
                    }
                    i++;
                    count = 0;
                }
                else if (enb[i] < 0)
                {
                    for (int j = 1; j < Math.Abs(enb[i]) + 1; j++)
                    {
                        b.Add(enb[j + i]);
                    }
                    i += Math.Abs(enb[i]);
                }
            }
            return b;
        }
        public static int[,] toBit(List<sbyte> enb)
        {
            int[,] BIT = new int[enb.Count, 8];
            //получаем биты
            for (int i = 0; i < enb.Count; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    BIT[i, j] = (enb[i] >> j) & 0x01;
                }
            }
            for (int i = 0; i < enb.Count; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Console.Write(BIT[i, j]);
                }
                Console.WriteLine();
            }
            return BIT;
        }

    }
}
