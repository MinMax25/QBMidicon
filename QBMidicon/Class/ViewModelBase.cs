using CommunityToolkit.Mvvm.ComponentModel;
using libQB.DialogServices;
using libQB.UndoRedo;
using libQB.WindowServices;
using QBMidicon.Class.Services;
using QBMidicon.Contracts.Services;
using QBMidicon.Contracts.ViewModels;

namespace QBMidicon.Class
{
    public abstract partial class ViewModelBase
        : ObservableObject
        , INavigationAware
        , IDisposable
    {
        #region Properties

        #region Services

        protected internal IDIContainer DIContainer;

        protected internal ISettingService SettingService => DIContainer?.SettingService;

        protected internal INavigationService Navigation => DIContainer?.Navigation;

        protected internal IDialogService Dialog => DIContainer?.Dialog;

        protected internal IWindowService WindowService => DIContainer?.WindowService;

        protected internal IUndoManager UndoManager => DIContainer?.UndoManager;

        #endregion

        #endregion

        #region Fields

        private bool disposedValue;

        #endregion

        #region ctor

        public ViewModelBase()
        {
        }

        public ViewModelBase(IDIContainer dIContainer)
        {
            DIContainer = dIContainer;
        }

        #endregion

        #region Methods

        public virtual void OnNavigatedFrom()
        {
        }

        public virtual void OnNavigatedTo(object parameter)
        {
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
