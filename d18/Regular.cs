public class Regular : Base
{
    public int Value { get; set; }

    public Regular(int value, Pair? parent = null) : base(parent) { this.Value = value; }

    private bool CanSplit => this.Value >= 10;

    private void Split()
    {
        var asSplit = this.AsSplit();
        if (this.Parent!.Left == this)
        {
            this.Parent.Left = asSplit;
        }

        if (this.Parent!.Right == this)
        {
            this.Parent.Right = asSplit;
        }
    }

    private Pair AsSplit()
      => new Pair(
        left: new Regular((int)Math.Floor(this.Value / 2f)),
        right: new Regular((int)Math.Ceiling(this.Value / 2f)),
        parent: this.Parent);

    public override bool CanReduce => this.CanSplit;

    public override void Reduce() => this.Split();

    public override ulong Magnitude => (ulong)this.Value;

    public override string ToString() => this.Value.ToString();
}