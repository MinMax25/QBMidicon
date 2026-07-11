using CommunityToolkit.Mvvm.ComponentModel;
using QBMidicon.Class.Services;

namespace QBMidicon.Class
{
    public abstract partial class ViewModelBaseHasContents
        : ViewModelBase
    {
        #region Properties

        [ObservableProperty]
        private bool isContentEnabled;

        [ObservableProperty]
        private ViewModelBase contentViewModel;

        #endregion

        #region ctor

        public ViewModelBaseHasContents()
        {
        }

        public ViewModelBaseHasContents(IDIContainer diContainer)
            : base(diContainer)
        {
        }

        #endregion
    }
}