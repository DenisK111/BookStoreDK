using BookStoreDK.BL.Interfaces;
using BookStoreDK.DL.Intefraces;
using BookStoreDK.Models.Models.Users;

namespace BookStoreDK.BL.Services
{
    public class EmployeeService : IEmployeeService,IUserInfoService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserInfoStore _userInfoStore;

        public EmployeeService(IEmployeeRepository employeeRepository, IUserInfoStore userInfoStore)
        {
            _employeeRepository = employeeRepository;
            _userInfoStore = userInfoStore;
        }

        public async Task AddEmployee(Employee employee)
        {
            await _employeeRepository.AddEmployee(employee);
        }

        public async Task<bool> CheckEmployee(int id)
        {
            return await _employeeRepository.CheckEmployee(id);
        }

        public async Task DeleteEmployee(int id)
        {
            await _employeeRepository.DeleteEmployee(id);
        }

        public async Task<IEnumerable<Employee>> GetEmployeeDetails()
        {
            return await _employeeRepository.GetEmployeeDetails();
        }

        public async Task<Employee?> GetEmployeeDetails(int id)
        {
            return await _employeeRepository.GetEmployeeDetails(id);
        }

        public async Task<UserInfo?> GetUserInfoAsync(string email, string password)
        {
            return await _userInfoStore.GetUserInfoAsync(email,password);
        }

        public async Task UpdateEmployee(Employee employee)
        {
             await _employeeRepository.UpdateEmployee(employee);
        }
    }
}
