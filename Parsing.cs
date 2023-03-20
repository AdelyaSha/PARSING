using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace NewVersionOfParsing
{
    public class Parsing
    {
        private string preparsed_expression = "";
        private List<string> parsed_expression = new List<string>();
        private Stack<string> preparsed_expression_stack = new Stack<string>();
        public static readonly List<string> functions = new List<string> { "sin", "cos", "tan", "cot", "ln"};
        public static readonly List<string> operators = new List<string> { "+", "-", "*", "/", "^" };
        public Parsing(string preparsed_expression)
        {
            this.preparsed_expression = preparsed_expression;
        }
        public List<string> StartParse()
        {
            bool isNumber = false;
            double num;
            string operand = "";
            foreach (var item in preparsed_expression)
            {
                isNumber = double.TryParse(item.ToString(), out num);
                if (preparsed_expression_stack.Count != 0)
                {
                    if (item >= 97 && item <= 116)
                    {
                        operand += item;
                    }
                    else if (operators.Contains(item.ToString()) && operators.IndexOf(item.ToString()) > operators.IndexOf(preparsed_expression_stack.Peek()))
                    {
                        preparsed_expression_stack.Push(item.ToString());

                    }
                    else if (operators.Contains(item.ToString()) && operators.IndexOf(item.ToString()) < operators.IndexOf(preparsed_expression_stack.Peek()))
                    {
                        if (functions.Contains(operand))
                        {
                            while (operators.Contains(preparsed_expression_stack.Peek()))
                            {
                                parsed_expression.Add(preparsed_expression_stack.Pop());
                            }
                            preparsed_expression_stack.Push(operand);
                            operand = "";
                        }
                        else if (!functions.Contains(operand))
                        {
                            if (preparsed_expression_stack.Count > 0)
                            {
                                while (operators.IndexOf(item.ToString()) < operators.IndexOf(preparsed_expression_stack.Peek()))
                                {
                                    parsed_expression.Add(preparsed_expression_stack.Pop());
                                }

                                preparsed_expression_stack.Push(item.ToString());
                            }
                            
                        }
                    }
                    else if (item.ToString() == "(" && functions.Contains(operand))
                    {
                        preparsed_expression_stack.Push(operand);
                        operand = "";
                        preparsed_expression_stack.Push(item.ToString());

                    }
                    else if (item.ToString() == "(" && !functions.Contains(operand))
                    {
                        preparsed_expression_stack.Push(item.ToString());
                    }

                    else if (item.ToString() == ")" && !preparsed_expression_stack.Contains("sin") && 
                             !preparsed_expression_stack.Contains("cos") && !preparsed_expression_stack.Contains("tan")
                             && !preparsed_expression_stack.Contains("cot") && !preparsed_expression_stack.Contains("ln"))
                    {
                        while (preparsed_expression_stack.Count > 0 && preparsed_expression_stack.Peek() != "(")
                        {
                            parsed_expression.Add(preparsed_expression_stack.Pop());

                        }
                        if (preparsed_expression_stack.Count > 0)
                        {
                            preparsed_expression_stack.Pop();
                        }
                        
                    }
                    else if (item.ToString() == ")" && (preparsed_expression_stack.Contains("sin") ||
                             preparsed_expression_stack.Contains("cos") || preparsed_expression_stack.Contains("tan")
                             || preparsed_expression_stack.Contains("cot") || preparsed_expression_stack.Contains("ln")))
                    {
                        while (preparsed_expression_stack.Peek() != "sin" && preparsed_expression_stack.Peek() != "cos"
                               && preparsed_expression_stack.Peek() != "tan" && preparsed_expression_stack.Peek() != "cot"
                               && preparsed_expression_stack.Peek() != "ln")
                        {
                            if (preparsed_expression_stack.Peek() != "(")
                            {
                                parsed_expression.Add(preparsed_expression_stack.Pop());
                            }    
                            else
                            {
                                preparsed_expression_stack.Pop();
                            }

                        }
                        if (isNumber || item == 'x')
                        {
                            parsed_expression.Add(item.ToString());
                            parsed_expression.Add(preparsed_expression_stack.Pop());
                        }
                        else
                            parsed_expression.Add(preparsed_expression_stack.Pop());
                    }
                    else if (isNumber || item == 'x')
                    {
                        parsed_expression.Add(item.ToString());
                    }
                }
                else if (preparsed_expression_stack.Count == 0)
                {
                    if (item >= 97 && item <= 116)
                    {
                        operand += item;
                    }
                    if (operators.Contains(item.ToString()) || functions.Contains(operand))
                    {
                        if (functions.Contains(operand))
                        {
                            preparsed_expression_stack.Push(operand);
                            operand = "";
                        }
                        else
                            preparsed_expression_stack.Push(item.ToString());
                       
                    }
                    else if (isNumber || item == 'x')
                    {
                        parsed_expression.Add(item.ToString());
                    }
                    else if (item.ToString() == "(")
                    {
                        preparsed_expression_stack.Push(item.ToString());

                    }
                }

            }
            while (preparsed_expression_stack.Count() > 0) 
            {
                parsed_expression.Add(preparsed_expression_stack.Pop());
            }
            return parsed_expression;
        }
    }
}
