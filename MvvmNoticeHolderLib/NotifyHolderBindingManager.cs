using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MvvmNoticeHolderLib
{
    public class NotifyHolderBindingManager
    {
        private static T BindSlaveProperty<T>(T source, object root)
        {
            try
            {
                if (source != null)
                {
                    var type = source.GetType();

                    var properties = type.GetProperties();

                    var NoticeFlags = type.GetCustomAttributes<NoticeFlagAttribute>();

                    if (NoticeFlags != null && NoticeFlags.Count() > 0)
                    {
                        foreach (var noticeFlag in NoticeFlags)
                        {
                            PropertyInfo info = properties.SingleOrDefault(x => x.Name == noticeFlag.Name);

                            if (info != null)
                            {
                                BindProperty(source, root, info);
                            }
                        }
                    }
                }
                return source;
            }
            catch (Exception ex)
            {
                return source;
            }
        }

        public static T BindSelfProperty<T>(T root)
        {
            try
            {
                if (root != null)
                {
                    var type = root.GetType();

                    var properties = type.GetProperties();

                    var NoticeFlags = type.GetCustomAttributes<NoticeFlagAttribute>();

                    if (NoticeFlags != null && NoticeFlags.Count() > 0)
                    {
                        foreach (var noticeFlag in NoticeFlags)
                        {
                            PropertyInfo info = properties.SingleOrDefault(x => x.Name == noticeFlag.Name);

                            if (info != null)
                            {
                                BindSlaveProperty(root, root);

                                var tmp = info.GetValue(root);

                                if (root is INotifyPropertyChanged notify)
                                {
                                    notify.PropertyChanged += (sender, e) =>
                                    {
                                        if (NoticeFlags.Any(t => t.Name == e.PropertyName))
                                        {
                                            var senderType = sender.GetType();
                                            PropertyInfo senderProperty = senderType.GetProperty(e.PropertyName);
                                            BindProperty(sender, sender, senderProperty);
                                        }
                                    };
                                }
                            }
                        }
                    }
                }
                return root;
            }
            catch (Exception)
            {
                return root;
            }
        }

        private static void BindProperty<T>(T source, object root, PropertyInfo info)
        {
            if (info.PropertyType.IsGenericType && info.PropertyType.GetGenericTypeDefinition() == typeof(ObservableCollection<>))
            {
                var tmp = info.GetValue(source);

                if (tmp != null && tmp is INotifyCollectionChanged notifyCollectionChanged)
                {
                    notifyCollectionChanged.CollectionChanged += (sender, e) =>
                    {
                        if (e.NewItems != null && e.NewItems.Count > 0)
                        {
                            BindSlaveProperty(e.NewItems[0], root);

                            if (e.NewItems[0] != null && e.NewItems[0] is INotifyPropertyChanged notify)
                            {
                                if (root is INotifyHolder notifyHolder)
                                {
                                    notify.PropertyChanged += (s, e) =>
                                    {
                                        notifyHolder.AfterPropertyChangedNotified(s, info.Name + "." + e.PropertyName + "发生了变化");
                                    };
                                }
                            }
                        }
                    };

                    var arr = (IEnumerable<object>)tmp;
                    foreach (var item in arr)
                    {
                        if (item is INotifyPropertyChanged notify && root is INotifyHolder notifyHolder)
                        {
                            BindSlaveProperty(item, root);
                            notify.PropertyChanged += (sender, e) =>
                            {
                                notifyHolder.AfterPropertyChangedNotified(sender, info.Name + "." + e.PropertyName + "发生了变化");
                            };
                        }
                    }
                }
            }
            else if (info.PropertyType.GetInterfaces().Contains(typeof(INotifyPropertyChanged)))
            {
                var tmp = info.GetValue(source);
                if (tmp != null && tmp is INotifyPropertyChanged notify)
                {
                    BindSlaveProperty(tmp, root);
                    if (root is INotifyHolder notifyHolder)
                    {
                        notify.PropertyChanged += (sender, e) =>
                        {
                            notifyHolder.AfterPropertyChangedNotified(sender, info.Name + "." + e.PropertyName + "发生了变化");
                        };
                    }
                }
            }
        }
    }
}
