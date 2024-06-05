using GameLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.Repository
{
    public interface IOrderRepository
    {
        void CreateOrder(Order order);
    }
}
