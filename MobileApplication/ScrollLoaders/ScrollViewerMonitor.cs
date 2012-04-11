using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

using MobileClientLibrary.Models;
using MetrocamPan;
using System.Collections;
using JeffWilcox.FourthAndMayor;

namespace MetrocamPan.ScrollLoaders
{
    public class ScrollViewerMonitor
    {
        public static DependencyProperty AtEndCommandProperty
            = DependencyProperty.RegisterAttached(
                "AtEndCommand", typeof(ICommand),
                typeof(ScrollViewerMonitor),
                new PropertyMetadata(OnAtEndCommandChanged));

        public static ICommand GetAtEndCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(AtEndCommandProperty);
        }

        public static void SetAtEndCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(AtEndCommandProperty, value);
        }

        public static void OnAtEndCommandChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)d;
            if (element != null)
            {
                element.Loaded -= element_Loaded;
                element.Loaded += element_Loaded;
            }
        }

        static void element_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)sender;
            element.Loaded -= element_Loaded;
            ScrollViewer scrollViewer = FindChildOfType<ScrollViewer>(element);
            if (scrollViewer == null)
            {
                throw new InvalidOperationException("ScrollViewer not found.");
            }

            var listener = new DependencyPropertyListener();
            listener.Changed
                += delegate
                {
                    // This bool denotes first load when an xaml element is loaded with no elements
                    bool firstLoad = (Math.Round(scrollViewer.VerticalOffset) == 0) 
                        && (Math.Round(scrollViewer.ScrollableHeight) == 0);

                    bool atBottom = Math.Round(scrollViewer.VerticalOffset)
                                        >= Math.Round(scrollViewer.ScrollableHeight);

                    if (atBottom && !firstLoad)
                    {
                        //GlobalLoading.Instance.IsLoading = true;

                        if (element.Name.Equals("recentPictures"))
                        {
                            foreach (PictureInfo p in App.ContinuedRecentPictures)
                            {
                                App.RecentPictures.Add(p);
                            }

                            App.ContinuedRecentPictures.Clear();
                        }
                        else if (element.Name.Equals("UserPictures"))
                        {
                            int count = 0;
                            int numberOfPicturesPerLoad = 24;
                            foreach (PictureInfo p in UserDetailPage.ContinuedUserPictures)
                            {
                                // Only load 24 PictureInfo objects at a time
                                if (count < numberOfPicturesPerLoad)
                                {
                                    App.UserPictures.Add(p);
                                    count++;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            // Now we delete those PictureInfo objects that were added
                            for (int i = 0; i < numberOfPicturesPerLoad; i++)
                            {
                                if (UserDetailPage.ContinuedUserPictures.Count == 0)
                                    break;
                                UserDetailPage.ContinuedUserPictures.RemoveAt(0);
                            }
                        }

                        /*
                        var atEnd = GetAtEndCommand(element);
                        if (atEnd != null)
                        {
                           atEnd.Execute(null);
                        }*/
                    }
                };
            Binding binding = new Binding("VerticalOffset") { Source = scrollViewer };
            listener.Attach(scrollViewer, binding);
        }

        static T FindChildOfType<T>(DependencyObject root) where T : class
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                DependencyObject current = queue.Dequeue();
                for (int i = VisualTreeHelper.GetChildrenCount(current) - 1; 0 <= i; i--)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    var typedChild = child as T;
                    if (typedChild != null)
                    {
                        return typedChild;
                    }
                    queue.Enqueue(child);
                }
            }
            return null;
        }
    }
}
