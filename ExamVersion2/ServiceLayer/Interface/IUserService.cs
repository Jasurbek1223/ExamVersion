using AirplaneManagementApp.BackEnd.DomainLayer.Model;
using AirplaneManagementApp.BackEnd.ServiceLayer.Helpers;

namespace AirplaneManagementApp.BackEnd.ServiceLayer.Interface;

public interface IUserService
{
    Response<User> Create(User user);
    Response<User> Update(User user);
    Response<bool> Delete(int id);
    Response<List<User>> GetAll();
    Response<User> GetByEmail(string email);
}
