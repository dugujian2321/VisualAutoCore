using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AutoFramework
{
    public class ComboBox : ControlBase
    {
        public List<ComboBoxItem> Items { get; set; }
        public ComboBox(ControlBase parent) : base(parent)
        {
            Items = new List<ComboBoxItem>();
        }
        public void Select(ComboBoxItem item, bool bUnfold = true)
        {
            if (bUnfold)
                Input.Click(new System.Drawing.Point(LocationP.X + 30, LocationP.Y + 10));
            System.Threading.Thread.Sleep(rnd.Next(400, 800));
            Input.Click(item.pControlCenter);
            System.Threading.Thread.Sleep(rnd.Next(400, 800));
        }
        public void Select(string name, bool bUnfold = true)
        {
            ComboBoxItem item = Items.Where(x => x.Name == name).FirstOrDefault();
            if (bUnfold)
                Input.Click(new System.Drawing.Point(LocationP.X + 30, LocationP.Y + 10));
            System.Threading.Thread.Sleep(rnd.Next(400, 800));
            Input.Click(item.pControlCenter);
            System.Threading.Thread.Sleep(rnd.Next(400, 800));
        }
        public void AddItem(string name, Rectangle rect)
        {
            Items.Add(new ComboBoxItem(this) { Name = name, RECT = rect });
        }

        public void AddItem(ComboBoxItem jX3ComboBox)
        {
            Items.Add(jX3ComboBox);
        }
    }
}
