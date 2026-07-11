using System.ComponentModel;
using libQB.Enums;

namespace QBMidicon.Class.Services
{
    public interface ISettingService
        : INotifyPropertyChanged
        , IDisposable
    {
        #region Properties

        BaseTheme BaseTheme { get; set; }

        string ThemeColor { get; set; }

        Languages Language { get; set; }

        string LastOpenedFilePath { get; set; }

        string LastSaveFolderPath { get; set; }

        #endregion

        #region Methods

        public void Load();

        public void Save();

        #endregion
    }
}
