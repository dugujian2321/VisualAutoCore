using System.Collections.Generic;
using System.Drawing;

namespace AutoFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class Listbox : ControlBase
    {
        public List<ListBoxRow> Rows;
        public ListBoxRow FirstRow;
        public int itemHeight;
        public int itemWidth;
        public Direction direction = Direction.Vertical;
        public Listbox(string srcFile, ControlBase parent, int colNum) : base(srcFile, parent)
        {
            Rows = new List<ListBoxRow>();
            FirstRow = new ListBoxRow(this);
            FirstRow.Columns = new List<ListRowColumn>(colNum);
            for (int i = 0; i < colNum; i++)
            {
                FirstRow.Columns.Add(new ListRowColumn(FirstRow));
            }
        }
        public Listbox(ControlBase parent) : base(parent)
        {

        }

        public void CreateRow(string name, Rectangle rec)
        {
            Rows.Add(new ListBoxRow(this) { RECT = rec, Name = name });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">base is 0</param>
        public void Select(int index)
        {
            Point target = new Point();
            if (direction == Direction.Vertical)
            {
                target.X = LocationP.X + itemWidth / 2;
                target.Y = LocationP.Y + index * itemHeight / 2;
            }
            else if (direction == Direction.Horizontal)
            {

            }
            Input.Click(target.X, target.Y);
        }
    }

    public enum Direction
    {
        Horizontal = 0,
        Vertical = 1
    }
}
