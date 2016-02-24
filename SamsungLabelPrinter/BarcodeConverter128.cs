using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamsungLabelPrinter
{
    class BarcodeConverter128
    {
        public static string StringToBarcode(string value)
        {
            bool flag1 = true;
            bool flag2 = true;
            string str = string.Empty;
            if (value.Length > 0)
            {
                for (int startIndex = 0; startIndex < value.Length; ++startIndex)
                {
                    int num = (int)char.Parse(value.Substring(startIndex, 1));
                    if (num < 32 || num > 126)
                    {
                        flag2 = false;
                        break;
                    }
                }
                if (flag2)
                {
                    int num1 = 0;
                    while (num1 < value.Length)
                    {
                        if (flag1)
                        {
                            int MinCharPos = num1 == 0 || num1 + 4 == value.Length ? 4 : 6;
                            if (BarcodeConverter128.IsNumber(value, num1, MinCharPos) < 0)
                            {
                                str = num1 != 0 ? str + 'Ç'.ToString() : 'Í'.ToString();
                                flag1 = false;
                            }
                            else if (num1 == 0)
                                str = 'Ì'.ToString();
                        }
                        if (!flag1)
                        {
                            int MinCharPos = 2;
                            if (BarcodeConverter128.IsNumber(value, num1, MinCharPos) < 0)
                            {
                                int num2 = int.Parse(value.Substring(num1, 2));
                                int num3 = num2 < 95 ? num2 + 32 : num2 + 100;
                                str += ((char)num3).ToString();
                                num1 += 2;
                            }
                            else
                            {
                                str += 'È'.ToString();
                                flag1 = true;
                            }
                        }
                        if (flag1)
                        {
                            str += value.Substring(num1, 1);
                            ++num1;
                        }
                    }
                    int num4 = 0;
                    for (int startIndex = 0; startIndex < str.Length; ++startIndex)
                    {
                        int num2 = (int)char.Parse(str.Substring(startIndex, 1));
                        int num3 = num2 < (int)sbyte.MaxValue ? num2 - 32 : num2 - 100;
                        num4 = startIndex != 0 ? (num4 + startIndex * num3) % 103 : num3;
                    }
                    int num5 = num4 < 95 ? num4 + 32 : num4 + 100;
                    str = str + ((char)num5).ToString() + 'Î'.ToString();
                }
            }
            return str;
        }

        private static int IsNumber(string InputValue, int CharPos, int MinCharPos)
        {
            --MinCharPos;
            if (CharPos + MinCharPos < InputValue.Length)
            {
                while (MinCharPos >= 0 && ((int)char.Parse(InputValue.Substring(CharPos + MinCharPos, 1)) >= 48 && (int)char.Parse(InputValue.Substring(CharPos + MinCharPos, 1)) <= 57))
                    --MinCharPos;
            }
            return MinCharPos;
        }
    }
}
