using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rs232
{
    public partial class Form1 : Form
    {
        string theText;
        List<string> censoredWords = new List<string>();
        string word;
        string binaryText;
        string encodedText;
        String[] wordcen;


        public Form1()
        {
            InitializeComponent();
            readFile();
        }


        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            theText = richTextBox1.Text;
        }


        public void readFile()
        {
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("black_list.txt"))
                {
                    // Read the stream to a string, and write the string to the console.
                    char delimiter = '\n';
                    string line = sr.ReadToEnd();
                     wordcen = line.Split(delimiter);
                  //  wordCen = line.Split(delimiter);
                   // censoredWords.Add(wordCen);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

        }
    


        public string  censor(string word)
        {
            string returnedWord=word;
               foreach (string wordcens in wordcen)
                {
                    Console.WriteLine(wordcens);
                    bool compare = wordcens.Contains(word);
                    if (compare)
                    {
                        Console.WriteLine("cenzura");
                        returnedWord = new string('*', word.Length);
                }
            }
            return returnedWord+" ";

        }


        public void ascii2bin(string text)
        {
            Char delimiter = ' ';
            String[] substrings = text.Split(delimiter);
            text = "";
            foreach (string substring in substrings)
            {
                text+=censor(substring);
            }
            richTextBox1.Text = text;

            foreach (char c in text) {                          // dla każdego znaku
                binaryText = binaryText + '0';                  //bit startu
                word=(Convert.ToString(c, 2).PadLeft(8, '0'));  //ascii2bin
                char[] charArray = word.ToCharArray();          // bity odwrócona kolejność
                Array.Reverse(charArray);
                string s = new string(charArray);
                    binaryText += s;                            //
                    binaryText =binaryText+ '1';    //bity stopu
                binaryText = binaryText + '1';       //tekst wyjściowy

            }

        }


        public void receive(string binText)
        {
            string word2;
            encodedText="";

            int length = (binaryText.Length/11);           //ilość znaków
            for (int i=0; i<length; i++){                   
                word2 = binaryText.Substring((11*i)+1, 8);      //znak z pominięciem bitu startu i stopu
                char[] charArray = word2.ToCharArray();         
                Array.Reverse(charArray);
                string s = new string(charArray);
                var number = Convert.ToInt32(s, 2);        //konwersja do ASCII
                encodedText += (char)number;


            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            ascii2bin(theText);
            richTextBox2.Text = binaryText;
            receive(binaryText);              
            richTextBox3.Text =encodedText;
            binaryText = "";
            encodedText = "";

        }


        

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}
