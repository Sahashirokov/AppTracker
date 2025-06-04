using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LauncherApp.Services.ManagerWindow;

public class CustomMenuRenderer : ToolStripProfessionalRenderer
{
    public CustomMenuRenderer() : base(new CustomColors()) { }

    protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
    {
        // Не рисуем стандартную границу
    }

    protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
    {
        // Прозрачный фон для элементов
        if (e.Item.Selected)
        {
            using (var brush = new SolidBrush(Color.FromArgb(50, 255, 255, 255)))
            {
                e.Graphics.FillRectangle(brush, new Rectangle(Point.Empty, e.Item.Size));
            }
        }
        else
        {
            base.OnRenderMenuItemBackground(e);
        }
    }

    protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
    {
        // Закругленный фон для всего меню
        using (var path = GetRoundedPath(e.AffectedBounds, 10))
        using (var brush = new SolidBrush(e.ToolStrip.BackColor))
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillPath(brush, path);
        }
    }

    private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
    {
        var path = new GraphicsPath();
        path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
        path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
        path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
        path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
        path.CloseFigure();
        return path;
    }
}

class CustomColors : ProfessionalColorTable
{
    public override Color MenuBorder => Color.Transparent;
    public override Color MenuItemBorder => Color.Transparent;
    public override Color SeparatorDark => Color.Transparent;
    public override Color SeparatorLight => Color.Transparent;
}