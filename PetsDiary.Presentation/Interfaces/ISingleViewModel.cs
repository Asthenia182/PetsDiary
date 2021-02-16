namespace PetsDiary.Presentation.Interfaces
{
    public interface ISingleViewModel: IBaseViewModel
    {
        int? Id { get; set; }
        bool IsDirty { get; set; }
        int PetId { get; set; }

        void Cancel();

        bool IsValid();

        bool Save();

        void SetValuesByOriginValues();

        bool Update();
    }
}