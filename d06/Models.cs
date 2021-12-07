public class PopulationSlice
{
  public PopulationSlice(int offset, ulong size) {
    this.Offset = offset;
    this.Size = size;
  }

  public void IncrementAge(out Action populationChange) {
    var capturedSize = this.Size;
    this.Size = 0;

    if (this.IsGivingBirth) {
      populationChange = () => {
        this.ContinuationSlice.Size += capturedSize;
        this.ChildSlice.Size += capturedSize;
      };
    }
    else
    {
      populationChange = () => {
        this.ContinuationSlice.Size += capturedSize;
      };
    }
  }

  public PopulationSlice ChildSlice { get; set; }
  public PopulationSlice ContinuationSlice { get; set; }
  public int Offset { get; }
  public ulong Size { get; private set; }
  private bool IsGivingBirth => this.Offset == 0;

  public override string ToString() => $"slice[{this.Offset}:{this.Size}]";
}