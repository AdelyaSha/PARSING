using System.Security;

namespace NewVersionOfParsing
{
    public partial class Form1 : Form
    {
        Parsing parse;
        string preparsed_expression;
        List<string> result;
        ExpressionTreeNode etn;
        List<ExpressionTreeNode> Tree;
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            preparsed_expression = textBox1.Text;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            parse = new Parsing(preparsed_expression);

            result = parse.StartParse();

            foreach (var item in result)
            {
                listBox1.Items.Add(item);
            }

            Tree = new List<ExpressionTreeNode>(result.Count);
            for (int i = result.Count - 1; i > 0; i--)
            {
                etn = new ExpressionTreeNode(result[i]);

                if (result[i].Any(item => item >= 48 && item <= 57) || result[i] == "x")
                {
                    Tree.Add(etn);
                }
                else
                {
                    if (i - 4 >= 0 && Parsing.operators.Contains(result[i]) && Parsing.operators.Contains(result[i - 1]))
                    {
                        etn.Insert(result[i - 1], result[i - 4]);
                        Tree.Add(etn);
                    }
                    if (i - 1 >= 0 && Parsing.functions.Contains(result[i]))
                    {
                        etn.Insert(result[i - 1]);
                        Tree.Add(etn);
                    }
                    else if (i - 1 >= 0 && i - 2 >= 0 && !Parsing.operators.Contains(result[i - 1]))
                    {
                        etn.Insert(result[i - 1], result[i - 2]);
                        Tree.Add(etn);

                    }
                    else if (i - 1 >= 0 && i - 2 < 0)
                    {
                        etn.Insert(result[i - 1]);
                        Tree.Add(etn);

                    }
                }

            }

            double value = 2;

            for (int i = Tree.Count - 1; i > 0; i--)
            {
                if (Parsing.operators.Contains(Tree[i].string_value))
                {
                    double temp = ExpressionTreeNode.CountValue(Tree[i], value);

                    value = temp;
                }
                
            }
            //double final_result = ExpressionTreeNode.CountValue(elem, 1);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }
    }
}