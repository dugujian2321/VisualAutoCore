namespace AutoFramework
{
    public class Button : ControlBase
    {
        public Button(string srcFile, ControlBase parent) : base(srcFile, parent)
        {

        }

        public Button(ControlBase parent) : base(parent)
        {

        }

        public void Click(int delay = 500)
        {
            Input.Click(pControlCenter.X, pControlCenter.Y, 5, 5, delay);
        }

        public void DragAndDrop()
        {

        }
    }
}
