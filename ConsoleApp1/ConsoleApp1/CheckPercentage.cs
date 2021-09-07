using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class CheckPercentage
    {
        static int num;
        static int cntF;
        static int cntS;
        public static int Percent(int checkNum)
        {

            Random ran = new Random();
            num = ran.Next(1, 100);

            if (checkNum == 75)
            {
                if (num <= 75)
                {
                    return "●";
                }
                else
                {
                    return "○";
                }
            }
            else if (checkNum == 65)
            {
                if (num <= 65)
                {
                    return "●";
                }
                else
                {
                    return "○";
                }
            }
            else if (checkNum == 55)
            {
                if (num <= 55)
                {
                    return "●";
                }
                else
                {
                    return "○";
                }
            }
            else if (checkNum == 45)
            {
                num = ran.Next(1, 100);
                if (num <= 45)
                {
                    return "●";
                }
                else
                {
                    return "○";
                }
            }
            else if (checkNum == 35)
            {
                num = ran.Next(1, 100);
                if (num <= 35)
                {
                    return "●";
                }
                else
                {
                    return "○";
                }
            }
            else if (checkNum == 25)
            {
                num = ran.Next(1, 100);
                if (num <= 25)
                {
                    return "●";
                }
                else
                {
                    return "○";
                }
            }
            else
            {
                return "";
            }
        }
    }
}
