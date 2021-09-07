using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string[] FirstOption = new string[10];
            string[] SecondOption = new string[10];
            string[] Debuff = new string[10];
            int FirstChoice = 0;
            int SecondChoice = 0;
            int DebuffChoice = 0;
            int Percent = 75;
            int FScnt = 0;
            int SScnt = 0;
            int DScnt = 0;
            int choice;
            for(int i=0; i<10; i++)
            {
                FirstOption[i] = "◇";
                SecondOption[i] = "◇";
                Debuff[i] = "◇";
            }
            while (true)
            {
                for(int i =0; i<10; i++)
                {
                    Console.Write(FirstOption[i]);
                }
                Console.WriteLine();
                for (int i = 0; i < 10; i++)
                {
                    Console.Write(SecondOption[i]);
                }
                Console.WriteLine();
                for (int i = 0; i < 10; i++)
                {
                    Console.Write(Debuff[i]);
                }
                if (FirstChoice + SecondChoice + DebuffChoice == 30)
                {
                    Console.WriteLine();
                    Console.WriteLine("{0}{1}{2}돌이에요~", FScnt, SScnt, DScnt);
                }
                Console.WriteLine();
                Console.Write("선택하세요(1,2,3입력 새돌(0) 확률확인(5)/현재 확률{0}% -> ", Percent);
                string str = Console.ReadLine();
                if(str.Trim() == "")
                {
                    Console.WriteLine("공백은 입력할 수 없습니다.");
                    continue;
                }
                else
                {
                    choice = int.Parse(str);
                }
                

                

                Console.WriteLine();
               if(choice == 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        FirstOption[i] = "◇";
                        SecondOption[i] = "◇";
                        Debuff[i] = "◇";
                    }
                    FirstChoice = 0;
                    SecondChoice = 0;
                    DebuffChoice = 0;
                    Percent = 75;
                    FScnt = 0;
                    SScnt = 0;
                    DScnt = 0;
                    Console.WriteLine("새시작입니다.");
                    continue;
                }
                
                //1번옵션
                if (choice == 1)
                {
                    if(FirstChoice == 10)
                    {
                        Console.WriteLine("횟수소진(1번외 다른옵션을 선택하세요.)");
                        continue;
                    }
                    FirstOption[FirstChoice] = Percentage.Percent(Percent);
                    if (FirstOption[FirstChoice] == "●")
                    {
                        Console.WriteLine("성공");
                        FScnt++;
                        if (Percent == 25)
                        {

                        }
                        else{
                            Percent = Percent - 10;
                        } 
                    }else if(FirstOption[FirstChoice] == "○")
                    {
                        Console.WriteLine("실패");
                        if (Percent == 75)
                        {

                        }
                        else
                        {
                            Percent = Percent + 10;
                        }
                    }
                    FirstChoice++;
                }
                //2번옵션
                else if(choice == 2)
                {
                    if (SecondChoice == 10)
                    {
                        Console.WriteLine("횟수소진(2번외 다른옵션을 선택하세요.)");
                        continue;
                    }
                    SecondOption[SecondChoice] = Percentage.Percent(Percent);
                    if (SecondOption[SecondChoice] == "●")
                    {
                        Console.WriteLine("성공");
                        SScnt++;
                        if (Percent == 25)
                        {

                        }
                        else
                        {
                            Percent = Percent - 10;
                        }
                    }
                    else if (SecondOption[SecondChoice] == "○")
                    {
                        Console.WriteLine("실패");
                        if (Percent == 75)
                        {

                        }
                        else
                        {
                            Percent = Percent + 10;
                        }
                    }
                    SecondChoice++;
                }
                //디버프옵션
                else if(choice == 3)
                {
                    if (DebuffChoice == 10)
                    {
                        Console.WriteLine("횟수소진(3번외 다른옵션을 선택하세요.)");
                        continue;
                    }
                    Debuff[DebuffChoice] = Percentage.Percent(Percent);
                    if (Debuff[DebuffChoice] == "●")
                    {
                        Console.WriteLine("성공");
                        DScnt++;
                        if (Percent == 25)
                        {

                        }
                        else
                        {
                            Percent = Percent - 10;
                        }
                    }
                    else if (Debuff[DebuffChoice] == "○")
                    {
                        Console.WriteLine("실패");
                        if (Percent == 75)
                        {

                        }
                        else
                        {
                            Percent = Percent + 10;
                        }
                    }
                    DebuffChoice++;
                }else if(choice == 5)
                {
                    Console.WriteLine("확률은 {0:0.00}%입니다.", PercentCheck.PercentageCheck(Percent));
                }
                else
                {
                    Console.WriteLine("다시입력해주세요.");
                }
                Console.WriteLine("현재 {0} {1} {2}돌", FScnt, SScnt, DScnt);

            }


        }
    }
}
