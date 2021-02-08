using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace PetsDiary.Presentation.Controls
{
    /// <summary>
    /// Interaction logic for PopupWithTopArrowControl.xaml
    /// </summary>
    public partial class PopupWithTopArrowControl : UserControl
    {
        public static readonly DependencyProperty InnerContentProperty = DependencyProperty.Register(
            name: nameof(InnerContent),
            propertyType: typeof(UIElement),
            ownerType: typeof(PopupWithTopArrowControl),
            typeMetadata: new UIPropertyMetadata(null));

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(
            name: nameof(IsOpen),
            propertyType: typeof(bool),
            ownerType: typeof(PopupWithTopArrowControl),
            typeMetadata: new UIPropertyMetadata(false));

        public static readonly DependencyProperty PopupPlacementTargetProperty = DependencyProperty.Register(
            name: nameof(PopupPlacementTarget),
            propertyType: typeof(UIElement),
            ownerType: typeof(PopupWithTopArrowControl),
            typeMetadata: new UIPropertyMetadata(null));

        public static readonly DependencyProperty ShowBasketStyleProperty = DependencyProperty.Register(
            name: nameof(ShowBasketStyle),
            propertyType: typeof(bool),
            ownerType: typeof(PopupWithTopArrowControl),
            typeMetadata: new UIPropertyMetadata(false));

        public static readonly DependencyProperty StaysOpenProperty = DependencyProperty.Register(
            name: nameof(StaysOpen),
            propertyType: typeof(bool),
            ownerType: typeof(PopupWithTopArrowControl),
            typeMetadata: new UIPropertyMetadata(false));

        public PopupWithTopArrowControl()
        {
            InitializeComponent();

            Pop.CustomPopupPlacementCallback =
                new CustomPopupPlacementCallback(placePopup);
        }

        /// <summary>
        /// Zawartosc wyswietlana w popupie
        /// </summary>
        public UIElement InnerContent
        {
            get { return (UIElement)GetValue(InnerContentProperty); }
            set { SetValue(InnerContentProperty, value); }
        }

        /// <summary>
        /// true - pokazanie sie popupa
        /// </summary>
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        /// <summary>
        /// Kontrolka pod ktora ma sie pojawic popup
        /// </summary>
        public UIElement PopupPlacementTarget
        {
            get { return (UIElement)GetValue(PopupPlacementTargetProperty); }
            set { SetValue(PopupPlacementTargetProperty, value); }
        }

        /// <summary>
        /// Czy popup ma sie wyswietlic lekko zmodyfikowany pod wyglad koszyka (zmiana koloru strzalki i paddingu)
        /// </summary>
        public bool ShowBasketStyle
        {
            get { return (bool)GetValue(ShowBasketStyleProperty); }
            set { SetValue(ShowBasketStyleProperty, value); }
        }

        /// <summary>
        /// true - popup nie zamyka sie gdy sie kliknie poza jego obszar
        /// </summary>
        public bool StaysOpen
        {
            get { return (bool)GetValue(StaysOpenProperty); }
            set { SetValue(StaysOpenProperty, value); }
        }

        public CustomPopupPlacement[] placePopup(Size popupSize, Size targetSize, Point offset)
        {
            CustomPopupPlacement placement;

            //odgleglosc strzalki do najblizszego konca (15) + rozpoczecie strzalki do srodka strzalki (10)
            var arrowCenterToEndLenght = 25;

            placement = new CustomPopupPlacement(new Point(-((popupSize.Width - targetSize.Width / 2) - arrowCenterToEndLenght), targetSize.Height), PopupPrimaryAxis.Vertical);

            CustomPopupPlacement[] ttplaces =
                    new CustomPopupPlacement[] { placement };

            return ttplaces;
        }
    }
}