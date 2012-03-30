using System;
using System.Windows.Media;
namespace HslColorConversion
{
	public struct HslColor
	{
		public double A;
		public double H;
		public double S;
		public double L;
		private static double ByteToPct(byte v)
		{
			double num = (double)v;
			return num / 255.0;
		}
		private static byte PctToByte(double pct)
		{
			pct *= 255.0;
			pct += 0.5;
			if (pct > 255.0)
			{
				pct = 255.0;
			}
			if (pct < 0.0)
			{
				pct = 0.0;
			}
			return (byte)pct;
		}
		public static HslColor FromColor(Color c)
		{
			return HslColor.FromArgb(c.A, c.R, c.G, c.B);
		}
		public static HslColor FromArgb(byte A, byte R, byte G, byte B)
		{
			HslColor result = HslColor.FromRgb(R, G, B);
			result.A = HslColor.ByteToPct(A);
			return result;
		}
		public static HslColor FromRgb(byte R, byte G, byte B)
		{
			HslColor result = default(HslColor);
			result.A = 1.0;
			double num = HslColor.ByteToPct(R);
			double num2 = HslColor.ByteToPct(G);
			double num3 = HslColor.ByteToPct(B);
			double num4 = Math.Max(num3, Math.Max(num, num2));
			double num5 = Math.Min(num3, Math.Min(num, num2));
			if (num4 == num5)
			{
				result.H = 0.0;
			}
			else
			{
				if (num4 == num && num2 >= num3)
				{
					result.H = 60.0 * ((num2 - num3) / (num4 - num5));
				}
				else
				{
					if (num4 == num && num2 < num3)
					{
						result.H = 60.0 * ((num2 - num3) / (num4 - num5)) + 360.0;
					}
					else
					{
						if (num4 == num2)
						{
							result.H = 60.0 * ((num3 - num) / (num4 - num5)) + 120.0;
						}
						else
						{
							if (num4 == num3)
							{
								result.H = 60.0 * ((num - num2) / (num4 - num5)) + 240.0;
							}
						}
					}
				}
			}
			result.L = 0.5 * (num4 + num5);
			if (num4 == num5)
			{
				result.S = 0.0;
			}
			else
			{
				if (result.L <= 0.5)
				{
					result.S = (num4 - num5) / (2.0 * result.L);
				}
				else
				{
					if (result.L > 0.5)
					{
						result.S = (num4 - num5) / (2.0 - 2.0 * result.L);
					}
				}
			}
			return result;
		}
		public HslColor Lighten(double pct)
		{
			HslColor result = default(HslColor);
			result.A = this.A;
			result.H = this.H;
			result.S = this.S;
			result.L = Math.Min(Math.Max(this.L + pct, 0.0), 1.0);
			return result;
		}
		public HslColor Saturate(double factor)
		{
			HslColor result = default(HslColor);
			result.A = this.A;
			result.H = this.H;
			result.S = Math.Min(Math.Max(this.S + factor, 0.0), 1.0);
			result.L = this.L;
			return result;
		}
		public static HslColor ConvertToHSLAndSaturateAndLighten(byte A, byte R, byte G, byte B, double satfactor, double lightenfactor)
		{
			HslColor result = default(HslColor);
			result.A = 1.0;
			double num = (double)R / 255.0;
			double num2 = (double)G / 255.0;
			double num3 = (double)B / 255.0;
			double num4 = Math.Max(num3, Math.Max(num, num2));
			double num5 = Math.Min(num3, Math.Min(num, num2));
			if (num4 == num5)
			{
				result.H = 0.0;
			}
			else
			{
				if (num4 == num && num2 >= num3)
				{
					result.H = 60.0 * ((num2 - num3) / (num4 - num5));
				}
				else
				{
					if (num4 == num && num2 < num3)
					{
						result.H = 60.0 * ((num2 - num3) / (num4 - num5)) + 360.0;
					}
					else
					{
						if (num4 == num2)
						{
							result.H = 60.0 * ((num3 - num) / (num4 - num5)) + 120.0;
						}
						else
						{
							if (num4 == num3)
							{
								result.H = 60.0 * ((num - num2) / (num4 - num5)) + 240.0;
							}
						}
					}
				}
			}
			result.L = 0.5 * (num4 + num5);
			if (num4 == num5)
			{
				result.S = 0.0;
			}
			else
			{
				if (result.L <= 0.5)
				{
					result.S = (num4 - num5) / (2.0 * result.L);
				}
				else
				{
					if (result.L > 0.5)
					{
						result.S = (num4 - num5) / (2.0 - 2.0 * result.L);
					}
				}
			}
			result.A = (double)A / 255.0;
			result.L = Math.Min(Math.Max(result.L + lightenfactor, 0.0), 1.0);
			result.S = Math.Min(Math.Max(result.S + satfactor, 0.0), 1.0);
			return result;
		}
		public HslColor Darken(double pct)
		{
			return this.Lighten(-pct);
		}
		private double norm(double d)
		{
			if (d < 0.0)
			{
				d += 1.0;
			}
			if (d > 1.0)
			{
				d -= 1.0;
			}
			return d;
		}
		private double getComponent(double tc, double p, double q)
		{
			if (tc < 0.16666666666666666)
			{
				return p + (q - p) * 6.0 * tc;
			}
			if (tc < 0.5)
			{
				return q;
			}
			if (tc < 0.66666666666666663)
			{
				return p + (q - p) * 6.0 * (0.66666666666666663 - tc);
			}
			return p;
		}
		public Color ToColorInlined()
		{
			double num = 0.0;
			if (this.L < 0.5)
			{
				num = this.L * (1.0 + this.S);
			}
			else
			{
				num = this.L + this.S - this.L * this.S;
			}
			double component = this.getComponent(this.norm(this.H / 360.0 + 0.33333333333333331), 2.0 * this.L - num, num);
			double component2 = this.getComponent(this.norm(this.H / 360.0), 2.0 * this.L - num, num);
			double component3 = this.getComponent(this.norm(this.H / 360.0 - 0.33333333333333331), 2.0 * this.L - num, num);
			return Color.FromArgb(HslColor.PctToByte(this.A), HslColor.PctToByte(component), HslColor.PctToByte(component2), HslColor.PctToByte(component3));
		}
		public Color ToColor()
		{
			double num = 0.0;
			if (this.L < 0.5)
			{
				num = this.L * (1.0 + this.S);
			}
			else
			{
				num = this.L + this.S - this.L * this.S;
			}
			double p = 2.0 * this.L - num;
			double num2 = this.H / 360.0;
			double component = this.getComponent(this.norm(num2 + 0.33333333333333331), p, num);
			double component2 = this.getComponent(this.norm(num2), p, num);
			double component3 = this.getComponent(this.norm(num2 - 0.33333333333333331), p, num);
			return Color.FromArgb(HslColor.PctToByte(this.A), HslColor.PctToByte(component), HslColor.PctToByte(component2), HslColor.PctToByte(component3));
		}
	}
}
