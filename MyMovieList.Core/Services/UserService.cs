using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyMovieList.Core.Contracts;
using MyMovieList.Core.Models;
using MyMovieList.Infrastructure.Data;
using MyMovieList.Infrastructure.Data.Repositories;

namespace MyMovieList.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationDbRepository repo;

        public UserService(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IdentityUser> GetUserById(string id)
        {
            return await repo.GetByIdAsync<IdentityUser>(id);
        }

        public async Task<UserEditViewModel> GetUserForEdit(string id)
        {
            var user = await repo.GetByIdAsync<IdentityUser>(id);

            return new UserEditViewModel()
            {
                Id = user.Id,
                UserName=user.UserName
            };
        }

        public async Task<string> GetUserId(string username)
        {
            return await repo.All<IdentityUser>()
                .Where(u => u.UserName == username)
                .Select(u=>u.Id)
                .FirstAsync();
        }

        public async Task<IEnumerable<UserListViewModel>> GetUsers()
        {
            return await repo.All<IdentityUser>()
                .Select(u => new UserListViewModel()
                {
                    Email = u.Email,
                    Id = u.Id,
                    UserName = u.UserName
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateUser(UserEditViewModel model)
        {
            bool result = false;
            var user = await repo.GetByIdAsync<IdentityUser>(model.Id);

            if (user != null)
            {
                user.UserName = model.UserName;

                await repo.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<bool> Suggestion(UserSuggestionViewModel model)
        {
            bool isSaved = false;
            UserSuggestion userSuggestion = new UserSuggestion
            {
                Title= model.Title,
                Type = model.Type,
            };

            try
            {
                await repo.AddAsync<UserSuggestion>(userSuggestion);
                await repo.SaveChangesAsync();
                isSaved = true;
            }
            catch (Exception)
            {
            }

            return isSaved;
        }

        public async Task<IEnumerable<UserSuggestionViewModel>> GetSuggestions()
        {
            return await repo.All<UserSuggestion>()
                .Select(us => new UserSuggestionViewModel
                {
                    Title = us.Title,
                    Type = us.Type,
                    Id = us.Id
                })
                .ToListAsync();
        }

        public async Task DeleteSuggestion(string suggestionId)
        {           
            try
            {
                Guid id = new Guid(suggestionId);
                await repo.DeleteAsync<UserSuggestion>(id);
                await repo.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
        }
    }
}
