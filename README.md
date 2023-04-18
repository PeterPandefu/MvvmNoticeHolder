# MvvmNoticeHolder

该库解决WPF MVVM 模式下，当ViewModel的属性一个多层级的实体类或者集合时，这个实体类的属性变化或者集合内元素变化通知到ViewModel。

## 前提条件

1. ViewModel需要实现 `INotifyHolder` 接口，用于通知ViewModel刷新。

   ```c#
   namespace MvvmNoticeHolderLib
   {
       public interface INotifyHolder
       {
           void AfterPropertyChangedNotified(object sender, string info);
       }
   }
   ```

   

2. ViewMolel所持有的实体类模型，必须实现 `INotifyCollectionChanged` 接口。

   ```c#
    namespace System.ComponentModel
   {
       //
       // 摘要:
       //     Notifies clients that a property value has changed.
       public interface INotifyPropertyChanged
       {
           //
           // 摘要:
           //     Occurs when a property value changes.
           event PropertyChangedEventHandler? PropertyChanged;
       }
   }
   ```

   

3. ViewModel所持有的泛型集合类型，必须实现 `INotifyCollectionChanged` ，其填充集合的模型也必须实现 `INotifyCollectionChanged` 接口。

   ```c#
   namespace System.Collections.Specialized
   {
       //
       // 摘要:
       //     Notifies listeners of dynamic changes, such as when an item is added and removed
       //     or the whole list is cleared.
       public interface INotifyCollectionChanged
       {
           //
           // 摘要:
           //     Occurs when the collection changes.
           event NotifyCollectionChangedEventHandler? CollectionChanged;
       }
   }
   ```



## 示例

1. 初始化ViewModel是，需要 `Binding` 一下。

   ```c#
   namespace MvvmNoticeHolder
   {
       public class ViewModels
       {
   
           private static PersonViewModel personViewModel;
           static ViewModels()
           {
               SimpleIoc.Default.Register<PersonViewModel>();
               personViewModel = NotifyHolderBindingManager.Binding(SimpleIoc.Default.GetInstance<PersonViewModel>(), SimpleIoc.Default.GetInstance<PersonViewModel>());
           }
   
           public static PersonViewModel PersonViewModel { get { return personViewModel; } }
   
       }
   }
   ```

   

2. ViewModel实现 `INotifyHolder` 接口，并且标记出你想要通知到ViewModel的Property。

   ```c#
   namespace MvvmNoticeHolder
   {
       [NoticeFlag(nameof(Persons), typeof(ObservableCollection<Person>))]
       [NoticeFlag(nameof(OnePerson), typeof(Person))]
       public class PersonViewModel : ViewModelBase, INotifyHolder{
          
           public void AfterPropertyChangedNotified(object? sender, string info)
           {
               //To do something...
           }
           
           private ObservableCollection<Person> perons = new ObservableCollection<Person>();
           public ObservableCollection<Person> Persons
           {
               get { return perons; }
               set
               {
                   perons = value; RaisePropertyChanged();
               }
           }
   
           private Person onePerson = new Person();
   
           public Person OnePerson
           {
               get { return onePerson; }
               set { onePerson = value; RaisePropertyChanged(); }
           }
       }
   }
   ```

   

3. 在Model中有多层级关系想要通知到ViewModel时，也需要标记出想要通知到ViewModel的Property。

   ```c#
   namespace MvvmNoticeHolder
   {
   
       [NoticeFlag(nameof(Child), typeof(Person))]
       public class Person : ObservableObject
       {
           private int age;
   
           public int Age
           {
               get { return age; }
               set
               {
                   age = value;
                   RaisePropertyChanged();
               }
           }
   
           private string name;
   
           public string Name
           {
               get { return name; }
               set
               {
                   name = value;
                   RaisePropertyChanged();
               }
           }
   
           private Person child;
   
           public Person Child
           {
               get { return child; }
               set { child = value; RaisePropertyChanged(); }
           }
       }
   }
   ```
