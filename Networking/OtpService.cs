using System;
using System.Linq;
using System.Text;

namespace SharpKnP321.Networking
{
    public enum OtpMode
    {
        Digits,     // Тільки цифри
        Letters,    // Тільки літери
        Mixed       // Змішаний (без схожих символів)
    }

    internal class OtpService
    {
        private static readonly Random _random = new();

        public static string Generate(int length, OtpMode mode)
        {
            if (length <= 0) throw new ArgumentException("Довжина має бути більше 0");

            string chars = mode switch
            {
                OtpMode.Digits => "0123456789",
                OtpMode.Letters => "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                // У змішаному режимі вилучаємо: O, 0, I, 1, l (схожі нариси)
                OtpMode.Mixed => "ABCDEFGHJKLMNPQRSTUVWXYZ23456789", 
                _ => throw new ArgumentException("Невідомий режим")
            };

            StringBuilder otp = new(length);
            for (int i = 0; i < length; i++)
            {
                otp.Append(chars[_random.Next(chars.Length)]);
            }

            return otp.ToString();
        }
    }
}