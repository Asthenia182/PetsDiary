namespace PetsDiary.Common.Models
{
    public class NoteModel
    {
        public int? Id { get; set; }
        public int PetId { get; set; }
        public string Note { get; set; }
    }
}