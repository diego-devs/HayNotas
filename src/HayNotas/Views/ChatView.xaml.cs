using System.Collections.Specialized;
using System.Windows.Controls;

namespace HayNotas.Views;

public partial class ChatView : UserControl
{
    public ChatView()
    {
        InitializeComponent();
        Loaded += (_, _) =>
        {
            if (MessagesListBox.ItemsSource is INotifyCollectionChanged collection)
            {
                collection.CollectionChanged += (_, _) =>
                {
                    if (MessagesListBox.Items.Count > 0)
                        MessagesListBox.ScrollIntoView(MessagesListBox.Items[^1]);
                };
            }
        };
    }
}
