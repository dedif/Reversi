using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.DAL
{
    public interface IPlayerGameDal
    {
        ActionResult<bool> JoinGame(Player player, int gameId);
    }
}
