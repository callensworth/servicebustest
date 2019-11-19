using System;

namespace alpha.BusinessLogic
{
    public class Number
    {
        public int Id { get; set; }

        public int IntNumber { get; set; }

        int num;

        public Number(int num)
        {
            IntNumber = num;
        }

        public int addTwoNums(int num1, int num2)
        {
            return num1 + num2;
        }
    }
}
