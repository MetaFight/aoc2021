using System.Linq;

public class Cell
{
  private int value;
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
    => new (int x, int y)[] { 
        (x-1, y-1), (x, y-1), (x+1, y-1),
        (x-1, y+0),           (x+1, y+0),
        (x-1, y+1), (x, y+1), (x+1, y+1),
        }
      .Where(point => point.x >= 0 && point.x < this.gridWidth && point.y >= 0 && point.y < this.gridHeight)
      .Select(point => this.IndexFromPosition(point.x, point.y, this.gridWidth));

  public void MeetNeighbors(List<Cell> grid)
    => this.Neighbors = this.GetNeighborIndices().Select(index => grid[index]).ToList();

  public List<Cell> Neighbors { get; private set; }

  public void Energize() {
    this.value++;
    if (!this.Flashed && this.value > 9) {
      this.Flashed = true;
      this.Neighbors.ForEach(n => n.Energize());
    }
  }

  public void StepEnd() {
    if (this.value > 9) {
      this.value = 0;
      this.Flashed = false;
    }
  }

  public bool Flashed { get; private set; }

  public override string ToString() => $"({this.x},{this.y}):{this.value}";
}