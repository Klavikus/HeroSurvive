namespace GameCore.Source.Common
{
    public interface ILocalizable
    {
        string LocalizationKey { get; }
        string LocalizedValue { get; }
        void SetValue(string localizedValue);
    }
}