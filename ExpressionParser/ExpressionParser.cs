using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExpressionParser
{
    /// <summary>
    /// 計算式のパーサー
    /// </summary>
    public class ExpressionParser
    {
        // 字句解析に使う正規表現
        private readonly string _regex = @"(([0-9]+)|\+|-|\*|/|\(|\)|pi)";
        // 演算子の文字
        internal static readonly string OprationChars = "+-*/";

        // 字句リスト
        private List<string> _lexicons = null;
        // 字句リスト中の現在の読み込み行
        private int _rowIndex = 0;


        /// <summary>
        /// 逆ポーランド記法に並び変えたリストを返す。
        /// </summary>
        /// <param name="expression">文字列表現された式</param>
        /// <returns>式を字句に分解し逆ポーランド記法に並べたリスト</returns>
        public static List<string> GetRPN(string expression)
        {
            ExpressionParser parser = new ExpressionParser();

            // 構文木の作成
            SyntaxNode syntaxNode = parser.GetSyntaxTree(expression);

            // 逆ポーランド記法のリストを返す
            return parser.GetRPNList(syntaxNode);
        }

        /// <summary>
        /// 構文木から逆ポーランド記法のリストを返す。再帰呼び出しの開始点。
        /// </summary>
        /// <param name="syntaxNode">構文木</param>
        /// <returns>逆ポーランド記法に並べたリスト</returns>
        private List<string> GetRPNList(SyntaxNode syntaxNode)
        {
            List<string> rpns = new List<string>();

            AddRecursiveRPNList(syntaxNode, rpns);

            rpns.Reverse();

            return rpns;
        }

        /// <summary>
        /// 構文木からポーランド記法のリストを返す。再帰呼び出しされる。
        /// </summary>
        /// <param name="syntaxNode">構文木</param>
        /// <param name="rpns">ポーランド記法に並べたリスト</param>
        private void AddRecursiveRPNList(SyntaxNode syntaxNode, List<string> rpns)
        {
            rpns.Add(syntaxNode.Value);

            if (syntaxNode.RightChild != null)
            {
                AddRecursiveRPNList(syntaxNode.RightChild, rpns);
            }
            if (syntaxNode.LeftChild != null)
            {
                AddRecursiveRPNList(syntaxNode.LeftChild, rpns);
            }
        }

        /// <summary>
        /// 構文解析後の構文木を返す。再帰呼び出しの開始点。
        /// </summary>
        /// <param name="expression">文字列表現された式</param>
        /// <returns>字句解析後の構文木</returns>
        private SyntaxNode GetSyntaxTree(string expression)
        {
            // 字句解析
            _lexicons = GetLexicons(expression);

            // 事前の構文チェック
            if (!IsValid())
            {
                throw new SyntaxException();
            }

            // 構文解析 後ろから前に字句を読み込む。
            _rowIndex = _lexicons.Count - 1;
            SyntaxNode syntaxNode = GetRecursiveSyntaxTree();

            return syntaxNode;
        }

        // 事前の構文チェック。カッコの数が対応しているかのみチェックする。
        private bool IsValid()
        {
            int level = 0;
            foreach (var lex in _lexicons)
            {
                if (lex == "(")
                {
                    level++; 
                }
                else if (lex == ")")
                {
                    level--;
                }
                if (level < 0)
                {
                    return false;
                }
            }
            return level == 0;
        }

        /// <summary>
        /// 各階層における構文木を返す。再帰呼び出しされる。
        /// </summary>
        /// <returns>字句解析後の構文木</returns>
        private SyntaxNode GetRecursiveSyntaxTree()
        {
            SyntaxNode currentNode = new SyntaxNode();

            // 1つ前に読んだ字句
            string prevLex = string.Empty;

            // 状態遷移図に従い解析を行う。字句リストを逆順に読み込んでいく。
            while (_rowIndex >= 0)
            {
                string lex = _lexicons[_rowIndex];
                // 今読み込んだ字句が演算子の場合
                if (OprationChars.Contains(lex))
                {
                    // 文末直前、演算子直前、「)」直前の演算子はエラー
                    if (prevLex == string.Empty || OprationChars.Contains(prevLex) || prevLex == ")")
                    {
                        throw new SyntaxException();
                    }
                    // 数字、「(」直前に演算子が来た場合はその演算子をノードに格納
                    else
                    {
                        currentNode = CreateNode(currentNode, lex);
                    }
                }
                else if (lex == "(")
                {
                    // 文末直前、「*」直前、「/」直前、「)」直前の「(」はエラー
                    if (prevLex == string.Empty || prevLex == "*" || prevLex == "/" || prevLex == ")")
                    {
                        throw new SyntaxException();
                    }
                    // 「+」直前、「-」直前に「(」が来た場合は「0」を補う
                    else if (prevLex == "+" || prevLex == "-")
                    {
                        currentNode.Value = "0";
                        return currentNode.Root;
                    }
                    // 数字、「(」直前に「(」が来た場合は階層をデクリメント
                    else
                    {
                        return currentNode.Root;
                    }
                }
                else if (lex == ")")
                {
                    // 文末直前、演算子直前、「)」直前に「)」が来た場合は階層をインクリメント
                    if (prevLex == string.Empty || OprationChars.Contains(prevLex) || prevLex == ")")
                    {
                        _rowIndex--;
                        // 「階層をインクリメント」＝自分自身を再帰呼び出しする
                        if (currentNode.Parent == null)
                        {
                            currentNode = GetRecursiveSyntaxTree();
                        }
                        else if (currentNode.Parent.LeftChild == currentNode)
                        {
                            currentNode.Parent.LeftChild = GetRecursiveSyntaxTree();
                            currentNode = currentNode.Parent.LeftChild;
                        }
                        else if (currentNode.Parent.RightChild == currentNode)
                        {
                            currentNode.Parent.RightChild = GetRecursiveSyntaxTree();
                            currentNode = currentNode.Parent.RightChild;
                        }
                        else
                        { 
                            throw new SyntaxException();
                        }
                    }
                    // 数字、「(」直前の「)」はエラー
                    else
                    {
                        throw new SyntaxException();
                    }
                }
                // 今読み込んだ字句が数字の場合
                else
                {
                    // 文末直前、演算子直前、「)」直前に数字が来た場合は、その数字をノードに格納
                    if (prevLex == string.Empty || OprationChars.Contains(prevLex) || prevLex == ")")
                    {
                        currentNode.Value = lex;
                    }
                    // 数字、「(」直前の数字はエラー
                    else
                    {
                        throw new SyntaxException();
                    }
                }
                prevLex = _lexicons[_rowIndex];
                _rowIndex--;
            }

            // 文頭
            // 文末、「*」、「/」、「)」が文頭となるのはエラー
            if (prevLex == string.Empty || prevLex == "*" || prevLex == "/" || prevLex == ")")
            {
                throw new SyntaxException();
            }
            // 「+」、「-」が文頭に来た場合は「0」を補う
            else if (prevLex == "+" || prevLex == "-")
            {
                currentNode.Value = "0";
            }

            return currentNode.Root;
        }

        /// <summary>
        /// ノードを追加しカレントノードへの参照を返す。
        /// </summary>
        /// <param name="node">カレントノード</param>
        /// <param name="opValue">格納する演算子</param>
        /// <returns>新しいカレントノード</returns>
        private SyntaxNode CreateNode(SyntaxNode node, string opValue)
        {
            SyntaxNode parentNode = new SyntaxNode{ Value = opValue };
            SyntaxNode newNode = new SyntaxNode();

            // ここのロジックは別資料参照
            if (node.Parent == null)
            {
                parentNode.RightChild = node;
                parentNode.LeftChild = newNode;
                return newNode;
            }
            else if (PriorTo(opValue, node.Parent.Value) >= 0)
            {
                if (node.Parent.LeftChild == node)
                {
                    node.Parent.LeftChild = parentNode;
                }
                else
                {
                    node.Parent.RightChild = parentNode;
                }
                parentNode.RightChild = node;
                parentNode.LeftChild = newNode;

                return newNode;
            }
            else
            {
                if (node.Parent.Parent == null)
                {
                    // 何もしない
                }
                else if (node.Parent.Parent.LeftChild == node)
                {
                    node.Parent.Parent.LeftChild = parentNode;
                }
                else
                {
                    node.Parent.Parent.RightChild = parentNode;
                }
                parentNode.RightChild = node.Parent;
                parentNode.LeftChild = newNode;

                return newNode;
            }
        }

        // 第一引数の演算子の優先順位が第二引数の演算子より、上位（1）、同じ（0）、下位（-1）かを返す。

        /// <summary>
        /// 第一引数の演算子の優先順位が第二引数の演算子より、上位、同じまたは下位かを返す。
        /// </summary>
        /// <param name="opValue1">第一引数の演算子</param>
        /// <param name="opValue2">第二引数の演算子</param>
        /// <returns>上位（1）、同じ（0）、下位（-1）</returns>
        private int PriorTo(string opValue1, string opValue2)
        {
            if (opValue1 == "+" || opValue1 == "-")
            {
                if (opValue2 == "+" || opValue2 == "-")
                {
                    return 0;
                }
                else if (opValue2 == "*" || opValue2 == "/")
                {
                    return -1;
                }
            }
            else if (opValue1 == "*" || opValue1 == "/")
            {
                if (opValue2 == "+" || opValue2 == "-")
                {
                    return 1;
                }
                else if (opValue2 == "*" || opValue2 == "/")
                {
                    return 0;
                }
            }
            return 0;
        }

        /// <summary>
        /// 字句解析した結果のリストを返す。
        /// </summary>
        /// <param name="expression">文字列表現された式</param>
        /// <returns>字句解析後の構文木</returns>
        private List<string> GetLexicons(string expression)
        {
            MatchCollection matches = Regex.Matches(expression, _regex);

            return matches.Select(m => m.Value).ToList();
        }

    }
}
