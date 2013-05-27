using System;

namespace Funcy
{
    public static class OptionModule
    {
        /// <summary>
        /// オプションが<c>None</c>でない場合、<c>true</c>を返します。
        /// </summary>
        /// <typeparam name="T">入力型</typeparam>
        /// <param name="option">入力オプション</param>
        /// <returns>オプションが<c>None</c>でない場合は<c>true</c></returns>
        public static bool IsSome<T>(Option<T> option)
        {
            return option.IsSome;
        }

        /// <summary>
        /// オプションが<c>None</c>の場合、<c>true</c>を返します。
        /// </summary>
        /// <typeparam name="T">入力型</typeparam>
        /// <param name="option">入力オプション</param>
        /// <returns>オプションが<c>None</c>の場合は<c>true</c></returns>
        public static bool IsNone<T>(Option<T> option)
        {
            return option.IsNone;
        }

        /// <summary>
        /// オプションに関連付けられている値を取得します。
        /// </summary>
        /// <typeparam name="T">入力型</typeparam>
        /// <param name="option">入力オプション</param>
        /// <returns>オプション内の値</returns>
        public static T Get<T>(Option<T> option)
        {
            if (option.IsNone)
                throw new ArgumentException("オプション値は None でした", "option");

            var some = option as Some<T>;
            return some.Value;
        }

        /// <summary>
        /// 省略可能な値に対して関数を呼び出します。
        /// </summary>
        /// <typeparam name="T">入力型</typeparam>
        /// <typeparam name="U">出力型</typeparam>
        /// <param name="binder">オプションから型 T の値を受け取り、型 U の値を格納するオプションに変換する関数</param>
        /// <param name="option">入力オプション</param>
        /// <returns>バインダーの出力型のオプション</returns>
        public static Option<U> Bind<T, U>(Func<T, Option<U>> binder, Option<T> option)
        {
            return option.Bind(binder);
        }
    }

    public abstract class Option<T>
    {
        /// <summary>
        /// <c>Some</c>型かどうかを取得します。
        /// </summary>
        public abstract bool IsSome { get; }

        /// <summary>
        /// <c>None</c>型かどうかを取得します。
        /// </summary>
        public bool IsNone
        {
            get { return !IsSome; }
        }

        /// <summary>
        /// <c>Some</c>型の場合、値を取得します。<c>None</c>型の場合、NullReferenceExceptionを送出します。
        /// </summary>
        public abstract T Value { get; protected set; }

        /// <summary>
        /// 渡された値を<c>Some</c>型で包んで返します。
        /// </summary>
        /// <param name="value"><c>Some</c>型に格納したい値</param>
        /// <returns><c>Some</c>型の値</returns>
        public static Option<T> Some(T value)
        {
            return Some<T>.Create(value);
        }

        /// <summary>
        /// <c>None</c>型の値を返します。
        /// </summary>
        /// <returns><c>None</c>型の値</returns>
        public static Option<T> None()
        {
            return None<T>.Self;
        }
                
        /// <summary>
        /// 省略可能な値に対して関数を呼び出します。
        /// </summary>
        /// <typeparam name="U">出力型</typeparam>
        /// <param name="binder">オプションから型 T の値を受け取り、型 U の値を格納するオプションに変換する関数</param>
        /// <returns>バインダーの出力型のオプション</returns>
        public Option<U> Bind<U>(Func<T, Option<U>> binder)
        {
            return this.IsSome ? binder(this.Value) : Option<U>.None();
        }

        /// <summary>
        /// 現在のオブジェクトを表す文字列を返します。
        /// </summary>
        /// <returns>現在のオブジェクトを説明する文字列</returns>
        public abstract override string ToString();

        /// <summary>
        /// 擬似パターンマッチを提供します。
        /// </summary>
        /// <param name="someCase"><c>Some</c>型の値の場合に行う処理</param>
        /// <param name="noneCase"><c>None</c>型の値の場合に行う処理</param>
        public abstract void MatchWith(Action<T> someCase, Action noneCase);

        /// <summary>
        /// 擬似パターンマッチを提供します。
        /// </summary>
        /// <typeparam name="U">処理結果の型</typeparam>
        /// <param name="someCase"><c>Some</c>型の値の場合に行う処理</param>
        /// <param name="noneCase"><c>None</c>型の値の場合に行う処理</param>
        /// <returns>処理結果</returns>
        public abstract U MatchWith<U>(Func<T, U> someCase, Func<U> noneCase);

        /// <summary>
        /// 指定のオブジェクトが現在のオブジェクトと等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj">現在のオブジェクトと比較するオブジェクト。</param>
        /// <returns>指定したオブジェクトが現在のオブジェクトと等しい場合は true。それ以外の場合は false。</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var other = obj as Option<T>;
            if (other == null)
                return false;
            
            return this.EqualsAsOption(other);
        }

        /// <summary>
        /// 特定の型のハッシュ関数として機能します。
        /// </summary>
        /// <returns>現在の<c>Object</c>のハッシュ コード。</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        protected abstract bool EqualsAsOption(Option<T> other);
    }

    public class Some<T> : Option<T>
    {
        public override bool IsSome
        {
            get
            {
                return true;
            }
        }

        private T value;
        public override T Value
        {
            get
            {
                return value;
            }
            protected set
            {
                this.value = value;
            }
        }

        private Some(T value)
        {
            this.value = value;
        }

        /// <summary>
        /// <c>Some</c>型の値を生成します。
        /// </summary>
        /// <param name="value"><c>Some</c>型に包みたい値</param>
        /// <returns>生成した<c>Some</c>型の値</returns>
        internal static Option<T> Create(T value)
        {
            return new Some<T>(value);
        }

        public override string ToString()
        {
            return string.Format("Some({0})", this.value);
        }

        public override void MatchWith(Action<T> someCase, Action noneCase)
        {
            someCase(value);
        }

        public override U MatchWith<U>(Func<T, U> someCase, Func<U> noneCase)
        {
            return someCase(value);
        }

        protected override bool EqualsAsOption(Option<T> other)
        {
            var some = other as Some<T>;
            if (some == null)
                return false;
            else if (this.value == null && some.value == null)
                return true;
            else if (this.value == null || some.value == null)
                return false;
            else
                return this.value.Equals(some.value);
        }

        public override int GetHashCode()
        {
            return 41 + (value == null ? 0 : value.GetHashCode());
        }
    }

    public class None<T> : Option<T>
    {
        public override bool IsSome
        {
            get { return false; }
        }

        public override T Value
        {
            get
            {
                throw new NullReferenceException("値は null です。");
            }
            protected set
            {
                throw new InvalidOperationException("None<T> では操作出来ません。");
            }
        }

        /// <summary>
        /// <c>None</c>型の唯一のインスタンス
        /// </summary>
        static internal readonly Option<T> Self;

        static None() { Self = new None<T>(); }

        private None() { }

        public override string ToString()
        {
            return "None";
        }

        public override void MatchWith(Action<T> someCase, Action noneCase)
        {
            noneCase();
        }

        public override U MatchWith<U>(Func<T, U> someCase, Func<U> noneCase)
        {
            return noneCase();
        }

        protected override bool EqualsAsOption(Option<T> other)
        {
            var none = other as None<T>;
            return none != null;
        }
    }

}