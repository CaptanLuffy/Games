using Köstebek_Avı.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Köstebek_Avı
{
    class CKostebek : CImageBase
    {
        private Rectangle _KöstebekHotSpot = new Rectangle();

        public CKostebek()
            : base(Resources.Köstebek)
        {
            _KöstebekHotSpot.X = Left + 20;
            _KöstebekHotSpot.Y = Top + 1;
            _KöstebekHotSpot.Width = 30;
            _KöstebekHotSpot.Height = 40;
        }

        public void Update(int X, int Y)
        {
            Left = X;
            Top = Y;
            _KöstebekHotSpot.X = Left + 20;
            _KöstebekHotSpot.Y = Top - 1;
        }
        public bool Hit(int X, int Y)
        {
            Rectangle c = new Rectangle(X, Y, 1, 1); // Create a cursor rect - quick way to check for hit.

            if (_KöstebekHotSpot.Contains(c))
            {
                return true;
            }
            return false;
            
        }

        internal void Update(object p1, object p2)
        {
            throw new NotImplementedException();
        }
    }
}
