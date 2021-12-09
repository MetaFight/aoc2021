using System.Linq;

public class Cell
{
  private readonly int value;
  private readonly int index;
  private readonly int gridWidth;
  private readonly int gridHeight;
  private readonly int x;
  private readonly int y;
  
  public Cell(int value, int index, int gridWidth, int gridHeight) {
    this.value = value;
    this.index = index;
    this.gridWidth = gridWidth;
    this.gridHeight = gridHeight;
    this.x = this.index % gridWidth;
    this.y = this.index / gridWidth;
  }

  private int IndexFromPosition(int x, int y, int gridWidth) => x + (y * gridWidth);

  private IEnumerable<int> GetNeighborIndices()
    => new (int x, int y)[] { (x, y-1), (x-1, y), (x+1, y), (x, y+1) }
      .Where(point => point.x >= 0 && point.x < this.gridWidth && point.y >= 0 && point.y < this.gridHeight)
      .Select(point => this.IndexFromPosition(point.x, point.y, this.gridWidth));

  public void MeetNeighbors(List<Cell> grid)
    => this.Neighbors = this.GetNeighborIndices().Select(index => grid[index]).ToList();

  public IEnumerable<Cell> Neighbors { get; private set; }

  public bool IsMinimum => this.value < this.Neighbors.Min(n => n.value);

  public int RiskLevel => this.IsMinimum ? this.value + 1 : 0;

  public int BasinSize => 
    this.IsMinimum 
      ? this.Inlets.Distinct().Count()
      : 0;
  
  public IEnumerable<Cell> Inlets 
    => new [] { this }.Concat(this.Neighbors.Where(n => n.value != 9 && (n.value == (this.value + 1))).SelectMany(item => item.Inlets));

  public override string ToString() => $"({this.x},{this.y}):{this.value}";
}