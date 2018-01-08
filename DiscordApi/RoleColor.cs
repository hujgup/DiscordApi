using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Echo.Discord.Api {
	public class RoleColor : IEquatable<RoleColor>, IEquatable<uint> {
		private static readonly string _byteRegex = @"(\d{1,2}|[01]\d{2}|2[0-4]\d|25[0-5])";
		private static readonly Dictionary<Regex, Action<RoleColor, Match>> _formats = new Dictionary<Regex, Action<RoleColor, Match>> {
			{
				// rgb(r, g, b)
				new Regex(@"^\s*rgb\s*\(\s*" + _byteRegex + @"\s*,\s*" + _byteRegex + @"\s*,\s*" + _byteRegex + @"\s*\)\s*$", RegexOptions.IgnoreCase),
				(c, m) => {
					c.Red = Convert.ToByte(m.Groups[1].Value);
					c.Green = Convert.ToByte(m.Groups[2].Value);
					c.Blue = Convert.ToByte(m.Groups[4].Value);
				}
			},
			{
				// XXXXXX and #XXXXXX
				new Regex(@"^\s*#?\s*([0-9a-f]{2})\s*([0-9a-f]{2})\s*([0-9a-f]{2})\s*$", RegexOptions.IgnoreCase),
				(c, m) => {
					c.Red = Convert.ToByte(m.Groups[1].Value, 16);
					c.Green = Convert.ToByte(m.Groups[2].Value, 16);
					c.Blue = Convert.ToByte(m.Groups[3].Value, 16);
				}
			},
			{
				// #XXX
				new Regex(@"^\s*#\s*([0-9a-f])\s*([0-9a-f])\s*([0-9a-f])\s*$", RegexOptions.IgnoreCase),
				(c, m) => {
					string r = m.Groups[1].Value;
					string g = m.Groups[2].Value;
					string b = m.Groups[3].Value;
					r += r;
					g += g;
					b += b;
					c.Red = Convert.ToByte(r, 16);
					c.Green = Convert.ToByte(g, 16);
					c.Blue = Convert.ToByte(b, 16);
				}
			}
		};
		public RoleColor(byte red, byte green, byte blue) : this(0) {
			Red = red;
			Green = green;
			Blue = blue;
		}
		public RoleColor(uint rgb) {
			Rgb = rgb & 0xFFFFFF; // Ignoring alpha
		}
		public RoleColor(string color) : this(0) {
			bool found = false;
			foreach (KeyValuePair<Regex, Action<RoleColor, Match>> kvp in _formats) {
				Match m = kvp.Key.Match(color);
				if (m.Success) {
					kvp.Value(this, m);
					found = true;
					break;
				}
			}
			if (!found) {
				throw new FormatException("Invalid color string format: " + color + ".");
			}
		}
		public static void RegisterParser(Regex pattern, Action<RoleColor, Match> parser) {
			_formats.Add(pattern, parser);
		}
		public uint Rgb {
			get;
			private set;
		}
		public byte Red {
			get => GetByte(2);
			private set => SetByte(2, value);
		}
		public byte Green {
			get => GetByte(1);
			private set => SetByte(1, value);
		}
		public byte Blue {
			get => GetByte(0);
			private set => SetByte(0, value);
		}
		public bool IsDefault {
			get => Rgb == 0;
		}
		private static uint GetMask(int i) {
			i *= 8;
			return (uint)(Math.Pow(2, i + 8) - Math.Pow(2, i));
		}
		private static int GetShift(int i) {
			return 8*i;
		}
		private byte GetByte(int i) {
			return (byte)((Rgb & GetMask(i)) >> GetShift(i));
		}
		private void SetByte(int i, byte value) {
			Rgb &= (uint)(~GetMask(i) & (value << GetShift(i)));
		}
		public override string ToString() {
			return "#" + Convert.ToString(Rgb, 16);
		}
		public bool Equals(uint argb) {
			return Rgb == (argb & 0xFFFFFF); // Ignoring alpha
		}
		public bool Equals([CanBeNull] RoleColor other) {
			return !(other is null) && Rgb == other.Rgb;
		}
		public override bool Equals([CanBeNull] object obj) {
			return obj is RoleColor color && Equals(color);
		}
		public override int GetHashCode() {
			// ReSharper disable once NonReadonlyMemberInGetHashCode (only set on construction)
			return Rgb.GetHashCode();
		}
		public static implicit operator RoleColor(uint argb) {
			return new RoleColor(argb);
		}
		public static explicit operator uint([CanBeNull] RoleColor color) {
			return color is null ? 0 : color.Rgb;
		}
		public static bool operator ==([CanBeNull] RoleColor a, [CanBeNull] RoleColor b) {
			return a is null ? b is null : a.Equals(b);
		}
		public static bool operator !=([CanBeNull] RoleColor a, [CanBeNull] RoleColor b) {
			return !(a == b);
		}
	}
}
