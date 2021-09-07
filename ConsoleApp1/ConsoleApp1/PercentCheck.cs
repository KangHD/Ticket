using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class PercentCheck
    {
        
        
        public static double PercentageCheck(int checkNum) {
            double SuccessCnt = 0;
            double SuccessNum;
            int num;
            Random ran = new Random();
            

            for(int i=0; i<100000; i++)
            {
                num = ran.Next(1, 100);
                if (num <= checkNum)
                {
                    SuccessCnt++;
                }
            }
            SuccessNum = SuccessCnt/1000;

            return SuccessNum;
        }
    }
}
