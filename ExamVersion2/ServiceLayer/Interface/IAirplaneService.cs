using AirplaneManagementApp.BackEnd.DomainLayer.Model;
using AirplaneManagementApp.BackEnd.ServiceLayer.Helpers;

namespace AirplaneManagementApp.BackEnd.ServiceLayer.Interface;

internal interface IAirplaneService
{
    Response<Airplane> Create(Airplane airplane);
    Response<Airplane> Update(Airplane airplane);
    Response<bool> Delete(int id);
    Response<List<Airplane>> GetAll();
    Response<Airplane> GetById(int id);
}
