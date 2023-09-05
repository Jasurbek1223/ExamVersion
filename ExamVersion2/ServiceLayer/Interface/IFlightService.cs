using AirplaneManagementApp.BackEnd.DomainLayer.Model;
using AirplaneManagementApp.BackEnd.ServiceLayer.Helpers;

namespace AirplaneManagementApp.BackEnd.ServiceLayer.Interface;

public interface IFlightService
{
    Response<Flight> Create(Flight flight, int airplaneId);
    Response<Flight> Update(Flight flight);
    Response<bool> Delete(int id);
    Response<List<Flight>> GetAll();
    Response<Flight> GetById(int id);
    bool CheckNumOfSeats(Flight flight);
}
