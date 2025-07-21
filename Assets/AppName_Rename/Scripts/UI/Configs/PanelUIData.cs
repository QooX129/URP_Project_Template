namespace AppName_Rename.UI
{
    public record PanelUIData<T1>(T1 Param1) : IUIData;
    public record PanelUIData<T1, T2>(T1 Param1, T2 Param2) : IUIData;
}