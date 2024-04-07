namespace GameCore.Source.Common.Localization
{
    public interface ILocalizable
    {
        string LocalizationKey { get; }
        string LocalizedValue { get; }
        void SetValue(string localizedValue);
    }
}