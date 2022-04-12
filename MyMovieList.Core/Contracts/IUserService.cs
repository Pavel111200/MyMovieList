using Microsoft.AspNetCore.Identity;
using MyMovieList.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMovieList.Core.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserListViewModel>> GetUsers();

        Task<UserEditViewModel> GetUserForEdit(string id);

        Task<bool> UpdateUser(UserEditViewModel model);

        Task<IdentityUser> GetUserById(string id);

        Task<string> GetUserId(string username);

        Task<bool> Suggestion(UserSuggestionViewModel model);
    }
}
