namespace AutoFramework
{
    public class ListRowColumn : ControlBase
    {
        public string Name { get; set; }
        public ListRowColumn(ListBoxRow parent) : base(parent)
        {
            Parent = parent;
        }
    }
}
