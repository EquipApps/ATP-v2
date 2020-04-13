using Splat;
using System;

namespace ReactiveUI
{
    public static class ViewForExt
    {
        public static void InitializeViewModel<TViewModel>(this IViewFor<TViewModel> viewFor)
            where TViewModel : class
        {
            if (viewFor.ViewModel != null)
            {
                return;
            }

            var viewModel = GetViewModel<TViewModel>();
            if (viewModel == null)
            {
                LogHost.Default.Error($"Модель представление не зарегистрированно: {typeof(TViewModel).Name}");
                return;
            }

            viewFor.ViewModel = viewModel;
        }

        private static TViewModel GetViewModel<TViewModel>() where TViewModel : class
        {
            try
            {
                return Locator.Current.GetService<TViewModel>();
            }
            catch (Exception ex)
            {
                LogHost.Default.Fatal(ex);

                return null;
            }
        }
    }
}
