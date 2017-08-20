using System;
using Xamarin.Forms;

namespace SoundApp.GUI.Components
{
    public class DeletableCell : TextCell
    {
        public static event Action<TrackViewTextItem> DeleteHandler;

        public DeletableCell() : base()
        {
            var deleteAction = new MenuItem { Text = "Delete", IsDestructive = true };
            deleteAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            deleteAction.Parent = this;
            deleteAction.Clicked += (sender, e) => {
                var mi = ((MenuItem)sender);
                var cell = (DeletableCell)mi.Parent;
                var textItem = (TrackViewTextItem)cell.BindingContext;


                DeleteHandler?.Invoke(textItem);
            };

            ContextActions.Add(deleteAction);
        }
    }
}