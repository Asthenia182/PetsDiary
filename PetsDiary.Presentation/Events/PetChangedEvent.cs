using Prism.Events;
using System;

namespace PetsDiary.Presentation.Events
{
    public class PetChangedEvent : PubSubEvent<PetChangedEventArgs> { }

    public class PetChangedEventArgs : EventArgs
    {
        public PetChangedEventArgs(string petName, bool isSaved)
        {
            PetName = petName;
            IsSaved = isSaved;
        }

        public string PetName { get; set; }

        public bool IsSaved { get; set; }
    }
}