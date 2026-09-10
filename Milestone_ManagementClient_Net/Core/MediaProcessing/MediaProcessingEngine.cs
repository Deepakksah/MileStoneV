using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MilestoneManagementClient.Core.MediaProcessing
{
    public enum ScalingMode
    {
        Letterbox,
        StretchToFit,
        CropToFill,
        PixelExact
    }

    public static class MediaProcessingEngine
    {
        public static Rectangle CalculateRenderBounds(Rectangle canvasBounds, Size streamSize, ScalingMode mode)
        {
            if (canvasBounds.Width <= 0 || canvasBounds.Height <= 0 || streamSize.Width <= 0 || streamSize.Height <= 0)
                return canvasBounds;

            if (mode == ScalingMode.StretchToFit)
                return canvasBounds;

            float streamAspect = (float)streamSize.Width / streamSize.Height;
            float canvasAspect = (float)canvasBounds.Width / canvasBounds.Height;

            int targetW, targetH;

            if (mode == ScalingMode.Letterbox)
            {
                if (canvasAspect > streamAspect)
                {
                    targetH = canvasBounds.Height;
                    targetW = (int)(targetH * streamAspect);
                }
                else
                {
                    targetW = canvasBounds.Width;
                    targetH = (int)(targetW / streamAspect);
                }

                int x = canvasBounds.X + (canvasBounds.Width - targetW) / 2;
                int y = canvasBounds.Y + (canvasBounds.Height - targetH) / 2;
                return new Rectangle(x, y, targetW, targetH);
            }

            return canvasBounds;
        }

        public static void ApplyVideoEnhancements(Graphics g, Rectangle destRect, bool enableDenoise = true, bool enableOsd = true)
        {
            if (g == null) return;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
        }
    }
}
