public abstract class Base
{
    public Base(Pair? parent = null) { this.Parent = parent; }

    public Pair? Parent { get; set; }

    public virtual bool CanReduce => false;

    public abstract void Reduce();

    public abstract ulong Magnitude { get; }
}