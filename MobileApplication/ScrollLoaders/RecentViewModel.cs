using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;

using MobileClientLibrary.Models;
using MetrocamPan;
using System.Collections;
using JeffWilcox.FourthAndMayor;

namespace MetrocamPan.ScrollLoaders
{
    public class RecentViewModel
    {
        readonly DelegateCommand fetchMoreDataCommand;

        public RecentViewModel()
        {
            fetchMoreDataCommand = new DelegateCommand(
                obj =>
                {
                    ThreadPool.QueueUserWorkItem(
                        delegate
                        {
                            /* This is just to demonstrate a slow operation. */
                            Thread.Sleep(10000);
                            /* We invoke back to the UI thread. */
                            Deployment.Current.Dispatcher.BeginInvoke(
                                delegate
                                {
                                    GlobalLoading.Instance.IsLoading = true;

                                    foreach (PictureInfo p in App.ContinuedRecentPictures)
                                    {
                                        App.RecentPictures.Add(p);
                                    }

                                    App.ContinuedRecentPictures.Clear();

                                    GlobalLoading.Instance.IsLoading = false;
                                });
                        });

                });
        }

        public ICommand FetchMoreDataCommand
        {
            get
            {
                return fetchMoreDataCommand;
            }
        }
    }
}
