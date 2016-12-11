//#define My_Debug

using Köstebek_Avı.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Köstebek_Avı
{
    public partial class KöstebekAvı : Form
    {
        const int FrameNum = 8;
        const int SplatNum = 3;
        bool splat = false;

        int _gameFrame = 0;
        int _splatTime = 0;

        int _vurus = 0;
        int _iska = 0;
        int _toplamatis = 0;
        double _ortalamisabet = 0;

#if My_Debug
        int _cursX = 0;
        int _cursY = 0;
#endif
        CKostebek _Köstebek;
        CSplat _Splat;
        CSign _Sign;
        CScoreFrame _ScoreFrame;

        Random rnd = new Random();        
        public KöstebekAvı()
        {
            InitializeComponent();

            //Create scope site
            Bitmap b = new Bitmap(Resources.Site);
            this.Cursor = CustomCursor.CreateCursor(b, b.Height / 2, b.Width / 2);

            _ScoreFrame = new CScoreFrame() { Left = 10, Top = 10 };
            _Sign = new CSign() { Left = 340, Top = 10 };
            _Köstebek = new CKostebek() { Left = 10, Top = 200 };
            _Splat = new CSplat();
        }

        private void timerGameLoop_Tick(object sender, EventArgs e)
        {
            if (_gameFrame >= FrameNum)
            {
                UpdateMole();
                _gameFrame = 0;
            }

            if(splat)
            {
                if(_splatTime >= SplatNum)
                {
                    splat = false;
                    _splatTime = 0;
                    UpdateMole();
                }
                _splatTime++;
            }
            _gameFrame++;
                  
            this.Refresh();
        }

        private void UpdateMole()
        {
            _Köstebek.Update(
                rnd.Next(Resources.Köstebek.Width, this.Width - Resources.Köstebek.Width),
                rnd.Next(this.Height / 2, this.Height - Resources.Köstebek.Height * 2)
                );
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics dc = e.Graphics;

            _Sign.DrawImage(dc);
            _ScoreFrame.DrawImage(dc);

            if (splat == true)
            {
                _Splat.DrawImage(dc);
            }
            else
            {
                _Köstebek.DrawImage(dc);
            }

#if My_Debug
            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.EndEllipsis;
            Font _font = new System.Drawing.Font("Stencil", 12, FontStyle.Regular);
            TextRenderer.DrawText(dc, "X=" + _cursX.ToString() + ":" + _cursY.ToString(), _font,
                new Rectangle(0, 0, 120, 20), SystemColors.ControlText, flags);
#endif
            // Put scores on the screen
            TextFormatFlags flags = TextFormatFlags.Left;
            Font _font = new System.Drawing.Font("Stencil", 12, FontStyle.Regular);
            TextRenderer.DrawText(e.Graphics, "Hits: "  +_vurus.ToString(), _font, new Rectangle(30, 32, 120, 20), SystemColors.ControlText, flags);
            TextRenderer.DrawText(e.Graphics, "Misses: "  +_iska.ToString(), _font, new Rectangle(30, 52, 120, 20), SystemColors.ControlText, flags);
            TextRenderer.DrawText(e.Graphics, "Shots: "  +_toplamatis.ToString(), _font, new Rectangle(30, 72, 120, 20), SystemColors.ControlText, flags);
            TextRenderer.DrawText(e.Graphics, "AVG: "  +_ortalamisabet.ToString("F0") + "%", _font, new Rectangle(30, 92, 120, 20), SystemColors.ControlText, flags);

            base.OnPaint(e);
        }

        private void KöstebekAvı_MouseMove(object sender, MouseEventArgs e)
        {
#if My_Debug
            _cursX = e.X;
            _cursY = e.Y;
#endif
            this.Refresh();
        }

        private void KöstebekAvı_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.X > 416 && e.X < 544 && e.Y > 92 && e.Y <121) //Start Hot Spot
            {
                timerGameLoop.Start();
            }
            else if (e.X > 416 && e.X < 544 && e.Y > 124 && e.Y < 150) //Stop Hot Spot
            {
                timerGameLoop.Stop();
            }
            else if (e.X > 416 && e.X < 544 && e.Y > 156 && e.Y < 186) //Reset Hot Spot
            {
                timerGameLoop.Stop();
                _vurus = 0;
                _iska = 0;
                _toplamatis = 0;
                _ortalamisabet = 0;
            }
            else if (e.X > 416 && e.X < 544 && e.Y > 190 && e.Y < 218) //Quit Hot Spot
            {
                timerGameLoop.Stop();
                Application.Exit();
            }
            else
            {
                if (_Köstebek.Hit(e.X, e.Y))
                {
                    splat = true;
                    _Splat.Left = _Köstebek.Left - Resources.Splat.Width / 3;
                    _Splat.Top = _Köstebek.Top - Resources.Splat.Height / 3;

                    _vurus++;
                }
                else
                {
                    _iska++;
                }                
                _toplamatis = _vurus + _iska;
                _ortalamisabet = (double)_vurus / (double)_toplamatis * 100.0;
            }
            FireGun();
        }

        private void FireGun()
        {
            // Fire off the shotgun
            SoundPlayer simpleSound = new SoundPlayer(Resources.Shotgun);

            simpleSound.Play();
        }
    }
}
