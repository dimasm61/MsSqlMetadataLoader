namespace MsSqlMetadataLoader
{
    public class MAttrTable
    {
        public string Title;

        public int ListWinWidth;
        public int ListWinHeight;

        public int EditWinWidth;
        public int EditWinHeight;

        public bool IsShowPropGridInList = true;
        public bool IsShowMainMenu = false;

        public bool IsEditInWindow = false;

        public void CopyFrom(MAttrTable source)
        {
            Title = source.Title;
            ListWinWidth = source.ListWinWidth;
            ListWinHeight = source.ListWinHeight;
            EditWinWidth = source.EditWinWidth;
            EditWinHeight = source.EditWinHeight;
            IsShowPropGridInList = source.IsShowPropGridInList;
            IsShowMainMenu = source.IsShowMainMenu;
            IsEditInWindow = source.IsEditInWindow;
        }
    }


}

