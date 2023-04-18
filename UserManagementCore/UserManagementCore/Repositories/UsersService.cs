﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UserManagementCore.Interfaces;
using UserManagementCore.Models;
using UserManagementEntityModel.ViewModel;

namespace UserManagementCore.Repositories
{
    public class UsersService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUserValidator<ApplicationUser> _userValidator;
        private readonly IPasswordValidator<ApplicationUser> _passwordValidator;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public UsersService(IUserRepository repository, IMapper mapper, IUserValidator<ApplicationUser> userValidator, IPasswordValidator<ApplicationUser> passwordValidator, IPasswordHasher<ApplicationUser> passwordHasher, SignInManager<ApplicationUser> signInManager)
        {
            _repository = repository;
            _mapper = mapper;
            _userValidator = userValidator;
            _passwordValidator = passwordValidator;
            _passwordHasher = passwordHasher;
            _signInManager = signInManager;
        }

        public IQueryable<ApplicationUser> Get()
        {

            var returnedList = new List<ApplicationUser>();
            _repository.Get().ToList().ForEach(u =>
            {                
               // returnedList.Add(_mapper.Map<ApplicationUser, vm_ApplicationUser>(u));
            });

            return returnedList.AsQueryable();
        }

        public ApplicationUser GetByEmail(string email)
        {
            return _mapper.Map<ApplicationUser, ApplicationUser>(_repository.GetByEmail(email));
        }

        public Task<IdentityResult> Create(ApplicationUser user, string password)
        {
            return _repository.Create(_mapper.Map<ApplicationUser, ApplicationUser>(user), password);
        }

        public async Task<IdentityResult> Delete(ApplicationUser user)
        {
            return await _repository.Delete(_mapper.Map<ApplicationUser, ApplicationUser>(user));
        }

        public async Task<IdentityResult> Update(ApplicationUser user)
        {
            return await _repository.Update(_mapper.Map<ApplicationUser, ApplicationUser>(user));
        }

        public async Task<IdentityResult> ValidatePassword(ApplicationUser user, string password)
        {
            var appUser = _mapper.Map<ApplicationUser, ApplicationUser>(user);
            return await _passwordValidator.ValidateAsync(_repository.GetUserManager(), appUser, password);
        }

        public async Task<IdentityResult> ValidateUser(ApplicationUser user)
        {
            var appUser = _mapper.Map<ApplicationUser, ApplicationUser>(user);
            return await _userValidator.ValidateAsync(_repository.GetUserManager(), appUser);
        }

        public string HashPassword(ApplicationUser user, string password)
        {
            var appUser = _mapper.Map<ApplicationUser, ApplicationUser>(user);
            return _passwordHasher.HashPassword(appUser, password);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password, bool lockoutOnFailure, bool isPersistent)
        {
            var appUser = _mapper.Map<ApplicationUser, ApplicationUser>(user);
            return await _signInManager.PasswordSignInAsync(appUser, password, isPersistent, lockoutOnFailure);
        }


    }
}
