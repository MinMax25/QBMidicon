using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace QBMidicon.ViewModels
{
    public partial class SaveParameter
        : ObservableObject
    {
        #region Properties

        [ObservableProperty]
        private string folderPath = string.Empty;

        [ObservableProperty]
        private string fileName = string.Empty;

        [ObservableProperty]
        private bool isRawData = true;

        [ObservableProperty]
        private bool isFilteredData = true;

        [ObservableProperty]
        private bool isLyricTrackData = true;

        [ObservableProperty]
        private bool isLyricTextData = true;

        [ObservableProperty]
        private bool isLyricSRTData = true;

        #endregion

        #region ctor

        public SaveParameter()
        {

        }

        #endregion

        #region Methods

        public SaveParameter(string filename, string name)
        {
            FolderPath = Path.GetDirectoryName(filename);
            FileName = name;
        }

        #endregion
    }
}
