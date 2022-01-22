namespace AutoFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class CheckBox : ControlBase
    {
        string CheckedPic = string.Empty;
        public CheckBox(string srcFile, ControlBase parent) : base(srcFile, parent)
        {
            CheckedPic = srcFile;
        }

        public void Check()
        {
            if (IsChecked) return;
            Input.Click(new System.Drawing.Point(LocationP.X + 10, LocationP.Y + 10));
        }

        public void UnCheck()
        {
            if (!IsChecked) return;
        }

        public bool IsChecked
        {
            get
            {
                return ImageManager.Instance.SearchPicture(RECT, CheckedPic) != pNULL;
            }
        }
    }
}

