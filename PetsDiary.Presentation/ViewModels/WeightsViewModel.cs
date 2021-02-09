namespace PetsDiary.Presentation.ViewModels
{
    public class WeightsViewModel : BaseViewModel
    {
        private double actualWeight;

        public double ActualWeight
        {
            get { return actualWeight; }
            set
            {
                actualWeight = value;
                RaisePropertyChanged();
            }
        }

        //podzielic na kilog i gram, zapisywac jako double
    }
}