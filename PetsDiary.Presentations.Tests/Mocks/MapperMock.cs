using AutoMapper;
using Moq;

namespace PetsDiary.Presentations.Tests.Mocks
{
    public class MapperMock : Mock<IMapper>
    {
        public void SetupMap<Type>(object source, Type mappedType)
        {
            Setup(x => x.Map<Type>(source))
                .Returns(mappedType);
        }

        public void SetupMap<Type>( Type mappedType)
        {
            Setup(x => x.Map<Type>(It.IsAny<object>()))
                .Returns(mappedType);
        }
    }
}