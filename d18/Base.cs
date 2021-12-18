public abstract class Base
{
    protected Pair? parent;

    public Base(Pair? parent = null) { this.parent = parent; }
    
    public void SetParent(Pair parent) => this.parent = parent;

    public virtual bool CanReduce => false;

    public abstract void Reduce();
}