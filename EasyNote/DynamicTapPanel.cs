using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows;

namespace EasyNote
{
        public class DynamicTapPanel : TabPanel
        {
            protected override Size MeasureOverride(Size availableSize)
            {
                double maxTabWidth = 180;
                double minTabWidth = 80;
                int count = InternalChildren.Count;

                double tabWidth = maxTabWidth;
                double totalWidth = count * maxTabWidth;

                if (totalWidth > availableSize.Width)
                {
                    tabWidth = Math.Max(minTabWidth, availableSize.Width / count);
                }

                foreach (UIElement child in InternalChildren)
                {
                    child.Measure(new Size(tabWidth, availableSize.Height));
                }

                return base.MeasureOverride(availableSize);
            }

            protected override Size ArrangeOverride(Size finalSize)
            {
                double maxTabWidth = 180;
                double minTabWidth = 80;
                int count = InternalChildren.Count;

                double tabWidth = maxTabWidth;
                double totalWidth = count * maxTabWidth;

                if (totalWidth > finalSize.Width)
                {
                    tabWidth = Math.Max(minTabWidth, finalSize.Width / count);
                }

                double x = 0;
                foreach (UIElement child in InternalChildren)
                {
                    child.Arrange(new Rect(x, 0, tabWidth, finalSize.Height));
                    x += tabWidth;
                }

                return finalSize;
            }
        }
}
