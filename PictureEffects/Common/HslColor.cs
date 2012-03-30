using System;
using System.Windows.Media;

namespace HslColorConversion
{
    public struct HslColor
    {
        public double A;

        public double H;

        public double L;

        public double S;

        private static double ByteToPct(byte v)
        {
            double num = (double)v;
            num = num / 255;
            return num;
        }

        public static HslColor ConvertToHSLAndSaturateAndLighten(byte A, byte R, byte G, byte B, double satfactor, double lightenfactor)
        {
            HslColor l = new HslColor();
            l.A = 1;
            double r = (double)R / 255;
            double g = (double)G / 255;
            double b = (double)B / 255;
            double num1 = Math.Max(b, Math.Max(r, g));
            double num2 = Math.Min(b, Math.Min(r, g));
            if (num1 == num2)
            {
                l.H = 0;
            }
            else
            {
                if (num1 == r && g >= b)
                {
                    l.H = 60 * (g - b) / (num1 - num2);
                }
            }
            l.L = 0.5 * (num1 + num2);
            if (num1 == num2)
            {
                l.S = 0;
            }
            else
            {
                if (l.L <= 0.5)
                {
                    l.S = (num1 - num2) / 2 * l.L;
                }
                else
                {
                    if (l.L > 0.5)
                    {
                        l.S = (num1 - num2) / (2 - 2 * l.L);
                    }
                }
            }
            l.A = (double)((double)A) / 255;
            l.L = Math.Min(Math.Max(l.L + lightenfactor, 0), 1);
            l.S = Math.Min(Math.Max(l.S + satfactor, 0), 1);
            return l;
        }

        public HslColor Darken(double pct)
        {
            return this.Lighten(-pct);
        }

        public static HslColor FromArgb(byte A, byte R, byte G, byte B)
        {
            HslColor pct = HslColor.FromRgb(R, G, B);
            pct.A = HslColor.ByteToPct(A);
            return pct;
        }

        public static HslColor FromColor(Color c)
        {
            return HslColor.FromArgb(c.A, c.R, c.G, c.B);
        }

        public static HslColor FromRgb(byte R, byte G, byte B)
        {
            HslColor l = new HslColor();
            l.A = 1;
            double pct1 = HslColor.ByteToPct(R);
            double num1 = HslColor.ByteToPct(G);
            double pct2 = HslColor.ByteToPct(B);
            double num2 = Math.Max(pct2, Math.Max(pct1, num1));
            double num3 = Math.Min(pct2, Math.Min(pct1, num1));
            if (num2 == num3)
            {
                l.H = 0;
            }
            else
            {
                if (num2 == pct1 && num1 >= pct2)
                {
                    l.H = 60 * (num1 - pct2) / (num2 - num3);
                }
            }
            l.L = 0.5 * (num2 + num3);
            if (num2 == num3)
            {
                l.S = 0;
            }
            else
            {
                if (l.L <= 0.5)
                {
                    l.S = (num2 - num3) / 2 * l.L;
                }
                else
                {
                    if (l.L > 0.5)
                    {
                        l.S = (num2 - num3) / (2 - 2 * l.L);
                    }
                }
            }
            return l;
        }

        private double getComponent(double tc, double p, double q)
        {
            if (tc < 0.166666666666667)
            {
                return p + (q - p) * 6 * tc;
            }
            if (tc < 0.5)
            {
                return q;
            }
            if (tc < 0.666666666666667)
            {
                return p + (q - p) * 6 * (0.666666666666667 - tc);
            }
            return p;
        }

        public HslColor Lighten(double pct)
        {
            HslColor a = new HslColor();
            a.A = this.A;
            a.H = this.H;
            a.S = this.S;
            a.L = Math.Min(Math.Max(this.L + pct, 0), 1);
            return a;
        }

        private double norm(double d)
        {
            if (d < 0)
            {
                d = d + 1;
            }
            if (d > 1)
            {
                d = d - 1;
            }
            return d;
        }

        private static byte PctToByte(double pct)
        {
            pct = pct * 255;
            pct = pct + 0.5;
            if (pct > 255)
            {
                pct = 255;
            }
            if (pct < 0)
            {
                pct = 0;
            }
            return (byte)pct;
        }

        public HslColor Saturate(double factor)
        {
            HslColor a = new HslColor();
            a.A = this.A;
            a.H = this.H;
            a.S = Math.Min(Math.Max(this.S + factor, 0), 1);
            a.L = this.L;
            return a;
        }

        public Color ToColor()
        {
            double l = 0;
            if (this.L < 0.5)
            {
                l = this.L * (1 + this.S);
            }
            else
            {
                l = this.L + this.S - this.L * this.S;
            }
            double num1 = 2 * this.L - l;
            double h = this.H / 360;
            double component1 = this.getComponent(this.norm(h + 0.333333333333333), num1, l);
            double component2 = this.getComponent(this.norm(h), num1, l);
            double num2 = this.getComponent(this.norm(h - 0.333333333333333), num1, l);
            return Color.FromArgb(HslColor.PctToByte(this.A), HslColor.PctToByte(component1), HslColor.PctToByte(component2), HslColor.PctToByte(num2));
        }

        public Color ToColorInlined()
        {
            double l = 0;
            if (this.L < 0.5)
            {
                l = this.L * (1 + this.S);
            }
            else
            {
                l = this.L + this.S - this.L * this.S;
            }
            double component1 = this.getComponent(this.norm(this.H / 360 + 0.333333333333333), 2 * this.L - l, l);
            double num = this.getComponent(this.norm(this.H / 360), 2 * this.L - l, l);
            double component2 = this.getComponent(this.norm(this.H / 360 - 0.333333333333333), 2 * this.L - l, l);
            return Color.FromArgb(HslColor.PctToByte(this.A), HslColor.PctToByte(component1), HslColor.PctToByte(num), HslColor.PctToByte(component2));
        }
    }
}