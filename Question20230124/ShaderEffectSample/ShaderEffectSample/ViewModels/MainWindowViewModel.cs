using Prism.Mvvm;
using Reactive.Bindings;
using ShaderEffectSample.Effects;
using ShaderEffectSample.Helpers;
using ShaderEffectSample.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace ShaderEffectSample.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public ReactiveCommand BlurEffectImage { get; }
        public ReactiveCommand MosaicEffectImage { get; }

        public MainWindowViewModel()
        {
            BlurEffectImage = new ReactiveCommand().WithSubscribe(() =>
            {
                var mosaic = new MosaicViewModel();
                mosaic.Bitmap = new BitmapImage(new System.Uri("../net7.0-windows10.0.22000.0/Assets/Fm2MhTmakAAlBhz.jpg", System.UriKind.Relative));
                var drawingVisual = new DrawingVisual();
                using (var context = drawingVisual.RenderOpen())
                {
                    var mainwindow = App.Current.MainWindow as MainWindow;

                    //When I add an Image object from ViewModel, the blur effect doesn't work for some reason
                    var image = new Image();
                    image.Source = mosaic.Bitmap;
                    image.DataContext = mosaic;
                    image.Width = mosaic.Bitmap.Width;
                    image.Height = mosaic.Bitmap.Height;
                    RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
                    mainwindow.dummyCC.Content = image;
                    image.Effect = new BlurEffect()
                    {
                        Radius = 30,
                        KernelType = KernelType.Gaussian,
                    };
                    image.UpdateLayout();

                    BitmapCacheBrush _brush = new BitmapCacheBrush(image);

                    //When adding an Image object from ViewModel, the code prepared for testing does not apply the blur effect for some reason
                    //var label = new Label();
                    //label.Content = "TEST";
                    //label.FontSize = 12;
                    //label.Foreground = new SolidColorBrush(Colors.Red);
                    //label.Background = new SolidColorBrush(Colors.White);
                    //var _vbrush = new VisualBrush(label);
                    //_vbrush.Viewport = new Rect(0, 0, 0.25, 0.25);
                    //_vbrush.TileMode = TileMode.FlipXY;
                    //var rectangle = new Rectangle();
                    //rectangle.Fill = _vbrush;
                    //rectangle.Width = 1000;
                    //rectangle.Height = 1000;
                    //var effect = new BlurEffect();
                    //effect.Radius = 30;
                    //effect.KernelType = KernelType.Gaussian;
                    //rectangle.Effect = effect;
                    //mainwindow.test.Content = rectangle;
                    //rectangle.UpdateLayout();
                    //BitmapCacheBrush _brush = new BitmapCacheBrush(rectangle);

                    context.DrawRectangle(_brush, null, new Rect(0, 0, mosaic.Bitmap.Width, mosaic.Bitmap.Height));
                }
                OpenCvSharpHelper.ImShow("BlurEffectResult", drawingVisual, (int)mosaic.Bitmap.Width, (int)mosaic.Bitmap.Height);
            });
            MosaicEffectImage = new ReactiveCommand().WithSubscribe(() =>
            {
                var mosaic = new MosaicViewModel();
                mosaic.Bitmap = new BitmapImage(new System.Uri("../net7.0-windows10.0.22000.0/Assets/Fm2MhTmakAAlBhz.jpg", System.UriKind.Relative));
                var drawingVisual = new DrawingVisual();
                using (var context = drawingVisual.RenderOpen())
                {
                    var mainwindow = App.Current.MainWindow as MainWindow;

                    //Why does the mosaic effect not work when adding an Image object from ViewModel
                    var image = new Image();
                    image.Source = mosaic.Bitmap;
                    image.DataContext = mosaic;
                    image.Width = mosaic.Bitmap.Width;
                    image.Height = mosaic.Bitmap.Height;
                    RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
                    MosaicEffect effect = new MosaicEffect();
                    effect.Width = mosaic.Bitmap.Width;
                    effect.Height = mosaic.Bitmap.Height;
                    effect.Cp = 10;
                    effect.Rp = 10;
                    effect.Bytecode = mosaic.Bytecode.Value;
                    image.Effect = effect;
                    mainwindow.dummyCC2.Content = image;
                    image.UpdateLayout();
                    BitmapCacheBrush _brush = new BitmapCacheBrush(image);

                    context.DrawRectangle(_brush, null, new Rect(0, 0, mosaic.Bitmap.Width, mosaic.Bitmap.Height));
                }
                OpenCvSharpHelper.ImShow("MosaicEffectResult", drawingVisual, (int)mosaic.Bitmap.Width, (int)mosaic.Bitmap.Height);
            });
        }
    }
}
