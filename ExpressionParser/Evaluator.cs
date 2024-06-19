using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionParser
{
    /// <summary>
    /// 逆ポーランド記法のリストを読み取り、式評価を行うクラス
    /// </summary>
    public class Evaluator
    {
        /// <summary>
        /// 逆ポーランド記法のリストを読み取り、式評価の結果を返す。
        /// </summary>
        /// <param name="rpns">逆ポーランド記法に並べたリスト</param>
        /// <returns>式評価の結果</returns>
        public static float Evaluate(List<string> rpns)
        {
            Evaluator evaluator = new Evaluator();
            return evaluator.Calc(rpns);
        }

        private int Calc(List<string> rpns)
        {
            Stack<int> stack = new Stack<int>();

            foreach (string rpn in rpns)
            { 
                if (rpn == "pi")
                {
                    stack.Push(3);
                }
                else if (int.TryParse(rpn, out int num))
                {
                    stack.Push(num);
                }
                else
                {
                    int b = stack.Pop();
                    int a = stack.Pop();

                    switch (rpn)
                    {
                        case "+":
                            stack.Push(a + b);
                            break;

                        case "-":
                            stack.Push(a - b);
                            break;

                        case "*":
                            stack.Push(a * b);
                            break;

                        case "/":
                            stack.Push((int)(a / b));
                            break;
                    }
                }
            }

            return stack.Pop();
        }
    }
}
