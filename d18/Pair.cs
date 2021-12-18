public class Pair : Base
{
    public static Pair Parse(string encoded, out int charsRead, Pair? parent = null)
    {
        charsRead = 0;

        static Base ParseElement(string encoded, ref int charsRead, Pair? parent = null)
        {
            Base result;

            char nextChar = encoded[charsRead];
            if (nextChar == '[')
            {
                result = Parse(encoded[charsRead..], out var readCount, parent);
                charsRead += readCount;
            }
            else
            {
                result = new Regular(int.Parse(nextChar.ToString()), parent);
                charsRead++;
            }

            return result;
        }

        var pair = new Pair(null, null, parent);

        // opening bracket
        charsRead++;
        // left element
        Base left = ParseElement(encoded, ref charsRead, pair);
        // comma
        charsRead++;
        // right element
        Base right = ParseElement(encoded, ref charsRead, pair);
        // closing bracket
        charsRead++;

        pair.Left = left;
        pair.Right = right;

        return pair;
    }

    public Pair(Base? left = null, Base? right = null, Pair? parent = null) : base(parent)
    {
        if (left != null)
        {
            left.Parent = this;
            this.Left = left;
        }
        if (right != null)
        {
            right.Parent = this;
            this.Right = right;
        }
    }

    public Base Left { get; set; }
    public Base Right { get; set; }

    private bool CanExplode
        => this.Left is Regular &&
            this.Right is Regular &&
            this.Parent?.Parent?.Parent?.Parent != null;

    private bool RecursiveCanExplode
        => this.CanExplode ||
            (this.Left is Pair leftPair && leftPair.RecursiveCanExplode) ||
            (this.Right is Pair rightPair && rightPair.RecursiveCanExplode);

    private bool RecursiveExplodeOne()
    {
        if (this.CanExplode)
        {
            this.Explode();
            return true;
        }

        if (this.Left is Pair leftPair && leftPair.RecursiveCanExplode)
        {
            return leftPair.RecursiveExplodeOne();
        }

        if (this.Right is Pair rightPair && rightPair.RecursiveCanExplode)
        {
            return rightPair.RecursiveExplodeOne();
        }

        return false;
    }

    private void Explode()
    {
        // Find leftTarget
        Regular? leftTarget = null;
        var current = this;
        while (current != null)
        {
            var parent = current.Parent;

            if (parent?.Right == current)
            {
                var candidateLeft = parent.Left;
                while (candidateLeft is Pair leftPair)
                {
                    candidateLeft = leftPair.Right;
                }
                leftTarget = candidateLeft as Regular;
                break;
            }
            current = parent;
        }

        // Find rightTarget
        Regular? rightTarget = null;
        current = this;
        while (current != null)
        {
            var parent = current.Parent;

            if (parent?.Left == current)
            {
                var candidateRight = parent.Right;
                while (candidateRight is Pair rightPair)
                {
                    candidateRight = rightPair.Left;
                }
                rightTarget = candidateRight as Regular;
                break;
            }
            current = parent;
        }

        // Distribute values
        if (leftTarget != null && this.Left is Regular left)
        {
            leftTarget.Value += left.Value;
        }

        if (rightTarget != null && this.Right is Regular right)
        {
            rightTarget.Value += right.Value;
        }

        // Replace with zero
        if (this.Parent != null)
        {
            if (this.Parent.Left == this)
            {
                this.Parent.Left = new Regular(0, this.Parent);
            }

            if (this.Parent.Right == this)
            {
                this.Parent.Right = new Regular(0, this.Parent);
            }
        }
    }

    public override bool CanReduce => this.CanExplode || this.Left.CanReduce || this.Right.CanReduce;

    public override void Reduce()
    {
        if (this.RecursiveCanExplode && this.RecursiveExplodeOne())
        {
            return;
        }

        if (this.Left.CanReduce)
        {
            this.Left.Reduce();
            return;
        }

        if (this.Right.CanReduce)
        {
            this.Right.Reduce();
            return;
        }
    }

    public override ulong Magnitude => (3 * this.Left.Magnitude) + (2 * this.Right.Magnitude);

    public override string ToString() => $"[{this.Left},{this.Right}]";
}