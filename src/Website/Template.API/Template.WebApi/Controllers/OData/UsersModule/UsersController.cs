using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Template.Application.UsersModule.Models;
using Template.Domain.TenantsModule.Enums;
using Template.Domain.UsersModule;
using Template.WebApi.Attributes;

namespace Template.WebApi.Controllers.OData.UsersModule
{
    public class UsersController : ODataController
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UsersController(
            IMapper mapper,
            IUserRepository userRepository
        )
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [AuthorizeRoles(Role.Manager)]
        [HttpGet]
        [EnableQuery]
        [ProducesResponseType(typeof(IQueryable<UserModel>), 200)]
        public IActionResult Get()
        {
            return Ok(_userRepository.RetrieveOData<UserModel>(_mapper));
        }
    }
}
