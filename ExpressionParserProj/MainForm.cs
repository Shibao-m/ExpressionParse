using ExpressionParser;

namespace ExpressionParserProj
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtExpression.Text))
            {
                try
                {
                    List<string> rpns = ExpressionParser.ExpressionParser.GetRPN(txtExpression.Text);
                    float value = Evaluator.Evaluate(rpns);

                    txtRPN.Text = String.Join(" ", rpns);
                    txtValue.Text = value.ToString();
                }
                catch (SyntaxException)
                {
                    MessageBox.Show("構文エラーです。");
                }
            }
        }
    }
}
