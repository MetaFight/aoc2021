using System.Linq;

public record Point(int x, int y);

public class Line
{
  public Line(int x1, int y1, int x2, int y2)
  {
    this.X1 = x1;
    this.Y1 = y1;
    this.X2 = x2;
    this.Y2 = y2;
  }

  public int X1 { get; }
  public int Y1 { get; }
  public int X2 { get; }
  public int Y2 { get; }

  public bool IsHorizontal => this.Y1 == this.Y2;
  public bool IsVertical => this.X1 == this.X2;
  public bool IsDiagonal => Math.Abs(this.X2 - this.X1) == Math.Abs(this.Y2 - this.Y1);

  public int HorizontalIncrement => (this.X2 - this.X1) / Math.Abs(this.X2 - this.X1);
  public int VerticalIncrement => (this.Y2 - this.Y1) / Math.Abs(this.Y2 - this.Y1);

  public IEnumerable<Point> Range
    => this.IsHorizontal ? Enumerable.Range(Math.Min(this.X1, this.X2), Math.Abs(this.X2 - this.X1) + 1).Select(item => new Point(item, this.Y1)).ToList()
      : this.IsVertical ? Enumerable.Range(Math.Min(this.Y1, this.Y2), Math.Abs(this.Y2 - this.Y1) + 1).Select(item => new Point(this.X1, item)).ToList()
      : this.IsDiagonal ? Enumerable.Range(0, Math.Abs(this.X2 - this.X1) + 1).Select(offset => new Point(this.X1 + (offset * this.HorizontalIncrement), this.Y1 + (offset * this.VerticalIncrement))).ToList()
        : Enumerable.Empty<Point>();

  public override string ToString() => $"{this.X1},{this.Y1} -> {this.X2},{this.Y2}";
}