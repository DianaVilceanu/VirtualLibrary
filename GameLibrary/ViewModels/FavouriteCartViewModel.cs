using GameLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameLibrary.ViewModels
{
    public class FavouriteCartViewModel
    {
        public List<Favourite> CartItems { get; set; }
        public float? CartTotal { get; set; }
    }
}
