using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemoryGame
{
    //Create global variables
    public partial class Form1 : Form
    {
        bool allowClick = false;
        PictureBox firstGuess;
        Random rnd = new Random();
        Timer clickTimer = new Timer();
        int time = 100; //countdown timer
        Timer timer = new Timer { Interval = 1000 }; //1000ms = 1 sec
        public Form1()
        {
            InitializeComponent();

        }

        //Creating an Array with the pictureBoxes
        private PictureBox[] pictureBoxes
        {
            get { return Controls.OfType<PictureBox>().ToArray(); }
        }

        //addig images to the pictureBoxes
        private static IEnumerable<Image> images
        {
            get
            {
                return new Image[]
                {
                    Properties.Resources._1,
                    Properties.Resources._2,
                    Properties.Resources._3,
                    Properties.Resources._4,
                    Properties.Resources._5,
                    Properties.Resources._6,
                    Properties.Resources._7,
                    Properties.Resources._8,
                    Properties.Resources._9,
                    Properties.Resources._10,
                };
            }
        }

        //Flipping images down to show a question mark
        private void hideImages()
        {
            foreach (var pic in pictureBoxes)
            {
                pic.Image = Properties.Resources._11;
            }
        }

        //Check empty pictureboxes
        private PictureBox getFreeSlot()
        {
            int num;

            do
            {
                num = rnd.Next(0, pictureBoxes.Count());
            }
            while (pictureBoxes[num].Tag != null);
            return pictureBoxes[num];
        }

        //Find pairs of images
        private void setRndImgs()
        {
            foreach (var image in images)
            {
                getFreeSlot().Tag = image;
                getFreeSlot().Tag = image;
            }
        }

        //For resetting the pictureBoxes
        private void ResetImages()
        {
            foreach (var pic in pictureBoxes)
            {
                pic.Tag = null;
                pic.Visible = true;
            }

            hideImages();
            setRndImgs();
            time = 100;
            timer.Start();
        }

        //To start and display the timer
        private void startGameTimer()
        {
            timer.Start();
            timer.Tick += delegate
            {
                time--;
                if (time < 0)
                {
                    timer.Stop();
                    MessageBox.Show("Lejárt az idő!");
                    ResetImages();
                }
                var ssTime = TimeSpan.FromSeconds(time);

                timerLBL.Text = " " + time.ToString();
            };
        }

        //hide images and stop timer
        private void clickTimerTick(object sender, EventArgs e)
        {
            hideImages();

            allowClick = true;
            clickTimer.Stop();

        }

        //Image clicking settings
        private void clickImage(object sender, EventArgs e)
        {
            if (!allowClick) return;

            var pic = (PictureBox)sender;

            //Check if we are able to make our first guess
            if (firstGuess == null)
            {
                firstGuess = pic;
                pic.Image = (Image)pic.Tag;
                return;
            }

            //If the image was found set the tag
            pic.Image = (Image)pic.Tag;

            if(pic.Image == firstGuess.Image && pic != firstGuess)

            {
                pic.Visible = firstGuess.Visible = false;
                {
                    firstGuess = pic;
                }
                hideImages();
            }
            else
            {
                allowClick = false;
                clickTimer.Start();
            }

            firstGuess = null;

            //Check if  there are any visible picturebox
            if (pictureBoxes.Any(p => p.Visible)) return;

            //If all matched you won
            MessageBox.Show("Gratulálok nyertél!");
            ResetImages();
        }

        //stargameButton
        private void starGame(object sender, EventArgs e)
        {
            allowClick = true;
            setRndImgs();
            hideImages();
            startGameTimer();
            clickTimer.Interval = 1000;
            clickTimer.Tick += clickTimerTick;
            startBTN.Enabled = false;

        }
    }
}
