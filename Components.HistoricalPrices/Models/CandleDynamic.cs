using System.Windows.Media;

namespace DeepInsights.Components.HistoricalPrices.Models
{
    public class CandleDynamic
    {
        public Brush Brush { get; private set; }
        public ImageSource ImageSource { get; private set; }

        public CandleDynamic(Brush brush, ImageSource imageSource)
        {
            Brush = brush;
            ImageSource = imageSource;
        }
    }
}
