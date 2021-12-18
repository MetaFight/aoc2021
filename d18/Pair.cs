public class Pair : Base
{
    public static Pair Parse(string encoded, out int charsRead)
    {
        charsRead = 0;

        static Base ParseElement(string encoded, ref int charsRead)
        {
            Base result;
            char nextChar = encoded[charsRead];
            if (nextChar == '[')
            {
                result = Parse(encoded[charsRead..], out var readCount);
                charsRead += readCount;
            }
            else
            {
                result = new Regular(int.Parse(nextChar.ToString()));
                charsRead++;
            }
            return result;
        }

        // opening bracket
        charsRead++;
        // left element
        Base left = ParseElement(encoded, ref charsRead);
        // comma
        charsRead++;
        // right element
        Base right = ParseElement(encoded, ref charsRead);
        // closing bracket
        charsRead++;

        var result = new Pair(left, right);
        left.SetParent(result);
        right.SetParent(result);

        return result;
    }

    public Pair(Base left, Base right, Pair? parent = null) : base(parent)
    {
        this.Left = left;
        this.Left.SetParent(this);
        this.Right = right;
        this.Right.SetParent(this);
    }

    public Base Left { get; set; }
    public Base Right { get; set; }

    private bool CanExplode
        => this.Left is Regular &&
            this.Right is Regular &&
            this.parent?.parent?.parent?.parent != null;

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
            var parent = current.parent;

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
            var parent = current.parent;

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
        if (this.parent != null)
        {
            if (this.parent.Left == this)
            {
                this.parent.Left = new Regular(0, this);
            }
            else
            {
                this.parent.Right = new Regular(0, this);
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

    public override string ToString() => $"[{this.Left},{this.Right}]";
}