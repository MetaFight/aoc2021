public class Regular : Base
{
    public int Value { get; set; }

    public Regular(int value, Pair? parent = null) : base(parent) { this.Value = value; }

    private bool CanSplit => this.Value >= 10;

    private void Split()
    {
        var asSplit = this.AsSplit();
        if (this.parent.Left == this)
        {
            this.parent.Left = asSplit;
        }
        else
        {
            this.parent.Right = asSplit;
        }
    }

    private Pair AsSplit()
      => new Pair(
        left: new Regular((int)Math.Floor(this.Value / 2f)),
        right: new Regular((int)Math.Ceiling(this.Value / 2f)),
        parent: this.parent);

    public override bool CanReduce => this.CanSplit;

    public override void Reduce() => this.Split();

    public override string ToString() => this.Value.ToString();
}