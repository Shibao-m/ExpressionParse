using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionParser
{
    /// <summary>
    /// ノード
    /// </summary>
    internal class SyntaxNode
    {
        private string _Value = string.Empty;
        /// <summary>
        /// ノードの値を取得または設定する。
        /// </summary>
        public string Value
        {
            set
            {
                // 整数、演算子またはpiのみ入力可
                if (int.TryParse(value, out int i)
                    || ExpressionParser.OprationChars.Contains(value)
                    || value == "pi")
                {
                    _Value = value;
                }
            }
            get { return _Value; }
        }

        private SyntaxNode _LeftChild = null;
        /// <summary>
        /// このノードの左につくノードを取得または設定する。
        /// </summary>
        public SyntaxNode LeftChild
        {
            get
            {
                return _LeftChild;
            }
            set
            {
                _LeftChild = value;
                value.Parent = this;
            }
        }

        private SyntaxNode _RightChild = null;
        /// <summary>
        /// このノードの右につくノードを取得または設定する。
        /// </summary>
        public SyntaxNode RightChild
        {
            get
            {
                return _RightChild;
            }
            set
            {
                _RightChild = value;
                value.Parent = this;
            }
        }

        /// <summary>
        /// このノードの親ノードを取得する。
        /// </summary>
        public SyntaxNode Parent { get; private set; } = null;

        /// <summary>
        /// このノードのルートノード（最も祖先のノード）を取得する。
        /// </summary>
        public SyntaxNode Root
        {
            get
            {
                if (Parent != null)
                {
                    return Parent.Root;
                }
                else
                {
                    return this;
                }
            }
        }
    }
}
