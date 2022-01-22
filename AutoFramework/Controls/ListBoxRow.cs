using System.Collections.Generic;

namespace AutoFramework
{
    public class ListBoxRow : ControlBase
    {
        public string Name { get; set; }
        public List<ListRowColumn> Columns = new List<ListRowColumn>();
        public ListBoxRow(ControlBase parent) : base(parent)
        {
            Parent = parent;
        }

    }
}
