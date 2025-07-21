using System;

namespace AppName_Rename.UI
{
    public abstract record PopupUIData(
        string Subject,
        string PrimaryButtonText,
        string SecondaryButtonText,
        Action PrimaryButtonAction,
        Action SecondaryButtonAction
    ) : IUIData;

    public record PopupInitData(string Key);

    public record PopupShowData(
        string Subject,
        string PrimaryButtonText = null,
        string SecondaryButtonText = null,
        Action PrimaryButtonAction = null,
        Action SecondaryButtonAction = null
    ) : PopupUIData(Subject, PrimaryButtonText, SecondaryButtonText, PrimaryButtonAction, SecondaryButtonAction);

    public class PopupConfig<TInitData, TShowData> : UIConfigBase<TInitData, TShowData>
        where TInitData : PopupInitData
        where TShowData : PopupShowData
    {
        public class Builder : BuilderBase<Builder>
        {
            public Builder() : base(new PopupConfig<TInitData, TShowData>())
            {
            }
        }
    }
}