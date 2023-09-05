using AirplaneManagementApp.BackEnd.DomainLayer.Model;
using AirplaneManagementApp.BackEnd.ServiceLayer.Helpers;

namespace AirplaneManagementApp.BackEnd.ServiceLayer.Interface;

public interface ITicketService
{
    Response<Ticket> Create(Ticket ticket);
    Response<Ticket> Update(Ticket flight);
    Response<bool> Delete(int id);
    Response<List<Ticket>> GetAll();
    Response<Ticket> GetById(int id);
     List<Ticket> GetAllList();
}
