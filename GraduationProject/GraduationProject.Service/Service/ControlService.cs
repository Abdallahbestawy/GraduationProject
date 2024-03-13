using GraduationProject.Repository.Repository;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class ControlService : IControlService
    {

        private readonly UnitOfWork _unitOfWork;
        public ControlService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public Task Test()
        {
            throw new NotImplementedException();
        }
    }
}
