using PetsDiary.Common.Models;
using System.Collections.Generic;

namespace PetsDiary.Common.Interfaces
{
    public interface IPetsData
    {
        AnimalModel AddAnimal(AnimalModel model);

        AnimalModel DeleteAnimalById(int id);

        IEnumerable<AnimalModel> GetPets();

        AnimalModel GetAnimalById(int id);

        AnimalModel UpdateAnimal(AnimalModel updatedAnimal);
    }
}