using System.Linq;

public class Node
{
  public Node(string name) {
    this.Name = name;
    this.Neighbors = new List<Node>();
  }

  public List<Node> Neighbors { get; set; }
  public string Name { get; }

  public bool IsVisitableOnlyOnce => char.IsLower(this.Name[0]);

  public IEnumerable<Node> ChartCourseTo(Node destination, List<Node> visited = null) {
    visited = visited ?? new List<Node>();
    visited.Add(this);

    if (this == destination) {
      return visited;
    }

    var offLimits = visited.Where(item => item.IsVisitableOnlyOnce);
    return this.Neighbors.Except(offLimits).SelectMany(n => n.ChartCourseTo(destination, visited.ToList()));
  }

  public IEnumerable<Node> ChartLeasurelyCourseTo(Node destination, List<Node> visited = null, bool extraTimeSpent = false) {
    visited = visited ?? new List<Node>();
    
    extraTimeSpent |= this.IsVisitableOnlyOnce && visited.Contains(this);

    visited.Add(this);

    if (this == destination) {
      return visited;
    }

    var offLimits = 
      extraTimeSpent 
        ? visited.Where(item => item.IsVisitableOnlyOnce)
        : Enumerable.Empty<Node>();

    return this.Neighbors
      .OrderBy(item => item.Name)
      .Where(item => item.Name != "start")
      .Except(offLimits)
      .SelectMany(n => n.ChartLeasurelyCourseTo(destination, visited.ToList(), extraTimeSpent));
  }

  public override string ToString() => $"{this.Name}";
}
