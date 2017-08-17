﻿using System.Globalization;
using VA = VisioAutomation;

namespace VisioAutomation.Drawing
{
    public struct ColorRGB
    {
        private readonly byte _r;
        private readonly byte _g;
        private readonly byte _b;

        public ColorRGB(byte r, byte g, byte b)
        {
            _r = r;
            _g = g;
            _b = b;
        }

        public ColorRGB(short r, short g, short b) :
            this( (byte)r, (byte)g, (byte) b)
        {
        }

        public ColorRGB(int rgb)
        {
            GetRGBBytes((uint) rgb, out _r, out _g, out _b);
        }

        public ColorRGB(uint rgb)
        {
            GetRGBBytes(rgb, out _r, out _g, out _b);
        }


        public ColorRGB(System.Drawing.Color color)
        {
            this._r = color.R;
            this._g = color.G;
            this._b = color.B;
        }

        public byte R
        {
            get { return _r; }
        }

        public byte G
        {
            get { return _g; }
        }

        public byte B
        {
            get { return _b; }
        }

        public override string ToString()
        {
            var s = string.Format(System.Globalization.CultureInfo.InvariantCulture, "RGB({0},{1},{2})", this._r, this._g, this._b);
            return s;
        }

        /// <summary>
        /// a convenient explicit cast to System.Drawing.Color 
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static explicit operator System.Drawing.Color(VA.Drawing.ColorRGB color)
        {
            return System.Drawing.Color.FromArgb(color._r, color._g, color._b);
        }

        /// <summary>
        /// a convenient explicit cast to an int
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static explicit operator int(VA.Drawing.ColorRGB color)
        {
            return color.ToRGB();
        }

        /// <summary>
        /// a convenient explicit cast an int to a ColorRGB24Bit
        /// </summary>
        /// <param name="rgbint"></param>
        /// <returns></returns>
        public static explicit operator ColorRGB(int rgbint)
        {
            return new ColorRGB(rgbint);
        }

        public string ToWebColorString()
        {
            return ToWebColorString(this._r, this._g, this._b);
        }

        public override bool Equals(object other)
        {
            return other is ColorRGB && Equals((ColorRGB) other);
        }

        public static bool operator ==(ColorRGB lhs, ColorRGB rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ColorRGB lhs, ColorRGB rhs)
        {
            return !lhs.Equals(rhs);
        }

        private bool Equals(ColorRGB other)
        {
            return (this._r == other._r && this._g == other._g && this._b == other._b);
        }

        public override int GetHashCode()
        {
            return ToRGB();
        }

        /// <summary>
        /// Returns an int containing RGB values.
        /// </summary>
        /// <returns></returns>
        public int ToRGB()
        {
            return (this._r << 16) | (this._g << 8) | (this._b);
        }

        /// <summary>
        /// Parses a web color string of form "#ffffff"
        /// </summary>
        /// <param name="webcolor"></param>
        /// <returns></returns>
        public static ColorRGB ParseWebColor(string webcolor)
        {
            var c = TryParseWebColor(webcolor);
            if (!c.HasValue)
            {
                string s = string.Format("Failed to parse color string \"{0}\"", webcolor);
                throw new VA.AutomationException(s);
            }

            return c.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        /// Sample usage:
        ///
        /// System.Drawing.Color c;
        /// bool result = TryParseRGBWebColorString("#ffffff", ref c);
        /// if (result)
        /// {
        ///    //it was correctly parsed
        /// }
        /// else
        /// {
        ///    //it was not correctly parsed
        /// }
        ///
        /// </example>
        /// <param name="webcolor"></param>
        ///<returns></returns>
        public static ColorRGB? TryParseWebColor(string webcolor)
        {
            // fail if string is null
            if (webcolor == null)
            {
                return null;
            }

            // fail if string is empty
            if (webcolor.Length < 1)
            {
                return null;
            }


            // clean any leading or trailing whitespace
            webcolor = webcolor.Trim();

            // fail if string is empty
            if (webcolor.Length < 1)
            {
                return null;
            }

            // strip leading # if it is there
            while (webcolor.StartsWith("#"))
            {
                webcolor = webcolor.Substring(1);
            }

            // clean any leading or trailing whitespace
            webcolor = webcolor.Trim();

            // fail if string is empty
            if (webcolor.Length < 1)
            {
                return null;
            }

            // fail if string doesn't have exactly 6 digits
            if (webcolor.Length != 6)
            {
                return null;
            }

            int current_color;
            bool result = System.Int32.TryParse(webcolor, System.Globalization.NumberStyles.HexNumber, null, out current_color);

            if (!result)
            {
                // fail if parsing didn't work
                return null;
            }

            // at this point parsing worked

            // the integer value is converted directly to an rgb value

            var the_color = new ColorRGB(current_color);
            return the_color;
        }
        
        private static void GetRGBBytes(uint rgb, out byte r, out byte g, out byte b)
        {
            r = (byte)((rgb & 0x00ff0000) >> 16);
            g = (byte)((rgb & 0x0000ff00) >> 8);
            b = (byte)((rgb & 0x000000ff) >> 0);
        }

        private static string ToWebColorString(byte r, byte g, byte b)
        {
            const string format_string = "#{0:x2}{1:x2}{2:x2}";
            CultureInfo invariant_culture = System.Globalization.CultureInfo.InvariantCulture;
            string color_string = string.Format(invariant_culture, format_string, r, g, b);
            return color_string;
        }

        public string ToFormula()
        {
            string formula = System.String.Format("RGB({0},{1},{2})", this.R, this.G, this.B);
            return formula;
        }        
    }
}