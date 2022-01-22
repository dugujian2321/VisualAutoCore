namespace AutoFramework
{
    public class TextBlock : ControlBase
    {
        public TextBlock(ControlBase parent) : base(parent)
        {
            Parent = parent;
        }

        public TextBlock(string mark, ControlBase parent) : base(mark, parent)
        {
            Parent = parent;
        }
    }
}
