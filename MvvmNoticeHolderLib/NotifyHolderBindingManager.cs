using System;
using System.Collections.Generic;
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
        public static T Binding<T>(T source, object root)
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
                                if (info.PropertyType.Name.Equals("ObservableCollection`1"))
                                {
                                    var tmp = info.GetValue(source, null);

                                    if (tmp != null && tmp is INotifyCollectionChanged notifyCollectionChanged)
                                    {
                                        notifyCollectionChanged.CollectionChanged += (sender, e) =>
                                        {
                                            if (e.NewItems != null && e.NewItems.Count > 0)
                                            {
                                                Binding(e.NewItems[0], root);

                                                if (e.NewItems[0] != null && e.NewItems[0] is INotifyPropertyChanged notify)
                                                {
                                                    if (root is INotifyHolder notifyHolder)
                                                    {
                                                        notify.PropertyChanged += (s, e) =>
                                                        {
                                                            notifyHolder.AfterPropertyChangedNotified(s, noticeFlag.Name + "." + e.PropertyName + "发生了变化");
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
                                                Binding(item, root);
                                                notify.PropertyChanged += (sender, e) =>
                                                {
                                                    notifyHolder.AfterPropertyChangedNotified(sender, noticeFlag.Name + "." + e.PropertyName + "发生了变化");
                                                };
                                            }
                                        }
                                    }
                                }
                                else if (info.PropertyType.IsClass)
                                {
                                    var tmp = info.GetValue(source, null);
                                    if (tmp != null && tmp is INotifyPropertyChanged notify)
                                    {
                                        Binding(tmp, root);
                                        if (root is INotifyHolder notifyHolder)
                                        {
                                            notify.PropertyChanged += (sender, e) =>
                                            {
                                                notifyHolder.AfterPropertyChangedNotified(sender, noticeFlag.Name + "." + e.PropertyName + "发生了变化");
                                            };
                                        }
                                    }
                                }
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
    }
}
